using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointBlock : MonoBehaviour {
    GameObject _parent;
    GameObject _otherParent;
    FixedJoint fj;

    private GameObject _concaveSideBlock;

    public GameObject ConcaveSideBlock => _concaveSideBlock;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if (fj != null &&_otherParent != null) {

        //}
	}
    
    /// <summary>
    /// 凹が近づいてきたら凸をくっつけに行く
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Convex" && fj == null) {
            _parent = transform.root.gameObject;
            _otherParent = other.transform.root.gameObject;

            _concaveSideBlock = _otherParent;
            
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
            fj.enablePreprocessing = true;
            //fj.connectedMassScale = 10;
            fj.massScale = 1.77f;
            //fj.autoConfigureConnectedAnchor = false;
            Debug.Log("aiu");
            for (int i = 0; i < 1000; i++) {
                _otherParent.transform.position = _parent.transform.position - new Vector3(0, 0.02f, 0);
                _otherParent.transform.rotation = _parent.transform.rotation;

            }
        }
        
    }
    
    private void OnTriggerExit(Collider other) {
        if ( fj != null ) {

            Destroy(this.GetComponent<FixedJoint>());
            Destroy(fj);
        }
        
    }
    
}
