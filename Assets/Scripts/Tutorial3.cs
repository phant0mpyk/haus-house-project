using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial3 : MonoBehaviour
{

    public void GoToGame()
    {
        SceneManager.LoadScene("CatNap");
    }

}