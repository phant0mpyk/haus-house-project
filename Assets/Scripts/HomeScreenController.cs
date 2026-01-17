using UnityEngine;
using UnityEngine.SceneManagement;


public class HomeScreenController : MonoBehaviour
{
   public void PlayGame()
    {
        SceneManager.LoadScene("Tutorial1"); //loads into the Tutorial Scene
    }

    public void QuitGame()
    {
        Application.Quit(); //this closes the game
        Debug.Log("Game Closing...");

    }
}
