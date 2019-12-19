using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordBlockAttacher : MonoBehaviour {
    private Rigidbody rigid;
    private BoxCollider bc;
    FixedJoint fj;
    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody>();
        bc = this.GetComponent<BoxCollider>();
        //this.transform.rotation = Quaternion.Euler(-90, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    private void OnTriggerEnter(Collider other) {
        if ( other.gameObject.name == "Concave"
            && HoloToolkit.Unity.InputModule.HandDraggable._isDragging == false
            && fj == null) {
            Debug.Log("Hit"); // ログを表示する
                              //rigid.velocity = Vector3.zero;
                              //rigid.angularVelocity = Vector3.zero;
            for (int i = 0; i < 1000; i++) {
                other.transform.root.transform.rotation = this.transform.rotation;
                other.transform.root.transform.position = this.transform.GetChild(0).position + new Vector3(0, 0.012f, 0);
            }
            
            gameObject.AddComponent<FixedJoint>();
            fj = gameObject.GetComponent<FixedJoint>();
            fj.breakForce = 3;
            fj.breakTorque = 3;
            fj.connectedBody = other.transform.root.GetComponent<Rigidbody>();
            fj.connectedMassScale = 1.5f;
            for (int i = 0; i < 1000; i++) {
                other.transform.root.transform.rotation = this.transform.rotation;
                other.transform.root.transform.position = this.transform.GetChild(0).position + new Vector3(0, 0.012f, 0);
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if (fj != null) {
            Destroy(this.GetComponent<FixedJoint>());
            Destroy(fj);
        }

    }
}
