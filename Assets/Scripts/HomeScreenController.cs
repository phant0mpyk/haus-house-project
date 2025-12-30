using UnityEngine;
using UnityEngine.SceneManagement;


public class HomeScreenController : MonoBehaviour
{
   public void PlayGame()
    {
        SceneManager.LoadScene("CatNap"); //loads into the game
    }

    public void QuitGame()
    {
        Application.Quit(); //this closes the game
        Debug.Log("Game Closing...");

    }
}
