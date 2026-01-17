using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial1 : MonoBehaviour
{

    public void GoToTutorial2()
    {
        SceneManager.LoadScene("Tutorial2");
        Debug.Log("Button pressed");
    }

}