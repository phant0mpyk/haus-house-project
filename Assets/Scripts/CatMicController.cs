using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

[RequireComponent(typeof(AudioSource))]
public class CatMicController : MonoBehaviour
{
    //Cat Sprites
    public Image catImage;
    public Sprite catSleeping;
    public Sprite catAwake;
    public Image fishPoint;

    //Loudness Settings
    public AudioLoudnessDetector loudnessDetector;
    public float minGreen = 0.001f;   // quiet threshold
    public float maxGreen = 20f;   // loud threshold


    private AudioSource micSource;
    private string micName;

    //point counter
    public float catPoints = 0f; //the points gained after keeping the cat asleep/relaxed for 3 seconds
    public float winPoints = 5f; //the points needed to win 

    //Win Settings
    public float winTime = 3f; // the time the player needs to keep the cat asleep for for gsining a catPoint
    private float currentGreenTime = 0f;
    public bool gameWon = false; //the game is not won from the start

    //public float endTime = 15f; //the time that needs to be reached to actually win (it's shorter for now since it's the prototype), dont need this because we already have the winPoints




    void Start()
    {
        micSource = GetComponent<AudioSource>();

        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone found!");
            return;
        }
        //GainPoint();
        micName = Microphone.devices[0];

        // Start mic
        micSource.clip = Microphone.Start(micName, true, 1, 48000); //48000=the frequency

        while (Microphone.GetPosition(micName) <= 0) { }

        micSource.loop = true;
        micSource.Play(); // this plays the voice input that was given; we don't want this but when it's not there the whole thing is not working; I just put the output to a mixergroup with 0% volume, so now we wont hear the echo anymore

    }

    void Update()
    {


        //if (gameWon) return; this stops the Update function from running after game won is true once in GainPoint()

        float loudness = loudnessDetector.GetLoudnessFromAudioClip(
            micSource.timeSamples,
            micSource.clip
        );

        bool inGreen = (loudness >= minGreen && loudness <= maxGreen);

        // Change cat sprite
        if (inGreen)
        {
            catImage.sprite = catSleeping; //switching the cat picture to the sleeping cat
            currentGreenTime += Time.deltaTime;

            if (currentGreenTime >= 3) //when the player manages to keep the volume in the green range for three seconds he gains a catPoint
            {
                GainPoint();
            }
        }
        else
        {
            catImage.sprite = catAwake; //switching the cat picture to the awake cat if the volume is in the red
            currentGreenTime = 0f;
        }



    }

    void GainPoint()
    {
        //gameWon = true;
        Debug.Log("Cat asleep for 3 seconds, you gained +1 point"); //if the game is won it shows a debug message
        catPoints += 1f;
        Debug.Log(catPoints);
        StartCoroutine(WaitAndPrint(1));





        currentGreenTime = 0; //resets the greentime to 0 again so the function doesn't give out more points

        if (catPoints >= winPoints) // this is the statement for winning the game. If the player hasd gained 5 or more catPoints the win statement is played
        {
            Debug.Log("The kitty fellk asleep; congrats <3");
            SceneManager.LoadScene("SleepyScreen"); //changes the scene to the winning scene
        }


    }

    private IEnumerator WaitAndPrint(float waitTime) //an extra is running without stopping the main fuction from running
    {
        fishPoint.enabled = true;
        Debug.Log("Starting");
        yield return new WaitForSeconds(waitTime);
        print("Coroutine ended: " + Time.time + " seconds");
        fishPoint.enabled = false;
    }


}
