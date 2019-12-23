using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordBlockBoxView : MonoBehaviour
{

	public GameObject _chordBlockBox;

	public void Initialize()
	{
		_chordBlockBox.transform.rotation = Quaternion.Euler(-90, 0, 0);
	}
	
	public void RotateBox(float max, float speed)
	{
		_chordBlockBox.transform.rotation = Quaternion.Slerp(_chordBlockBox.transform.rotation, Quaternion.Euler(max, 0, 0), speed);
	}
}
