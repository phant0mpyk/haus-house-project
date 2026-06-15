using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{


    [Header("Player Settings")]
    public float moveAmount = 0.5f;     // world units per move
    public float moveDuration = 0.25f;  // seconds

    private bool isMoving;

    public void MoveRight()
    {
        if (!isMoving)
            StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + Vector3.right * moveAmount;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }
}