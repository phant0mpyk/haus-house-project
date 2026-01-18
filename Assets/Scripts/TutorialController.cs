using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class TutorialController : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;

    [TextArea(3, 5)]
    public string[] tutorialLines; // public, so I can add the text in unity, those are the tutorial lines

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
            //loading the game scene after text is done, this is good so I can add as much text as I want and the game recorgnizes once all the text is over it can now switch to the game, like this its easier to add lines
            SceneManager.LoadScene("CatNap");
        }
    }

    void ShowLine()
    {
        tutorialText.text = tutorialLines[currentLineIndex];
    }
}
