using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TutorialController : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;

    [Header("Localization Settings")]
    public LocalizedString[] tutorialLines;

    public string sceneName = "CatNap";

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
            SceneManager.LoadScene(sceneName);
        }
    }

    void ShowLine()
    {
        tutorialLines[currentLineIndex].GetLocalizedStringAsync().Completed += (AsyncOperationHandle<string> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                tutorialText.text = handle.Result;
            }
        };
    }
}