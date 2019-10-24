using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChordBlockManager : MonoBehaviour {
    private Rigidbody dr;
    List<Rigidbody> listAb;
    private Rigidbody rb;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.V) && HoloToolkit.Unity.InputModule.HandDraggable.draggingRigid != null) {
            listAb = new List<Rigidbody>();
            dr = HoloToolkit.Unity.InputModule.HandDraggable.draggingRigid;
            
            listAb.Add(dr);
            DetectAttachedBlocks();
            Spawn();
        }
	}
    void DetectAttachedBlocks() {
        if (dr.GetComponent<FixedJoint>() != null) {
            rb = dr.GetComponent<FixedJoint>().connectedBody;
            listAb.Add(rb);
            while (rb.gameObject.GetComponent<FixedJoint>() != null) {
                rb = rb.gameObject.GetComponent<FixedJoint>().connectedBody;
                listAb.Add(rb);
            }
        }

    }
    void Spawn () {
        int ct = 1;
        Rigidbody crb;
        //Debug.Log("aiu");
        listAb.Reverse();
        foreach(Rigidbody child in listAb) {
            crb = Instantiate(child, new Vector3(dr.transform.position.x - 0.2f, dr.transform.position.y + 0.02f * ct, dr.transform.position.z), dr.transform.rotation);
            Destroy(crb.GetComponent<FixedJoint>());
            crb.name = child.name + ct;
            crb.isKinematic = false;
            ct++;
        }
    }
}
