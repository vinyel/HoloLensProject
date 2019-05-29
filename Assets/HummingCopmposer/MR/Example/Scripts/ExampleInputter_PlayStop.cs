using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ExampleInputter_PlayStop : MonoBehaviour, IInputClickHandler {
    bool isRunning = false;
    float lastClick = 0f;
    float interval = 0.3f;

    int num = 0;
    int rnum = 0;
    
    public void OnInputClicked(InputClickedEventData eventData) {
        
        if ((lastClick + interval) > Time.time) {
            num = 2;
        }
        else {
            num = 1;
        }
        lastClick = Time.time;
    }
     
    GameObject stick;
    private ChaseObject isPlay;
    public  static bool isPause;
    // Use this for initialization
    void Start() {
        stick = GameObject.Find("Stick");
        isPlay = stick.GetComponent<ChaseObject>();
        isPause = false;
        
    }

    void Update() {
        if ( num != 0 ) {
            StartCoroutine(HoldSphere());
            //Debug.Log(rnum);
        }
    }

    IEnumerator HoldSphere() {
        if (isRunning)
            yield break;
        isRunning = true;
        yield return new WaitForSeconds(0.3f);
        Debug.Log(num);
        rnum = num;
        num = 0;
        if (rnum == 2) {
            //スクリプト自体を可、不可に
            isPlay.enabled = !isPlay.enabled;
            //停止すると、一時停止状態は保持されない
            isPause = false;
        }
        //スペースで一時停止
        if (rnum == 1) {
            isPause = !isPause;
        }
        isRunning = false;
    }
}
