using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//参考
//http://tsubakit1.hateblo.jp/entry/2015/02/20/235021

//ターゲット間を等速でスティックを動かす

public class ChaseObject : MonoBehaviour {

    [SerializeField, Range(0, 10)]
    float time = 1;
    [SerializeField]
    Vector3 endPosition;

    //[SerializeField]
    //AnimationCurve curve;
    
    //区間のターゲット
    public Transform target;

    //区間の開始位置と開始時間
    private float startTime;
    private Vector3 startPosition;

    //ターゲットの番号01~
    private int tgNum = 1;

    //一番最初の開始位置
    private Vector3 firstPosition;

    //一時停止の判定
    private bool isPause;
    private float pauseTime;

    //Update()の前に最初に実行される
    void OnEnable() {
        //停止したときように
        firstPosition = transform.position;
        //時間に関する例外処理
        if (time <= 0) {
            transform.position = endPosition;
            enabled = false;
            return;
        }
        //初期化処理
        target = GameObject.Find("tg02").transform;
        endPosition = target.gameObject.transform.position;
        
        //----startTime = Time.timeSinceLevelLoad;
        startTime = Time.realtimeSinceStartup;

        startPosition = transform.position;
        tgNum = 2;
        pauseTime = 0;
        isPause = false;
    }

    //Scriptをdisableにしたときに停止。スティックの位置を初期位置にする
    void OnDisable() {
        transform.position = firstPosition;
    }

    void Update() {
        //ポーズ状態なら
        if (ExampleInputter_PlayStop.isPause) {
            pauseTime += Time.deltaTime;
        }

        //ポーズ状態でないなら
        else if (!ExampleInputter_PlayStop.isPause) {
            //区間の開始位置を出発してからの経過時間
            //---var diff = Time.timeSinceLevelLoad - startTime;
            var diff = Time.realtimeSinceStartup - pauseTime - startTime;

            //ある区間移動しきったときの処理
            if (diff > time) {
                pauseTime = 0;
                transform.position = endPosition;
                //--enabled = false;

                //次のターゲットへ切り替え
                tgNum += 1;
                if (tgNum > 33) {
                    transform.position = endPosition;
                    enabled = false;
                }
                else if (tgNum == 33) {
                    target = GameObject.Find("tg01").transform;
                }
                else if (tgNum > 9) {
                    target = GameObject.Find("tg" + tgNum).transform;
                }
                else if (tgNum <= 9) {
                    target = GameObject.Find("tg0" + tgNum).transform;

                }

                //----startTime = Time.timeSinceLevelLoad;
                startTime = Time.realtimeSinceStartup;

                startPosition = transform.position;
                endPosition = target.gameObject.transform.position;
                //diff=0にしないと大きな影響はないが瞬間移動が起こる
                diff = 0;
            }

            var rate = diff / time;
            //var pos = curve.Evaluate(rate);

            //等速直線運動
            transform.position = Vector3.Lerp(startPosition, endPosition, rate);
            //transform.position = Vector3.Lerp (startPosition, endPosition, pos);
        }

    }

}
