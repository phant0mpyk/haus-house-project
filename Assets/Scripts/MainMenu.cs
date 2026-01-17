using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   //Switching to the scenes of the different minigames//
    public void PlayGame1()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void PlayGame2()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void PlayGame3()
    {
        SceneManager.LoadSceneAsync(3);
    }

    //Switching to settings scene//
    public void Settings()
    {
        SceneManager.LoadSceneAsync(4);
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
