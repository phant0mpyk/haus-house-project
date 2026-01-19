using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerScript playerScript;
    public AudioLoudnessMicrophone detector;
    public float microphoneSensitivity = 100;
    public float treshold = 0.1f;

    [Header("Tempo")]
    public float bpm = 120f;
    public float inputWindow = 0.15f; // seconds before/after beat allowed

    [Header("Beat Visuals")]
    public GameObject[] beatSquares; // size 4
    public SpriteRenderer[] beatSquaresRenderer;

    private double secondsPerBeat;
    private double nextBeatTime;
    private int beatIndex = 0;

    private bool canRegisterInput;
    public bool hitOnBeat;
    public int hitCounter = 0;


    void Start()
    {
        beatSquaresRenderer = new SpriteRenderer[beatSquares.Length];
        for (int i = 0; i < beatSquares.Length; i++)
        {
            beatSquaresRenderer[i] = beatSquares[i].GetComponent<SpriteRenderer>();
        }

        secondsPerBeat = 60f / bpm;
        nextBeatTime = AudioSettings.dspTime + 1.0f; // start after 1 sec
    }

    void Update()
    {
        double dspTime = AudioSettings.dspTime;

        if (dspTime >= nextBeatTime)
        {
            TriggerBeat();
            nextBeatTime += secondsPerBeat;
        }

        // Input detection
        float loudness = detector.GetLoudnessFromMicrophone() * microphoneSensitivity;

        //if (canRegisterInput && Input.GetKeyDown(KeyCode.Space))
        if (canRegisterInput && loudness > treshold)
        {
            hitOnBeat = true;
            canRegisterInput = false;
            Debug.Log("HIT");
            hitCounter++;
        }

        foreach (SpriteRenderer sr in beatSquaresRenderer)
            sr.color = hitOnBeat ? Color.green : Color.red;

        //move player
        if (hitCounter == 8)
        {
            playerScript.MoveRight();
            hitCounter = 0;
        }

        void TriggerBeat()
        {
            // Reset visuals
            foreach (var square in beatSquares)
                square.SetActive(false);

            beatSquares[beatIndex].SetActive(true);

            hitOnBeat = false;
            canRegisterInput = true;

            // Close input window after tolerance
            Invoke(nameof(CloseInputWindow), inputWindow);

            beatIndex = (beatIndex + 1) % beatSquares.Length;
        }

        void CloseInputWindow()
        {
            canRegisterInput = false;
        }
    }
}
