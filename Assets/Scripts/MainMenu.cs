using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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

    public void ExitGame()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
