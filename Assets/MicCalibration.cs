using UnityEngine;
using UnityEngine.UI;

public class MicCalibration : MonoBehaviour
{
    public Slider sensitivitySlider;
    public Image volumeBarFill;
    public float currentLoudness;

    private AudioSource _audioSource;
    private string _micName;
    private int _sampleWindow = 128;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _micName = Microphone.devices[0];
        _audioSource.clip = Microphone.Start(_micName, true, 10, 44100);
        _audioSource.loop = true;
        while (!(Microphone.GetPosition(_micName) > 0)) { }
        _audioSource.Play();
    }

    void Update()
    {
        currentLoudness = GetLoudnessFromMicrophone();

        volumeBarFill.fillAmount = currentLoudness;

        if (currentLoudness >= sensitivitySlider.value)
        {
            volumeBarFill.color = Color.green;
        }
        else
        {
            volumeBarFill.color = Color.gray;
        }
    }

    float GetLoudnessFromMicrophone()
    {
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(_micName) - (_sampleWindow + 1);
        if (micPosition < 0) return 0;

        _audioSource.clip.GetData(waveData, micPosition);

        float totalSum = 0;
        for (int i = 0; i < _sampleWindow; i++)
        {
            totalSum += waveData[i] * waveData[i];
        }

        return Mathf.Sqrt(totalSum / _sampleWindow);
    }
}