using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

/// <summary>
/// 単音ブロックの振舞い
/// </summary>
public class ChordBlockManager : MonoBehaviour {
    private Rigidbody dr;
    List<Rigidbody> listAb;
    private Rigidbody rb;

    public TextMesh tm;
    
    // Use this for initialization
    void Start ()
    {
        // 音声認識「コピー」でブロックをコピーする
        var dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationResult += (text, config) =>
        {
            tm.text = text;
            // 認識結果
            if (HoloToolkit.Unity.InputModule.HandDraggable.draggingRigid.tag == "TargetChordBlock" && text == "コピー")
            {
                listAb = new List<Rigidbody>();
                dr = DetectTopBlock(HoloToolkit.Unity.InputModule.HandDraggable.draggingRigid.gameObject).GetComponent<Rigidbody>();
            
                listAb.Add(dr);
                DetectAllBlocks();
                Spawn();
            }
        };
        dictationRecognizer.DictationComplete += (config) => { dictationRecognizer.Start(); };
        dictationRecognizer.Start();
        
        this.UpdateAsObservable()
            .Select(_ => Input.inputString)
            .Where(xs => Input.GetKeyDown(KeyCode.N)
                         && HoloToolkit.Unity.InputModule.HandDraggable.draggingRigid != null)
            .Subscribe(_ => Debug.Log(DetectTopBlock(HoloToolkit.Unity.InputModule.HandDraggable.draggingRigid.gameObject)));
        this.UpdateAsObservable()
            .Select(_ => Input.inputString)
            .Where(xs => Input.GetKeyDown(KeyCode.V)
                         && HoloToolkit.Unity.InputModule.HandDraggable.draggingRigid != null)
            .Subscribe(_ =>
            {
                listAb = new List<Rigidbody>();
                dr = DetectTopBlock(HoloToolkit.Unity.InputModule.HandDraggable.draggingRigid.gameObject).GetComponent<Rigidbody>();
            
                listAb.Add(dr);
                DetectAllBlocks();
                Spawn();
            });
    }
	
    void DetectAllBlocks() {
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
