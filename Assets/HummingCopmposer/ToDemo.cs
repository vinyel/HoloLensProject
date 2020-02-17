using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ToDemo : MonoBehaviour, IInputClickHandler{

	public void OnInputClicked(InputClickedEventData eventData) {
		Destroy(this.gameObject);
	}
}
