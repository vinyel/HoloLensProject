using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointBlock : MonoBehaviour {
    GameObject _parent;
    GameObject _otherParent;
    FixedJoint fj;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Convex") {
            _parent = transform.root.gameObject;
            _otherParent = other.transform.root.gameObject;
            _otherParent.transform.position = _parent.transform.position - new Vector3(0, 0.02f, 0);
            _otherParent.transform.rotation = _parent.transform.rotation;
            fj = _parent.AddComponent<FixedJoint>();
            fj.breakForce = 10;
            fj.breakTorque = 10;
            fj.connectedBody = _otherParent.GetComponent<Rigidbody>();
            Debug.Log("aiu");
        }
    }
    private void OnTriggerExit(Collider other) {
        //hj.connectedBody = null;
    }
}
