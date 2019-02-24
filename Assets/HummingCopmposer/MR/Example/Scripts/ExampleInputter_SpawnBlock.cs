using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ExampleInputter_SpawnBlock : MonoBehaviour, IInputClickHandler {
    GameObject go;
    bool isSpawn = false;
    void Start() {
        go = GameObject.Find("ScriptObject");

    }

    public void OnInputClicked(InputClickedEventData eventData) {
        if ( !isSpawn ) {
            go.GetComponent<DynamicChordBlock>().enabled = true;
        }
        isSpawn = true;
    }
}
