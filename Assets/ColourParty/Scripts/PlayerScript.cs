using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{


    [Header("Player Settings")]
    public float moveDuration = 1f;
    public float moveDistance = 2.5f;

    public void MoveRight()
    {
        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        Vector3 start = transform.position;
        Vector3 end = start + new Vector3(moveDistance, 0, 0);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration; // increase t over time
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end; // ensure exact final position
    }
}