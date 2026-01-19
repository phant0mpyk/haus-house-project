using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DSPBeatPlayer : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip hiHat1;
    public AudioClip hiHat2;
    public AudioClip clap;

    [Header("Beat Settings")]
    public float bpm = 60f; // Beats per minute
    public float hitWindow = 0.15f; // seconds +/-
    public float clapProbability = 0.5f;

    public GameObject[] redGlow;
    public GameObject[] greenGlow;
    public GameObject[] targetBeats;
    public AudioLoudnessMicrophone micScript;
    public PlayerScript playerScript;
    public float clapThreshold = 0.3f;

    private AudioSource audioSource;

    private double nextBeatTime; // Time for the next beat
    private double beatInterval; // Time between beats
    public int beatCount = 0; // counts 0-3
    private double[] hitTimeDSP = new double[4];
    private bool[] targetActive = new bool[4];
    public int beatCycle = 0;
    private int scoreCounter = 0;


    public void StartGame()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        // Convert BPM to seconds per beat
        beatInterval = 60.0f / bpm;

        // Schedule the first beat
        nextBeatTime = AudioSettings.dspTime + 1.0f; // start after 1 second
    }

    void Update()
    {
        double dspTime = AudioSettings.dspTime;

        if (micScript.loudness > clapThreshold)
        {
            CheckPlayerClap();
        }

        // Check if it's time for the next beat
        if (dspTime >= nextBeatTime)
        {

            PlayBeat(beatCount);

            if (beatCycle < 4)
            {
                SpawnTargets(beatCount);
            }
            
            if (scoreCounter == 3)
            {
                playerScript.MoveRight();
                scoreCounter = 0;
            }
            ;

            // Prepare the next beat
            nextBeatTime += beatInterval;
            beatCount = (beatCount + 1) % 4;  // 4/4 loop
            beatCycle = (beatCycle + 1) % 8;
        }
    }

    void PlayBeat(int beat)
    {
        switch (beat)
        {
            case 0:
                audioSource.PlayOneShot(hiHat2);
                redGlow[beat].SetActive(true);
                redGlow[beat + 3].SetActive(false);
                break;
            case 1:
                audioSource.PlayOneShot(hiHat1);
                redGlow[beat].SetActive(true);
                redGlow[beat - 1].SetActive(false);
                break;
            case 2:
                audioSource.PlayOneShot(hiHat1);
                redGlow[beat].SetActive(true);
                redGlow[beat - 1].SetActive(false);
                break;
            case 3:
                audioSource.PlayOneShot(hiHat1);
                redGlow[beat].SetActive(true);
                redGlow[beat - 1].SetActive(false);
                break;
            
        }

        if (beatCycle == 0)
        {
            
            for (int i = 0; i < 4; i++)
            {
                targetBeats[i].SetActive(false);
                targetActive[i] = false;
                greenGlow[i].SetActive(false);
                
            }
        }
    }

    void SpawnTargets(int beat)
    {
        bool shouldSpawn = Random.value > clapProbability;

        if (shouldSpawn) 
        {
            double spawnDPSTime = AudioSettings.dspTime;
            hitTimeDSP[beat] = spawnDPSTime + (4 * beatInterval);
            targetActive[beat] = true;
            targetBeats[beat].SetActive(true);
            audioSource.PlayOneShot(clap);
        }

    }
    void CheckPlayerClap()
    {
        double currentDSPTime = AudioSettings.dspTime;

        for (int i = 0; i < 4; i++)
        {
            if (!targetActive[i])
                continue;

            double timeDifference = Mathf.Abs((float)(currentDSPTime - hitTimeDSP[i]));

            if (timeDifference < +hitWindow)
            {
                targetBeats[i].SetActive(false);
                targetActive[i] = false;
                greenGlow[i].SetActive(true);
                scoreCounter++;
                return;

            }
        }
    }
    }

