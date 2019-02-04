using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//
//http://ibako-study.hateblo.jp/entry/2014/02/09/015759

namespace SMFLibrary {
    // Standard Midi Format のカラオケ用読み込みに特化したライブラリ
    public class StandardMidiFormat {
        // イベントリスト用
        public class Event {
            // 0:何もしない 1:BPM変更 2:ノートON -1:EOF
            public int type;

            // イベント内容。タイプに応じて意味が違う
            // 1:BPM 2:ONにするノート 0,-1:未定義
            // ノートはA0(55Hz)が0になり、半音上がるごとに1上がる
            public int value;

            // イベントの継続時間(ms)
            public int time;
        }


        // 現在読み込んでいるファイルの名前
        public string filename { get; private set; }

        // 現在読み込んでいるMIDIのイベントリスト
        // ただし、BPM変更とノートON/OFF(1音)のみ
        public List<Event> event_list;

        // 4分音符に当たるデルタタイム
        private int time_unit;

        // テンポ(四分音符に当たるms)
        private int tempo;


        // byte列からshort型整数に変換する
        private int ToInt16(byte[] data, int index) {
            byte[] ar = new byte[2];
            ar[0] = data[index];
            ar[1] = data[index + 1];
            if (BitConverter.IsLittleEndian) {
                Array.Reverse(ar);
            }
            return BitConverter.ToInt16(ar, 0);
        }


        // byte列からint型整数に変換する
        private int ToInt32(byte[] data, int index) {
            byte[] ar = new byte[4];
            ar[0] = data[index];
            ar[1] = data[index + 1];
            ar[2] = data[index + 2];
            ar[3] = data[index + 3];
            if (BitConverter.IsLittleEndian) {
                Array.Reverse(ar);
            }
            return BitConverter.ToInt32(ar, 0);
        }


        // bit列からint型整数に変換する
        private int ToInt32FromBits(byte[] data, int index) {
            int value = 0;
            for (int i = 0; i < 32; i++) {
                value += data[index + 31 - i] * (int)Math.Pow(2, i);
            }
            return value;
        }

        //Application.dataPath + "/MIDIData/" + filename
        // バイナリファイルを読み込み、配列で返す
        private byte[] _load_binary(string filename) {
            //System.IO.FileStream fs = new System.IO.FileStream(@filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.FileStream fs = new System.IO.FileStream(Application.dataPath + "/MIDIData/" + filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            int filesize = (int)fs.Length;
            byte[] data = new byte[filesize];
            fs.Read(data, 0, filesize);
            fs.Dispose();

            return data;
        }


        // dataのindexから始まる可変長形式の数値をint型に変換する
        // plus_posは、可変長形式の数値がそこから何byteあったか
        private int _convert_variable_to_int(byte[] data, int index, out int plus_pos) {
            // 可変長形式の最大byte
            int max_variable_size = 5;

            // 変換後の最大byte
            int max_int_size = 4;

            // データサイズ
            int size = 1;
            for (int i = 0; (data[index + i] & (byte)0x80) > 0; i++) {
                size++;
            }
            if (size > max_variable_size) {
                Debug.LogError("variable size is bigger than 5");
                plus_pos = 0;
                return 0;
            }

            // 変換後の各ビットデータ
            byte[] bits = new byte[max_int_size * 8];

            // 埋めるべきビット
            int pos = bits.Length - 1;

            // ビットデータを埋める
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < 7; j++) {
                    byte comp = (byte)(1 << j);
                    if ((data[index + size - 1 - i] & comp) > 0) bits[pos] = 1;
                    pos--;
                }
            }

