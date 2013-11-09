using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneInput : MonoBehaviour {

	private const int sampleSize = 1024;
	private const float baseRMS = 0.1f;
	private const float amplitudeThreshold = 0.0f;
	private float RMSvalue;
	private float dBValue;
	private float pitch;
	
	private float[] samples;
	private float[] spectrum;
	
	// Use this for initialization
	void Start () {
		samples = new float[sampleSize];
		spectrum = new float[sampleSize];
		audio.clip = Microphone.Start(null, true, 10, 44100);
		audio.loop = true;
		audio.mute = true;
		audio.Play();
	}
	
	private void Analyze() {
		audio.GetOutputData(samples, 0);
		float sum = 0;
		for (int i = 0; i < sampleSize; i++) {
			sum += samples[i] * samples[i];
		}
		RMSvalue = Mathf.Sqrt(sum / sampleSize);
		dBValue = 20 * Mathf.Log10(RMSvalue / baseRMS);
		if(dBValue < -160) dBValue = -160;
		
		audio.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
		float maxValue = 0;
		int maxIndex = 0;
		for (int i = 0; i < sampleSize; i++) {
			if (spectrum[i] > maxValue && spectrum[i] > amplitudeThreshold) {
				maxValue = spectrum[i];
				maxIndex = i;
			}
		}
		float frequency = maxIndex;
		if (maxIndex > 0 && maxIndex < sampleSize - 1) {
			float dL = spectrum[maxIndex - 1] / spectrum[maxIndex];
			float dR = spectrum[maxIndex + 1] / spectrum[maxIndex];
			frequency += 0.5f * (dR * dR - dL * dL);
		}
		pitch = frequency * 24000 / sampleSize;
	}
	
	// Update is called once per frame
	void Update () {
		Analyze();
		Debug.Log(RMSvalue + ", " + dBValue + ", " + pitch);
	}
}
