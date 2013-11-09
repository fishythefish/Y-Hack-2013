#pragma strict

var sampleSize: int = 1024;
var baseRMS: float = 0.1;
var amplitudeThreshold = 0.02;
var RMS: float;
var dB: float;
var pitch: float;

private var samples: float[];
private var spectrum: float[];

var display: GUIText;

function Start () {
	samples = new float[sampleSize];
	spectrum = new float[sampleSize];
}

function Update () {
	Analyze();
	if (display) {
		display.text = "RMS: " + RMS.ToString("F2") +
			" (" + dB.ToString("F1") + " dB)\n" +
			"Pitch: " + pitch.ToString("F0") + " Hz";
	}
}

function Analyze () {
	audio.GetOutputData(samples, 0);
	var i: int;
	var sum: float = 0;
	for (i = 0; i < sampleSize; i++) {
		sum += samples[i] * samples[i];
	}
	RMS = Mathf.Sqrt(sum / sampleSize);
	dB = 20 * Mathf.Log10(RMS / baseRMS);
	if (dB < -160) dB = -160;
	
	audio.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
	var maxValue: float = 0;
	var maxIndex: int = 0;
	var spectrumValue: float = 0;
	for (i = 0; i < sampleSize; i++) {
		spectrumValue = spectrum[i];
		if (spectrumValue > maxValue && spectrumValue > amplitudeThreshold) {
			maxValue = spectrum[i];
			maxIndex = i;
		}
	}
	var frequency: float = maxIndex;
	if (maxIndex > 0 && maxIndex < sampleSize - 1) {
		var dL = spectrum[maxIndex - 1] / spectrum[maxIndex];
		var dR = spectrum[maxIndex + 1] / spectrum[maxIndex];
		frequency += 0.5 * (dL * dL + dR * dR);
	}
	pitch = frequency * 24000 / sampleSize;
}