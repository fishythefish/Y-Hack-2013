using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneTest : MonoBehaviour {
	
	float[] samples = new float[1024];
	float[] spectrum = new float[1024];
	
	// Use this for initialization
	void Start () {
		audio.clip = Microphone.Start(null, true, 10, 44100);
		audio.loop = true;
		audio.mute = true;
		audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
		audio.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
		for (int i = 0; i < 1023; i++) {
			Debug.DrawLine(new Vector3(i, Mathf.Log10(spectrum[i]) + 10, 0), new Vector3(i + 1, Mathf.Log10(spectrum[i + 1]) + 10, 0), Color.red);
		}
		
		audio.GetOutputData(samples, 0);
		Debug.Log (samples[0]);
	}
}