using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial2 : MonoBehaviour
{

    public void GoToTutorial3()
    {
        SceneManager.LoadScene("Tutorial3");
    }

}