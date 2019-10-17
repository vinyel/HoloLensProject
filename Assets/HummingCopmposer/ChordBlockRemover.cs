using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordBlockRemover : MonoBehaviour {
    private Rigidbody rigid;
    private BoxCollider bc;
    FixedJoint fj;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Concave"
            && HoloToolkit.Unity.InputModule.HandDraggable._isDragging == false
            && fj == null) {
            Debug.Log("Hit"); // ログを表示する
                              //rigid.velocity = Vector3.zero;
                              //rigid.angularVelocity = Vector3.zero;
            other.transform.root.transform.rotation = this.transform.rotation;
            other.transform.root.transform.position = this.transform.GetChild(0).position + new Vector3(0, 0.012f, 0);
            gameObject.AddComponent<FixedJoint>();
            fj = gameObject.GetComponent<FixedJoint>();
            fj.breakForce = 100;
            fj.breakTorque = 100;
            fj.connectedBody = other.transform.root.GetComponent<Rigidbody>();

        }

    }
    
    private void OnTriggerExit(Collider other) {
        Destroy(this.GetComponent<FixedJoint>());
    }
    
}
