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

	[SerializeField] private GameObject _droppedBlock;

	void Awake()
	{
		this.UpdateAsObservable()
			.Where(_ => HoloToolkit.Unity.InputModule.HandDraggable._isDragging == true
			            && HoloToolkit.Unity.InputModule.HandDraggable.draggingRigid.tag == "BlockBox")
			.Subscribe(_ =>
			{
				ShakeCheck();
				AngleSubscriber();
			});
		this.UpdateAsObservable()
			.Where(_ => HoloToolkit.Unity.InputModule.HandDraggable._isDragging == false)
			.Subscribe(_ =>
			{
				view.Initialize();
			});
		model.Angle.Subscribe(value => { view.RotateBox(model.AngleMax, value); });
		model.ShakeCount
			.Where(value => value % 3 == 0 && value != 0)
			.Subscribe(_ => 
				Instantiate(_droppedBlock, 
			new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation)
				);
	}

	private void AngleSubscriber()
	{
		model.Angle.Value = 5f * Time.deltaTime;
	}

	private void ShakeCheck()
	{
		model.PreAcceleration = model.Acceleration;
		model.Acceleration = GetComponent<Rigidbody>().velocity;
		model.DotProduct = Vector3.Dot(model.Acceleration, model.PreAcceleration);
		if (model.DotProduct < 0)
		{
			model.ShakeCount.Value++;
		}
	}
}
