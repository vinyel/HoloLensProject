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
			.Where(_ => HoloToolkit.Unity.InputModule.HandDraggable._isDragging == true
			            && HoloToolkit.Unity.InputModule.HandDraggable.draggingRigid.gameObject.name == this.name)
			.Subscribe(_ =>
			{
				AngleSubscriber();
			});
		this.UpdateAsObservable()
			.Where(_ => HoloToolkit.Unity.InputModule.HandDraggable._isDragging == false)
			.Subscribe(_ =>
			{
				view.Initialize();
			});
		model.Angle.Subscribe(value => { view.RotateBox(model.AngleMax, value); });
	}

	private void AngleSubscriber()
	{
		model.Angle.Value = 5f * Time.deltaTime;
	}

}
