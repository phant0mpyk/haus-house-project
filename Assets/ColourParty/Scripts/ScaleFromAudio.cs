using JetBrains.Annotations;
using UnityEngine;

public class ScaleFromAudio : MonoBehaviour
{
    public Vector2 minScale;
    public Vector2 maxScale;
    public AudioLoudnessMicrophone detector;
   
    public float loudnessSensitivity = 100;
    public float treshold = 0.1f;
    public float loudness = 0;
    public float maxLoudness = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        loudness = detector.GetLoudnessFromMicrophone() * loudnessSensitivity;
        if (loudness >= maxLoudness)
        {
            maxLoudness = loudness;
        }
        // if (loudness > treshold)
        if (loudness < treshold)
        {

            //if (loudness >= maxLoudness)
            //{
            //    maxLoudness = loudness;
            //}
            loudness = 0;
        }
        transform.localScale = Vector2.Lerp(minScale, maxScale, loudness);  
    }
}
