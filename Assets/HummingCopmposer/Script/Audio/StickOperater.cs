using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://unity3d.com/jp/learn/tutorials/topics/scripting/enabling-and-disabling-components

public class StickOperater: MonoBehaviour {

    GameObject stick;
    private ChaseObject isPlay;
    public bool isPause;
	// Use this for initialization
	void Start () {
        stick = GameObject.Find("Stick");
        isPlay = stick.GetComponent<ChaseObject>();
        isPause = false;
	}

    // Update is called once per frame
    void Update() {
        //左シフトで停止
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            //スクリプト自体を可、不可に
            isPlay.enabled = !isPlay.enabled;
            //停止すると、一時停止状態は保持されない
            isPause = false;
        }
        //スペースで一時停止
        if (Input.GetKeyUp(KeyCode.Space) && isPlay) {
            isPause = !isPause;
        }
    }

}
