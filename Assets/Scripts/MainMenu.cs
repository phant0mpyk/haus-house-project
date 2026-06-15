using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   //Switching to the scenes of the different minigames//
    public void PlayGame1()
    {
        SceneManager.LoadSceneAsync("Tutorial");
    }

    public void PlayGame2()
    {
        SceneManager.LoadSceneAsync("SleepyScreen");
    }

    public void PlayGame3()
    {
        SceneManager.LoadSceneAsync("ColourParty");
    }

    //Switching to settings scene//
    public void Settings()
    {
        SceneManager.LoadSceneAsync("Settings");
    }

    //Returning back to the main menu scene//
    public void ExitGame()
    {
        SceneManager.LoadSceneAsync(0);
    }

    //Closing the game//
    public void QuitGame()
    {
        Application.Quit();
    }
}
