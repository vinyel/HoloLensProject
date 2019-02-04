using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSystemController : MonoBehaviour {
    GameObject go;
	// Use this for initialization
	void Start () {
        go = GameObject.Find("ScriptObject");
        
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.C)) {
            go.GetComponent<DynamicChordBlock>().enabled = true;
        }
	}
}
