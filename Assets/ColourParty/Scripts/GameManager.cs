using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Tempo")]
    public float bpm = 120f;
    public float inputWindow = 0.15f; // seconds before/after beat allowed

    [Header("Beat Visuals")]
    public GameObject[] beatSquares; // size 4

    private double secondsPerBeat;
    private double nextBeatTime;
    private int beatIndex = 0;

    private bool canRegisterInput;
    public bool hitOnBeat;

    void Start()
    {
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
        if (canRegisterInput && Input.GetKeyDown(KeyCode.Space))
        {
            hitOnBeat = true;
            canRegisterInput = false;
            Debug.Log("HIT");
        }
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
