using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using UniRx.Triggers;

public class ChordBlockBoxPresenter : MonoBehaviour
{

	public ChordBlockBoxModel model;

	public ChordBlockBoxView view;

	void Awake()
	{
		this.UpdateAsObservable()
			.Select(_ => Input.inputString)
			.Where(xs => Input.GetKey(KeyCode.K))
			.Subscribe(__ => AngleSubscriber());
		model.Angle.Subscribe(value => { view.RotateBox(model.AngleMax, value); });
	}

	private void AngleSubscriber()
	{
		model.Angle.Value = 10f * Time.deltaTime;
	}

}
