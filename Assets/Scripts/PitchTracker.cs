using UnityEngine;
using System;

public class PitchTracker : MonoBehaviour
{
    private const int QSamples = 1024;
    private const float ReferenceA4 = 440f;
    private const int MIDINoteA4 = 69;

    private AudioSource audioSource;
    private float[] spectrum;
    private float[] samples;
    private float fundamentalFrequency = 0f;

    public enum PitchAlgorithm { HPS, Autocorrelation }
    public PitchAlgorithm currentAlgorithm = PitchAlgorithm.HPS;

    private readonly string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spectrum = new float[QSamples];
        samples = new float[QSamples];

        if (Microphone.devices.Length > 0)
        {
            audioSource.clip = Microphone.Start(null, true, 10, AudioSettings.outputSampleRate);
            audioSource.loop = true;

            while (!(Microphone.GetPosition(null) > 0)) { }
            audioSource.Play();
            Debug.Log("Microphone started. Pitch detection active.");
        }
        else
        {
            Debug.LogError("No microphone device detected!");
            enabled = false;
        }
    }

    void Update()
    {
        if (!audioSource.isPlaying) return;

        if (currentAlgorithm == PitchAlgorithm.HPS)
        {
            audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
            fundamentalFrequency = FindPitchUsingHPS(spectrum, QSamples, 4);
        }
        else
        {
            audioSource.GetOutputData(samples, 0);
            fundamentalFrequency = FindPitchUsingAutocorrelation(samples, QSamples);
        }

        string note = FrequencyToNote(fundamentalFrequency);
        Debug.Log($"Algorithm: {currentAlgorithm} | Freq: {fundamentalFrequency:F2} Hz | Note: {note}");
    }

    private float FindPitchUsingHPS(float[] currentSpectrum, int size, int downsampleLimit)
    {
        float[] hps = new float[size];
        System.Array.Copy(currentSpectrum, hps, size);

        for (int h = 2; h <= downsampleLimit; h++)
        {
            for (int i = 0; i < size / h; i++)
            {
                hps[i] *= currentSpectrum[i * h];
            }
        }

        int maxIndex = 0;
        float maxVal = 0f;

        int minSearchIndex = Mathf.FloorToInt(70f * size / AudioSettings.outputSampleRate);
        int maxSearchIndex = Mathf.FloorToInt(1000f * size / AudioSettings.outputSampleRate);

        if (maxSearchIndex > size / downsampleLimit) maxSearchIndex = size / downsampleLimit;

        for (int i = minSearchIndex; i < maxSearchIndex; i++)
        {
            if (hps[i] > maxVal && hps[i] > 0.001f)
            {
                maxVal = hps[i];
                maxIndex = i;
            }
        }

        if (maxIndex == 0) return 0f;

        return maxIndex * (AudioSettings.outputSampleRate / (float)size);
    }

    private float FindPitchUsingAutocorrelation(float[] currentSamples, int size)
    {
        float[] R = new float[size];

        for (int tau = 0; tau < size; tau++)
        {
            float sum = 0;
            for (int i = 0; i < size - tau; i++)
            {
                sum += currentSamples[i] * currentSamples[i + tau];
            }
            R[tau] = sum;
        }

        int maxIndex = 0;
        float maxVal = 0f;

        int minLag = Mathf.FloorToInt(AudioSettings.outputSampleRate / 500f);
        int maxLag = Mathf.FloorToInt(AudioSettings.outputSampleRate / 70f);

        for (int i = minLag; i < maxLag; i++)
        {
            if (R[i] > maxVal)
            {
                maxVal = R[i];
                maxIndex = i;
            }
        }

        if (maxIndex == 0) return 0f;

        return AudioSettings.outputSampleRate / (float)maxIndex;
    }

    private string FrequencyToNote(float frequency)
    {
        if (frequency < 20f || frequency > 3000f)
        {
            return "---";
        }

        float midiNoteFloat = 12f * (float)Math.Log(frequency / ReferenceA4, 2f) + MIDINoteA4;
        int midiNote = Mathf.RoundToInt(midiNoteFloat);

        midiNote = Mathf.Clamp(midiNote, 0, 127);

        int noteIndex = midiNote % 12;
        int octave = (midiNote / 12) - 1;

        return $"{noteNames[noteIndex]}{octave}";
    }
}