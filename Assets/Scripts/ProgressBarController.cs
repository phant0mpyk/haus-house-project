using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]

//https://youtu.be/J1ng1zA3-Pk?si=zH9g8dNgIDUQugpk , the tutorial I used for this code

public class ProgressBarController : MonoBehaviour
{
    public int maximum;
    public int current;
    public Image mask;


    private void Update()
    {
        GetCurrentFill();
    }
    void GetCurrentFill()
    {
        float fillAmount = (float)current / (float)maximum;
        mask.fillAmount = fillAmount;

    }

}
