using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    [SerializeField] private string sceneName;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Changing Scenes");
            SceneManager.LoadScene(sceneName);
        }
    }
}