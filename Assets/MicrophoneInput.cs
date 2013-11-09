using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource),typeof(CharacterController),typeof(OVRCameraController))]
public class MicrophoneInput : MonoBehaviour {
	
	private byte[][] map = new byte[][] {
		new byte[] {12, 14, 14, 6},
		new byte[] {13, 15, 15, 7},
		new byte[] {13, 15, 15, 7},
		new byte[] {9, 11, 11, 3}
	};
	
	private CharacterController controller;
	private GameObject cameraController;
	
	private const int sampleSize = 1024;
	private const float baseRMS = 0.1f;
	private const float amplitudeThreshold = 0.0f;
	private float RMSValue;
	private float dBValue;
	//private float pitch;
	
	private float[] samples;
	//private float[] spectrum;
	
	private bool isMoving = false;
	private float startTime;
	
	// Use this for initialization
	void Start () {
		controller = gameObject.GetComponent<CharacterController>();
		cameraController = GameObject.Find("CameraLeft");
		samples = new float[sampleSize];
		//spectrum = new float[sampleSize];
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
		RMSValue = Mathf.Sqrt(sum / sampleSize);
		dBValue = 20 * Mathf.Log10(RMSValue / baseRMS);
		if(dBValue < -160) dBValue = -160;
		
		/*audio.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
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
		pitch = frequency * 24000 / sampleSize;*/
	}
	
	private int[] GetGridPosition() {
		int[] pos = new int[2];
		float x = gameObject.transform.position.x;
		float z = gameObject.transform.position.z;
		pos[0] = Mathf.RoundToInt((x - 39f) / 3.8f);
		pos[1] = Mathf.RoundToInt((z - 103.5f) / (-3.9f));
		return pos;
	}
	
	private Vector3 GetDirection() {
		int[] gridPos = GetGridPosition();
		Debug.Log (gridPos[0] + ", " + gridPos[1]);
		byte b = map[gridPos[0]][gridPos[1]];
		float angle = cameraController.transform.eulerAngles.y;
		//Debug.Log (angle);
		if (angle >= 45 && angle < 135) {
			if ((b & 1) == 1) return new Vector3(10, 0, 0);
		} else if (angle >= 135 && angle < 225) {
			if ((b & 2) == 1) return new Vector3(0, 0, -10);
		} else if (angle >= 225 && angle < 315) {
			if ((b & 4) == 1) return new Vector3(-10, 0, 0);
		} else if (angle >= 315 || angle < 45) {
			if ((b & 8) == 1) return new Vector3(0, 0, 10);
		}
		return Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		Analyze();
		//Debug.Log("dB: " + dBValue);
		if ((RMSValue > baseRMS || Input.GetKeyDown(KeyCode.J)) && !isMoving) {
			if (!GetDirection().Equals(Vector3.zero)) {
				isMoving = true;
				startTime = Time.time;
				controller.SimpleMove(GetDirection());
			}
		}
		if(isMoving) {
			if (Time.time > startTime + 1) {
				isMoving = false;
				controller.SimpleMove(new Vector3(0, 0, 0));
			}
		}
	}
}