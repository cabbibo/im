using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneInput : MonoBehaviour {

	// Use this for initialization
	void Start () {

    AudioSource audio  = GetComponent<AudioSource>();
    audio.clip = Microphone.Start("Built-in Microphone" , true, 10 , 44100 );
    while(!(Microphone.GetPosition(null)>0)){}
		audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
