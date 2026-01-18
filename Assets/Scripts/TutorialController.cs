using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class TutorialController : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;

    [TextArea(3, 5)]
    public string[] tutorialLines;

    public string SceneName;

    private int currentLineIndex = 0;

    void Start()
    {
        ShowLine();
    }

    public void NextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < tutorialLines.Length)
        {
            ShowLine();

        }
        else
        {
            //loading the game scene after text is done
            SceneManager.LoadScene("CatNap");
        }
    }

    void ShowLine()
    {
        tutorialText.text = tutorialLines[currentLineIndex];
    }
}
