using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class AudioScript : MonoBehaviour, IInputClickHandler {
    private AudioSource sound;
	// Use this for initialization
	void Start () {
        sound = this.GetComponent<AudioSource>();
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySound () {
        //Debug.Log("あいう");
        sound.PlayOneShot(sound.clip);
    }

    /*
    private void OnCollisionEnter(Collision collision) {
        //if ( collision.gameObject.name == "Cube" ) {
        PlaySound();
        Debug.Log(this.gameObject.name);
        // 3D同士が接触した瞬間の１回のみ呼び出される処理
    }
    */
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Stick") {
            PlaySound();
            //Debug.Log(this.gameObject.name);
        }
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        PlaySound();
    }
}