            // 埋まったビットデータからintに変換して返す
            plus_pos = size;
            return ToInt32FromBits(bits, 0);
        }


        // デルタタイムを実際の時間(ms)に変換する
        private int _convert_delta_time_to_ms(int delta_time) {
            return (int)((float)tempo * (float)delta_time / (float)time_unit);
        }

        // data内の第nトラックがindex[n-1]から始まるとして、最も早く実行される命令を返す
        // before_commandはトラックの直前ステータスバイト、timeはトラックの現在時間
        // indexとtimeの中身は変更される。indexとtimeが-1になったら、そのトラックは終了した
        // どのトラックも終了していたらnullを返す
        private Event _get_command(byte[] data, int[] index, byte[] before_command, int[] time) {
            // 現在の全体の時間
            int world_time = time.Max();

            // 最小の(world_timeからの)デルタタイム・最小デルタタイムのインデックスナンバー・動かすインデックス座標
            int min_delta_time = -1;
            int min_index = -1;
            int min_plus_pos = -1;
            for (int i = 0; i < index.Length; i++) {
                if (index[i] >= 0) {
                    int plus_pos;
                    int delta_time = time[i] + _convert_variable_to_int(data, index[i], out plus_pos) - world_time;
                    if (min_delta_time < 0 || delta_time < min_delta_time) {
                        min_delta_time = delta_time;
                        min_index = i;
                        min_plus_pos = plus_pos;
                    }
                }
            }

            // 次に実行されるトラックが分かったから、その内容を読んでindexと時間を動かす
            if (min_index >= 0) {
                index[min_index] += min_plus_pos;
                time[min_index] += min_delta_time;

                // ステータスバイト
                byte status_byte = data[index[min_index]];
                Event ev = new Event();

                // ステータスバイトの最上位ビットが1なら、具体的命令
                if ((status_byte & (byte)0x80) > 0) {
                    index[min_index]++;
                    before_command[min_index] = status_byte;
                }
                // ステータスバイトの最上位ビットが0なら、前回の命令
                else {
                    status_byte = before_command[min_index];
                }

                byte st = (byte)(status_byte & (byte)0xF0);
                if (st == (byte)0x80) ev.type = 0;  // ノートオフ
                else if (st == (byte)0x90) ev.type = 2;  // ノートオン
                else if (status_byte == (byte)0xFF && data[index[min_index]] == (byte)0x51) ev.type = 1;  // テンポ変更
                else if (status_byte == (byte)0xFF && data[index[min_index]] == (byte)0x2F) ev.type = -1;  // トラック終端
                else  // 無効命令
                {
                    if (st == (byte)0xA0 || st == (byte)0xE0) {
                        index[min_index] += 2;
                    }
                    else if (st == (byte)0xC0 || st == (byte)0xD0) {
                        index[min_index] += 1;
                    }
                    else if (st == (byte)0xB0) {
                        if (data[index[min_index]] == (byte)0x7E && data[index[min_index] + 2] == (byte)0x04) {
                            index[min_index] += 3;
                        }
                        else {
                            index[min_index] += 2;
                        }
                    }
                    else {
                        int plus_pos;
                        int length = _convert_variable_to_int(data, index[min_index] + 1, out plus_pos);
                        index[min_index] += 1 + plus_pos + length;
                    }
                    return _get_command(data, index, before_command, time);
                }

                // ノートオフかオンかテンポ変更の命令だけ抽出したので、諸々の処理
                // 次のデルタタイムが記録されているインデックスナンバー
                int next_delta_time_index = index[min_index];

                // 何もしない処理
                if (ev.type == 0) {
                    next_delta_time_index += 2;
                    index[min_index] += 2;
                }
                // BPM変更の処理
                else if (ev.type == 1) {
                    next_delta_time_index += 5;
                    ev.value = 0;
                    ev.value += (int)((uint)data[index[min_index] + 2] * 65536);
                    ev.value += (int)(uint)(data[index[min_index] + 3] * 256);
                    ev.value += (int)(uint)(data[index[min_index] + 3]);
                    ev.value /= 1000;
                    tempo = ev.value;
                    index[min_index] += 5;
                }
                // ノートオンの処理
                else if (ev.type == 2) {
                    next_delta_time_index += 2;
                    ev.value = (int)data[index[min_index]];
                    index[min_index] += 2;
                }
                // トラック終端の処理
                else if (ev.type == -1) {
                    index[min_index] = -1;
                    time[min_index] = -1;
                }
                int pp;
                // デルタタイムを実際の時間（ms）に変換して代入
                ev.time = _convert_delta_time_to_ms(_convert_variable_to_int(data, next_delta_time_index, out pp));

                return ev;
            }
            return null;
        }


        // ファイルを読み込み、イベントリストを得る
        public void Load(string filename) {
            byte[] data = _load_binary(filename);
            event_list = new List<Event>();
            tempo = 500;  // BPM120

            // ヘッダを読み込む
            if (data.Length < 4) {
                Debug.LogError("File Length is not suitable.");
            }
            else if (!(data[0] == (byte)'M' && data[1] == (byte)'T' && data[2] == (byte)'h' && data[3] == (byte)'d')) {
                Debug.LogError("File is not Standard MIDI Format.");
            }
            else if (data[9] == 2) {
                Debug.LogError("Format 2 is not suitable.");
            }
            else {
                // このMIDIのフォーマット
                int format = ToInt16(data, 8);

                // このMIDIのトラック数
                int track_num = ToInt16(data, 10);

                // 4分音符に当たるデルタタイム
                time_unit = ToInt16(data, 12);

                if (format == 2) {
                    Debug.LogError("format is 2.");
                }
                else if (time_unit < 0) {
                    Debug.LogError("time_unit is minus.");
                }
                else {
                    // 読み込んでいるインデックス（トラックごと）
                    int[] pos = new int[track_num];

                    pos[0] = 22;
                    for (int i = 1; i < track_num; i++) {
                        pos[i] = pos[i - 1] + ToInt32(data, pos[i - 1] - 4) + 8;
                    }

                    // 各トラックの現在の時間
                    int[] time = new int[track_num];

                    // 各トラックの直前命令
                    byte[] before_command = new byte[track_num];

                    // 「何もしない」をイベントリストの先頭に追加
                    int min_time = -1;
                    for (int i = 0; i < track_num; i++) {
                        int plus_pos;
                        int t = _convert_variable_to_int(data, pos[i], out plus_pos);
                        if (min_time < 0 || t < min_time) min_time = t;
                    }
                    Event first_ev = new Event();
                    first_ev.type = 0;
                    first_ev.time = min_time;
                    event_list.Add(first_ev);

                    // 各トラックのうち、現在の時間+デルタタイムが最も小さいものはどれか？
                    // その命令を解析する
                    while (pos[0] != -1 || (pos.Length >= 2 && pos[1] != -1)) {
                        Event ev = _get_command(data, pos, before_command, time);
                        event_list.Add(ev);
                    }

                    // メンバの変更
                    this.filename = filename;

                }
            }
        }
    }
}
