using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//参考
//https://www.sawalemontea.com/entry/2017/07/26/164508
//http://neareal.com/1230/

public class DragObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    /*
    public void OnDrag() {
        Vector3 TapPos = Input.mousePosition;
        TapPos.z = 10f;
        transform.position = Camera.main.ScreenToViewportPoint(TapPos);
    }
    */
    private void OnMouseDrag() {
        Vector3 objectPointInScreen
    = Camera.main.WorldToScreenPoint(this.transform.position);

        Vector3 mousePointInScreen
            = new Vector3(Input.mousePosition.x,
                          Input.mousePosition.y,
                          objectPointInScreen.z);

        Vector3 mousePointInWorld = Camera.main.ScreenToWorldPoint(mousePointInScreen);
        mousePointInWorld.z = this.transform.position.z;
        this.transform.position = mousePointInWorld;
    }
}
