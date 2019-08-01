using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttacherBlock : MonoBehaviour {
    GameObject _parent;
    GameObject _otherParent;
    bool isAttach = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    /*
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Convex") {
            _parent = transform.root.gameObject;
            _otherParent = other.transform.root.gameObject;

            _otherParent.transform.parent = _parent.transform;
            _otherParent.transform.position = _parent.transform.localPosition - new Vector3(0, 0.02f, 0);
            _otherParent.transform.rotation = _parent.transform.localRotation;
            
            //Debug.Log(this.gameObject.name);
        }
    }
    */
    private void OnTriggerStay(Collider other) {

        if (other.gameObject.name == "Convex") {
            _parent = transform.root.gameObject;
            _otherParent = other.transform.root.gameObject;
            //if (!isAttach) {
            //    DoubleSizeCollision(_parent);
            //}
            //isAttach = true;
            //_otherParent.transform.parent = _parent.transform;
            _otherParent.transform.position = _parent.transform.position - new Vector3(0, 0.02f, 0);
            _otherParent.transform.rotation = _parent.transform.rotation;

            //Debug.Log(this.gameObject.name);
        }
    }
    
    private void DoubleSizeCollision(GameObject aboveBlock) {
        BoxCollider bc;
        bc = aboveBlock.GetComponent<BoxCollider>();
        bc.size = new Vector3(bc.size.x * 1.2f, bc.size.y * 1.2f, bc.size.z * 2);
        bc.center = new Vector3(bc.center.x, bc.center.y, bc.center.z - bc.size.z / 2);
    }
    private void HalfSizeCollision(GameObject aboveBlock) {
        BoxCollider bc;
        bc = aboveBlock.GetComponent<BoxCollider>();
        bc.size = new Vector3(bc.size.x, bc.size.y, bc.size.z * 2);
    }
}
