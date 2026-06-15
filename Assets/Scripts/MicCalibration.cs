using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class MicSensitivityCalibrator : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The slider that sets the activation threshold.")]
    public Slider sensitivitySlider;

    [Tooltip("The image representing the volume level (must be Image Type: Filled).")]
    public Image volumeBarFill;

    [Header("Settings")]
    [Range(1f, 30f)]
    public float smoothSpeed = 15f; // Controls how 'jittery' or 'smooth' the bar is
    public int sampleWindow = 128;   // Amount of audio data to analyze at once

    private AudioSource _audioSource;
    private string _selectedMic;
    private float _visualLoudness;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        // Check if there are any microphones connected
        if (Microphone.devices.Length > 0)
        {
            _selectedMic = Microphone.devices[0];

            // Start recording and loop the clip
            _audioSource.clip = Microphone.Start(_selectedMic, true, 10, 44100);
            _audioSource.loop = true;

            // Wait until the microphone is actually recording before playing
            while (!(Microphone.GetPosition(_selectedMic) > 0)) { }
            _audioSource.Play();
        }
        else
        {
            Debug.LogError("No microphone detected!");
        }

        // Initialize slider values if not set
        if (sensitivitySlider != null)
        {
            sensitivitySlider.minValue = 0f;
            sensitivitySlider.maxValue = 0.5f; // RMS rarely hits 1.0; 0.5 is a loud peak
        }
    }

    void Update()
    {
        if (Microphone.IsRecording(_selectedMic))
        {
            float actualLoudness = GetLoudnessFromMicrophone();

            // Apply Linear Interpolation (Lerp) to smoothen the movement
            _visualLoudness = Mathf.Lerp(_visualLoudness, actualLoudness, Time.deltaTime * smoothSpeed);

            // Update the UI Fill Amount (0 to 1)
            if (volumeBarFill != null)
            {
                volumeBarFill.fillAmount = _visualLoudness;

                // Change color based on threshold (like Discord)
                if (sensitivitySlider != null && _visualLoudness >= sensitivitySlider.value)
                {
                    volumeBarFill.color = Color.green; // Active/Talking
                }
                else
                {
                    volumeBarFill.color = Color.gray;  // Below threshold/Silent
                }
            }
        }
    }

    float GetLoudnessFromMicrophone()
    {
        float[] waveData = new float[sampleWindow];

        // Get the current position in the microphone recording
        int micPosition = Microphone.GetPosition(_selectedMic) - (sampleWindow + 1);

        if (micPosition < 0) return 0;

        _audioSource.clip.GetData(waveData, micPosition);

        // Calculate RMS (Root Mean Square) for volume
        float totalSum = 0;
        for (int i = 0; i < sampleWindow; i++)
        {
            totalSum += waveData[i] * waveData[i];
        }

        return Mathf.Sqrt(totalSum / sampleWindow);
    }
}