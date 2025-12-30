using UnityEngine;
using UnityEngine.UI;




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
