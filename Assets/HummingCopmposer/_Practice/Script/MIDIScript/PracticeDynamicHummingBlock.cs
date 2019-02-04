using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SMFLibrary;

public class PracticeDynamicHummingBlock : MonoBehaviour {

    private StandardMidiFormat smfData = new StandardMidiFormat();
    private int blockNum = 0;
    private float prevLen = 0;
    private float preX = 0;
    private float preZ = 0.0f;
    private float nextLen;
    private float tmpTime;
    private float stdTime = 205; //基準となるブロックの長さ(秒数で表している)ここでブロックの長さを変更可能

    private float measureOneTime = 2000; //１小節当たりの秒単位の時間2000s
    private float measureOneSumTime = 0;

    private float isMeasure = 0; //どの小節なのか

    //32角形の場合
    private float hummingBlockAngleY = 5.625f;

    private float gaugeWidth;

    private GameObject tmpParent;

    private GameObject hbp;

    private GameObject obj;

    //
    public List<AudioClip> audioClip = new List<AudioClip>();

    //2000を超えた場合の持ち越しの長さ
    private float behind = 0;

    // Use this for initialization
    void Start () {

        //鼻歌ブロックの親オブジェクト
        hbp = GameObject.Find("HummingBlock");

        GenerateHummingBlockParentPerMeasure();
        
        //1小節目の親
        tmpParent = GameObject.Find("HBParent01");


        //MIDIデータはAssets/MIDIData内に置く
        smfData.Load("noname5.mid");
        //Debug.Log(smfData.event_list.Count);

        //MIDIデータからブロックを生成
        //ブロックを一個一個
        foreach (StandardMidiFormat.Event a in smfData.event_list) {
            //Debug.Log(a.type + ", " + a.value + ", " + a.time);

            //ブロックを生成
            if ( a.type == 2 ) {
                //ブロックをいい感じに配置するためにごちゃごちゃ計算
                blockNum++; //ブロックに名前を付けるため
                //ブロックのプレハブを準備
                obj = (GameObject)Resources.Load("Prefabs/blockmini");
                //ブロックの音程を判定し、当てはめる
                DecisionPitch(obj, a.value);


                if ( blockNum > 1 ) {
                    //次に配置するブロックのために事前に今のブロックの長さを格納
                    prevLen = tmpTime / stdTime;    //stdTimeで割っているのは、そのままの時間の数値が大きいため。
                                                    //ブロックを配置
                                                    //前のブロックの中心のx値と長さを加算。そこに今のブロックの長さを加算
                    
                    GameObject instance = (GameObject)Instantiate(obj,
                                      new Vector3((preX + prevLen + (a.time / stdTime)) * 1.0f, (a.value - 44) * 1.2f, preZ),
                                      Quaternion.identity);
                    
                    instance.name = "HummingBlock" + blockNum;
                }
                else {
                    //最初のブロックのみに関する処理。原点にブロックを置く
                    preZ = GameObject.Find("tg01").transform.position.z;
                    //prevLen = 0;
                    GameObject instance = (GameObject)Instantiate(obj,
                                      new Vector3((a.time / stdTime), (a.value - 44) * 1.2f, preZ),
                                      Quaternion.identity);
                    instance.name = "HummingBlock" + blockNum;
                }
                
                GameObject hummingBlock = GameObject.Find("HummingBlock" + blockNum);

                //音の時間に合わせてブロックの長さを変える
                
                hummingBlock.gameObject.transform.localScale = new Vector3(
                    hummingBlock.gameObject.transform.localScale.x * a.time / stdTime,
                    hummingBlock.gameObject.transform.localScale.y,
                    hummingBlock.gameObject.transform.localScale.z);
                
                hummingBlock.transform.parent = tmpParent.transform;

                //次に生成するブロックのための変数
                tmpTime = a.time;
                preX = hummingBlock.transform.position.x;
            }
            //音がない時間は空白を作る
            if (a.type == 0 && a.time > 0) {
                prevLen = tmpTime / stdTime;
                preX = preX + prevLen + a.time / stdTime;
                tmpTime = a.time;
                //Debug.Log(blockNum);
            }
            
            //一小節が終わっていないか判定
            measureOneSumTime += a.time;
            if (measureOneSumTime >= measureOneTime) {
                behind = 0;
                //もし１小節の長さ2000を超えていたら次の小節に持ち越し
                if ( measureOneSumTime > 2100 ) {
                    behind = measureOneSumTime - 2000;
                }
                //もし小節が終わっていた場合
                //preX += 5.0f;
                measureOneSumTime = 0 + behind;
                //hummingBlockAngleY += 90;
                isMeasure++;
                //tgxxを探して座標を得る
                if (isMeasure < 10) {
                    preX = GameObject.Find("tg0" + isMeasure).transform.position.x;
                    preZ = GameObject.Find("tg0" + isMeasure).transform.position.z;
                }
                else {
                    preX = GameObject.Find("tg" + isMeasure).transform.position.x;
                    preZ = GameObject.Find("tg" + isMeasure).transform.position.z;
                }

                preX = preX + (behind / stdTime)*0.8f;
                tmpTime = behind;

                //prevLen = 0;
                //tmpTime = 0;

                //何小節目の親か.
                //親を切り替え
                if (isMeasure < 9) {
                    tmpParent = GameObject.Find("HBParent0" + (isMeasure + 1));
                }
                else {
                    tmpParent = GameObject.Find("HBParent" + (isMeasure + 1));
                }
            }

        }

        //帯に沿うように、小節ごとに回転
        RatateHummingBlockParentPerMeasure();
        //大きすぎる鼻歌ブロックをまとめて小さくする
        ShrinkHummingBlockParentPerMeasure();
    }

