using UnityEngine;
using UnityEngine.UI;

public class GameManagerV2 : MonoBehaviour
{
    [Header("Beat Settings")]
    public float bpm = 120f; // beats per minute
    private double beatInterval; // seconds between beats
    public bool useSwing = false; // swing style toggle

    [Header("Timing Window")]
    public float earlyWindow = 0.2f; // seconds before beat that a clap counts
    public float lateWindow = 0.2f;  // seconds after beat that a clap counts

    [Header("UI Elements")]
    public SpriteRenderer[] beatSquares; // 4 squares that indicate the beat
    public Image[] targetIndicators; // 4 indicators showing where player should clap

    [Header("Gameplay")]
    public float clapThreshold = 0.5f; // minimum microphone loudness to count as clap
    private int[] targetBeats = new int[4]; // which squares the player should clap
    private bool[] hitBeats = new bool[4]; // tracks if each beat was hit

    [Header("References")]
    public PlayerScript playerScript; // player movement script
    public AudioLoudnessMicrophone micScript; // your microphone input script
    public AudioSource beatAudioSource; // audio source to play beat sounds
    public AudioClip beatClip; // sound to play on each beat


    private double nextBeatTime; // DSP time for next beat
    private int currentBeat = 0; // 0-7 for 8-beat swing

    void Start()
    {
        // Convert BPM to seconds per beat
        beatInterval = 60.0 / bpm;

        // Schedule first beat using DSP time
        nextBeatTime = AudioSettings.dspTime + beatInterval;

        // Generate initial random target squares
        GenerateRandomTargets();
    }

    void Update()
    {

        double dspTime = AudioSettings.dspTime;

        // Check if it's time for the next beat
        if (dspTime >= nextBeatTime)
        {
            HandleBeat(); // highlight square and play sound

            // Schedule next beat
            double interval = beatInterval;

            // Optional swing timing for off-beats
            if (useSwing && currentBeat % 2 == 1)
                interval *= 1.5;

            nextBeatTime += interval;
        }

        // Always check for player clap
        CheckPlayerClap(dspTime);
    }

    void HandleBeat()
    {
        // Highlight the square for this beat
        int squareIndex = currentBeat % 4;

        for (int i = 0; i < beatSquares.Length; i++)
        {
            if (i == squareIndex)
            {
                // Orange for active beat
                beatSquares[i].color = Color.Lerp(Color.white, Color.red, 0.5f);
            }
            else if (!hitBeats[i])
            {
                // Reset unhit squares
                beatSquares[i].color = Color.white;
            }
        }

        // Play beat sound
        if (beatAudioSource != null && beatClip != null)
        {
            beatAudioSource.PlayOneShot(beatClip);
        }

        // Advance beat
        currentBeat = (currentBeat + 1) % 8;

        // Reset targets after 8 beats
        if (currentBeat == 0)
        {
            GenerateRandomTargets();
        }
    }

    void GenerateRandomTargets()
    {
        // Random squares for beats 0-3
        for (int i = 0; i < 4; i++)
        {
            targetBeats[i] = Random.Range(0, 4);
            hitBeats[i] = false;
        }

        // Show indicators for target squares
        //for (int i = 0; i < targetIndicators.Length; i++)
        //    targetIndicators[i].enabled = false;

        //for (int i = 0; i < 4; i++)
        //    targetIndicators[targetBeats[i]].enabled = true;
    }

    void CheckPlayerClap(double dspTime)
    {
        float clapLoudness = micScript.loudness;

        if (clapLoudness > clapThreshold)
        {
            // Map beats 4-7 to 0-3 (player hit beats)
            int beatIndex = (currentBeat - 4 + 8) % 4;

            if (!hitBeats[beatIndex])
            {
                // Calculate the absolute DSP time of this beat
                double beatTime = nextBeatTime - beatInterval;

                // Check timing window
                if (dspTime >= beatTime - earlyWindow && dspTime <= beatTime + lateWindow)
                {
                    int targetSquare = targetBeats[beatIndex];

                    // Mark beat as hit
                    hitBeats[beatIndex] = true;

                    // Turn square green
                    beatSquares[targetSquare].color = Color.green;

                    // Check if all 4 beats were hit
                    bool allHit = true;
                    for (int i = 0; i < hitBeats.Length; i++)
                        if (!hitBeats[i]) allHit = false;
                    }
                }
            }
        }
    }
