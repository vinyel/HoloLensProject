using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordBlockModel : MonoBehaviour
{
	private GameObject _parent;

	public GameObject Parent
	{
		get { return _parent; }
		set { _parent = value; }
	}
	
	private GameObject _otherParent;

	public GameObject OtherParent
	{
		get { return _otherParent; }
		set { _otherParent = value; }
	}

	private FixedJoint _fj;

	public FixedJoint Fj
	{
		get { return _fj; }
		set { _fj = value; }
	}

	// 下側（凹）にくっついているブロック
	private GameObject _concaveSideBlock;

	public GameObject ConcaveSideBlock
	{
		get { return _concaveSideBlock; }
		set { _concaveSideBlock = value;  }
	}
	// 上側（凸）にくっついているブロック
	private GameObject _convexSideBlock;

	public GameObject ConvexSideBlock
	{
		get { return _convexSideBlock; }
		set { _convexSideBlock = value; }
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
