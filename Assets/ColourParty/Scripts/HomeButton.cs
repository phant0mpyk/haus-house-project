using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeButton : MonoBehaviour
{
    [SerializeField] private string MainMenu;
    public Button button;

    private void Start()
    {
        button.onClick.AddListener(Clicked);
    }
    void Clicked()
    {
                Debug.Log("Changing Scenes");
                SceneManager.LoadScene(MainMenu);
          
    }

}