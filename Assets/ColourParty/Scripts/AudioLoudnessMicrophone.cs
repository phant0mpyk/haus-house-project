using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AudioLoudnessMicrophone : MonoBehaviour
{
    //private float totalLoudness;
    public Vector2 minScale; //for visualizer
    public Vector2 maxScale;
    public float loudnessSensitivity = 100;
    public float treshold = 0.001f;
    public float loudness;
    public float peakLoudness; //debug to see peak value
    void Start()
    {
        MicrophoneToAudioClip();

        //foreach (var device in Microphone.devices)
        //{
        //    Debug.Log("Name: " + device);
        //}
    }

    public int sampleWindow = 64;
    private AudioClip microphoneClip;
    public void MicrophoneToAudioClip()
    {
        string microphoneName = Microphone.devices[0];  //gets name of first available microphone
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate); //start recording from that microphone
    }

    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip); 
    }


    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow; //calculate where to start reading samples from the clip

        if (startPosition < 0) //if not enough has been recorded return 0
            return 0;

        float[] waveData = new float[sampleWindow]; //create an array that holds audio samples
        clip.GetData(waveData, startPosition); //copy audio data from the clip into the array

        //compute loudness
        float totalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++) //loop through each audio sample
        {   
            totalLoudness += Mathf.Abs(waveData[i]); //converts negative pressure to positive pressure (frequency) 
        }

        return totalLoudness / sampleWindow; // return avarage loudness across the sample window
    }
    void Update()
    {
        loudness = GetLoudnessFromMicrophone() * loudnessSensitivity;        
        
        if (loudness >= peakLoudness)
        {
            peakLoudness = loudness;
        }
        
        if (loudness < treshold)
        {
            loudness = 0;
        }
        transform.localScale = Vector2.Lerp(minScale, maxScale, loudness);
    }
}


