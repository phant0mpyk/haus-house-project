using UnityEngine;
using System;

public class PitchTracker : MonoBehaviour
{
    public AudioSource micSource; // The source listening to the player
    public AudioSource synthSource; // The source playing the "lower octave"

    private float[] spectrum = new float[4096];
    private string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

    // Synth variables
    private float currentFreq;
    private float targetFreq;
    private float phase;
    private float sampling_rate;
    private float volume = 0f;

    void Start()
    {
        sampling_rate = AudioSettings.outputSampleRate;

        // Setup micSource if not assigned
        if (micSource == null) micSource = GetComponent<AudioSource>();

        // Setup synthSource
        if (synthSource == null) synthSource = gameObject.AddComponent<AudioSource>();
        synthSource.playOnAwake = false;
        synthSource.Stop();
    }

    void Update()
    {
        micSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

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

        if (maxVal > 0.01f) // Threshold to prevent background noise singing
        {
            float detectedFreq = (float)maxIndex * sampling_rate / spectrum.Length;

            // Set target to half frequency (One octave lower)
            targetFreq = detectedFreq / 2.0f;
            volume = 0.2f; // Adjust volume of the synth

            Debug.Log($"<color=green>SINGING:</color> {targetFreq:F1}Hz (Octave Down)");
        }
        else
        {
            volume = 0f; // Silence if player isn't singing
        }

        // Smoothly interpolate the pitch to avoid "chirping" sounds
        currentFreq = Mathf.Lerp(currentFreq, targetFreq, Time.deltaTime * 10f);
    }

    // This method generates the actual audio data buffers
    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            phase += 2 * Mathf.PI * currentFreq / sampling_rate;
            float wave = Mathf.Sin(phase) * volume;

            for (int d = 0; d < channels; d++)
            {
                data[i + d] = wave;
            }

            if (phase > 2 * Mathf.PI) phase -= 2 * Mathf.PI;
        }
    }
}