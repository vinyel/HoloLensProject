using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ExampleInputter_PlayStop : MonoBehaviour, IInputClickHandler {
    GameObject stick;
    private ChaseObject isPlay;
    public static bool isPause;
    // Use this for initialization
    void Start() {
        stick = GameObject.Find("Stick");
        isPlay = stick.GetComponent<ChaseObject>();
        isPause = false;
        
    }

    public void OnInputClicked(InputClickedEventData eventData) {
        //スクリプト自体を可、不可に
        isPlay.enabled = !isPlay.enabled;
        //停止すると、一時停止状態は保持されない
        isPause = false;
    }
}