    // Update is called once per frame
    void Update () {
		
	}

    void GenerateHummingBlockParentPerMeasure() {
        int num = 0; //小節
        //Targetの子だけ走査
        //Targetと同じ座標に鼻歌ブロックようの小節ごとの親を作る
        GameObject parent = GameObject.Find("Target");
        foreach (Transform child in parent.gameObject.transform) {
            num++;
            GameObject emptyObject = new GameObject();
            emptyObject.transform.position = child.position;

            //のちに全部まとめて大きさを変えるため
            emptyObject.transform.parent = hbp.transform;

            if (num < 10) {
                emptyObject.name = "HBParent0" + num;
            }
            else {
                emptyObject.name = "HBParent" + num;
            }
        }
    }
    
    void RatateHummingBlockParentPerMeasure() {
        int tmp = 1;
        GameObject parent = GameObject.Find("HummingBlock");
        foreach (Transform child in parent.gameObject.transform) {
            hummingBlockAngleY = 5.625f * tmp;
            //Debug.Log(hummingBlockAngleY);
            child.transform.Rotate(new Vector3(0, hummingBlockAngleY, 0));
            tmp += 2;
        }
    }
    
    void ShrinkHummingBlockParentPerMeasure() {
        GameObject parent = GameObject.Find("HummingBlock");
        foreach (Transform child in parent.gameObject.transform) {
            float fnum = 100;
            
            child.localScale = new Vector3(
                child.localScale.x / fnum,
                child.localScale.y / fnum,
                child.localScale.z / fnum
                );
            
        }
    }

    void DecisionPitch(GameObject hb, int avalue) {
        int pitchOfAudioClip = 1;
        if (avalue / 12 == 3) {
            switch (avalue % 12) {
                case 0:
                    pitchOfAudioClip = 0;
                    break;
                case 2:
                    pitchOfAudioClip = 1;
                    break;
                case 4:
                    pitchOfAudioClip = 2;
                    break;
                case 5:
                    pitchOfAudioClip = 3;
                    break;
                case 7:
                    pitchOfAudioClip = 4;
                    break;
                case 9:
                    pitchOfAudioClip = 5;
                    break;
                case 11:
                    pitchOfAudioClip = 6;
                    break;
                default:
                    pitchOfAudioClip = 1;
                    break;
            }
        }
        else if (avalue / 12 == 4) {
            switch (avalue % 12) {
                case 0:
                    pitchOfAudioClip = 7;
                    break;
                case 2:
                    pitchOfAudioClip = 8;
                    break;
                case 4:
                    pitchOfAudioClip = 9;
                    break;
                case 5:
                    pitchOfAudioClip = 10;
                    break;
                case 7:
                    pitchOfAudioClip = 11;
                    break;
                case 9:
                    pitchOfAudioClip = 12;
                    break;
                case 11:
                    pitchOfAudioClip = 13;
                    break;
                default:
                    pitchOfAudioClip = 1;
                    break;
            }
        }
        else if (avalue / 12 == 5) {
            switch (avalue % 12) {
                case 0:
                    pitchOfAudioClip = 14;
                    break;
                case 2:
                    pitchOfAudioClip = 15;
                    break;
                case 4:
                    pitchOfAudioClip = 16;
                    break;
                case 5:
                    pitchOfAudioClip = 17;
                    break;
                default:
                    pitchOfAudioClip = 1;
                    break;
            }
        }
        hb.gameObject.GetComponent<AudioSource>().clip = audioClip[pitchOfAudioClip];
    }

}
