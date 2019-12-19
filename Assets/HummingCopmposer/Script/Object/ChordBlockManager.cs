using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// 単音ブロックの振舞い
/// </summary>
public class ChordBlockManager : MonoBehaviour {
    private Rigidbody dr;
    List<Rigidbody> listAb;
    private Rigidbody rb;

    // Use this for initialization
    void Start ()
    {
        this.UpdateAsObservable()
            .Select(_ => Input.inputString)
            .Where(xs => Input.GetKeyDown(KeyCode.N)
                         && HoloToolkit.Unity.InputModule.HandDraggable.draggingRigid != null)
            .Subscribe(_ => Debug.Log(DetectTopBlock(HoloToolkit.Unity.InputModule.HandDraggable.draggingRigid.gameObject)));
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

    /// <summary>
    /// 一番上のブロックを調べる
    /// </summary>
    private GameObject DetectTopBlock(GameObject draggingObj)
    {
        var topBlock = draggingObj;
        GameObject tmp;
        while ((tmp = topBlock.GetComponent<ChordBlockPresenter>()._convexSideBlock) != null)
        {
            topBlock = tmp;
        }
        return topBlock;
    }
}
