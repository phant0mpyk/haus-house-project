using UnityEngine;
using System;

public class PitchTracker : MonoBehaviour
{
    private AudioSource audioSource;
    private float[] spectrum = new float[4096];
    private string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 0;
        audioSource.volume = 1.0f;
    }

    void Update()
    {
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        float maxVal = 0;
        int maxIndex = 0;

        for (int i = 5; i < 2000; i++)
        {
            if (spectrum[i] > maxVal)
            {
                maxVal = spectrum[i];
                maxIndex = i;
            }
        }

        if (maxVal > 0.001f)
        {
            float freq = (float)maxIndex * AudioSettings.outputSampleRate / spectrum.Length;
            Debug.Log($"<color=green>DETECTED:</color> {freq:F1}Hz | Note: {FrequencyToNote(freq)} | Amp: {maxVal:F4}");
        }
    }

    private string FrequencyToNote(float frequency)
    {
        float midiNoteFloat = 12f * (float)Math.Log(frequency / 440f, 2f) + 69f;
        int midiNote = Mathf.RoundToInt(midiNoteFloat);
        int noteIndex = (midiNote % 12 + 12) % 12;
        int octave = (midiNote / 12) - 1;
        return $"{noteNames[noteIndex]}{octave}";
    }
}