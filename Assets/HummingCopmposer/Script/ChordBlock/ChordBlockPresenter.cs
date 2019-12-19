using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ChordBlockPresenter : MonoBehaviour
{
    public ChordBlockModel _chordBlockModel;
    public ChordBlockView _chordBlockView;

    public GameObject _convex;
    public GameObject _concave;

    public GameObject _convexSideBlock;
    
	// Use this for initialization
	void Start ()
    {
        // 持ってるブロックの凹が他のブロックの凸に近づいたときの処理
        _concave.OnTriggerEnterAsObservable()
            .Where(col => col.name == "Convex" && _chordBlockModel.Fj == null)
            .Subscribe(col => JointBlock(col));
        _concave.OnTriggerExitAsObservable()
            .Where(_ => _chordBlockModel.Fj != null)
            .Subscribe(_ =>
            {
                Destroy(this.GetComponent<FixedJoint>());
                Destroy(_chordBlockModel.Fj);
            });
        // 凸側にくっついた他のブロックを検知して代入
        _convex.OnTriggerEnterAsObservable()
            .Where(col => col.name == "Concave")
            .Subscribe(col =>
            {
                _convexSideBlock = col.transform.root.gameObject;
            });
        _convex.OnTriggerExitAsObservable()
            .Subscribe(_ =>
            {
                _convexSideBlock = null;
            });
    }

    /// <summary>
    ///　凹を凸に近づけると凸がくっついてくる
    /// </summary>
    /// <param name="other"></param>
    private void JointBlock(Collider other)
    {
        _chordBlockModel.Parent = this.gameObject;
        _chordBlockModel.OtherParent = other.transform.root.gameObject;
        _chordBlockModel.ConcaveSideBlock = _chordBlockModel.OtherParent;
        
        for (int i = 0; i < 1000; i++) {
            _chordBlockModel.OtherParent.transform.position = _chordBlockModel.Parent.transform.position - new Vector3(0, 0.02f, 0);
            _chordBlockModel.OtherParent.transform.rotation = _chordBlockModel.Parent.transform.rotation;
        }
        _chordBlockModel.Parent.AddComponent<FixedJoint>();
        _chordBlockModel.Fj = _chordBlockModel.Parent.GetComponent<FixedJoint>();
        _chordBlockModel.Fj.breakForce = 300;
        _chordBlockModel.Fj.breakTorque = 300;
        _chordBlockModel.Fj.connectedBody = _chordBlockModel.OtherParent.GetComponent<Rigidbody>();
        _chordBlockModel.Fj.enablePreprocessing = true;
        
        _chordBlockModel.Fj.massScale = 1.77f;
        Debug.Log("aiu");
        
        for (int i = 0; i < 1000; i++) {
            _chordBlockModel.OtherParent.transform.position = _chordBlockModel.Parent.transform.position - new Vector3(0, 0.02f, 0);
            _chordBlockModel.OtherParent.transform.rotation = _chordBlockModel.Parent.transform.rotation;
        }        
    }
    
}
