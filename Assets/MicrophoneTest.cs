using UnityEngine;
using System.Collections;

public class MicrophoneTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		audio.clip = Microphone.Start(Microphone.devices[0], true, 1, 44100);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
