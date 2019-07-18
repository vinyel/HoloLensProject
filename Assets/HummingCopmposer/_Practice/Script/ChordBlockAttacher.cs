﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordBlockAttacher : MonoBehaviour {
    private Rigidbody rigid;
    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody>();
        this.transform.rotation = Quaternion.Euler(-90, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    private void OnTriggerStay(Collider other) {
        if ( other.tag == "TargetChordBlock" ) {
            Debug.Log("Hit"); // ログを表示する
                              //rigid.velocity = Vector3.zero;
                              //rigid.angularVelocity = Vector3.zero;
            other.transform.rotation = this.transform.rotation;
            other.transform.position = this.transform.position;
        }
    }

}