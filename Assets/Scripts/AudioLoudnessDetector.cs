using Unity.VisualScripting;
using UnityEngine;

public class AudioLoudnessDetector : MonoBehaviour
{
    public int sampleWindow = 64;

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;

        if (startPosition < 0)
            return 0;

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        //compute loudness
        float totalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);

        }

        return totalLoudness / sampleWindow;

    }

}
