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
        //if (fj != null &&_otherParent != null) {

        //}
	}
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Convex" && fj == null) {
            _parent = transform.root.gameObject;
            _otherParent = other.transform.root.gameObject;
            //_parent.transform.rotation = Quaternion.Euler(0, 0, 0);
            for (int i = 0; i < 1000; i++) {
                _otherParent.transform.position = _parent.transform.position - new Vector3(0, 0.02f, 0);
                _otherParent.transform.rotation = _parent.transform.rotation;
                
            }

            _parent.AddComponent<FixedJoint>();
            fj = _parent.GetComponent<FixedJoint>();
            fj.breakForce = 300;
            fj.breakTorque = 300;
            fj.connectedBody = _otherParent.GetComponent<Rigidbody>();
            //fj.enableCollision = true;
            //fj.enablePreprocessing = false;
            //fj.connectedMassScale = 10;
            fj.massScale = 1.5f;
            //fj.autoConfigureConnectedAnchor = false;
            
            Debug.Log("aiu");
        }
        
    }
    
    private void OnTriggerExit(Collider other) {
        if ( fj != null ) {
            Destroy(this.GetComponent<FixedJoint>());
            Destroy(fj);
        }
        
    }
    
}
