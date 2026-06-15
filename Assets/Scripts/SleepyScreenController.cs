using UnityEngine;
using UnityEngine.SceneManagement;

public class SleepyScreenController : MonoBehaviour
{
   
    public void BackToHome()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
