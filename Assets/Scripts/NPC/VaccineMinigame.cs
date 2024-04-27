using UnityEngine;
using System.Collections;

public class VaccineMinigame : MonoBehaviour
{
    public GameObject needleImage;
    public GameObject leftBound;
    public GameObject rightBound;
    public GameObject targetX; // The target "X" object on the arm

    public float needleSpeed = 200.0f;
    public float downwardOffset = -65.0f;
    public float successRadius = 10.0f; // Radius within which the injection is considered successful

    private bool isMoving = true;
    private bool moveRight = true;

    private void Start()
    {
        PositionTargetX();
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveNeedle();
        }

        if (Input.GetKeyDown(KeyCode.Return) && isMoving)
        {
            StopAndInject();
        }
    }

    private void MoveNeedle()
    {
        float targetX = moveRight ? rightBound.transform.position.x : leftBound.transform.position.x;
        float step = needleSpeed * Time.deltaTime;
        float newX = Mathf.MoveTowards(needleImage.transform.position.x, targetX, step);

        if (isMoving)
        {
            needleImage.transform.position = new Vector3(newX, needleImage.transform.position.y, needleImage.transform.position.z);
        }

        if (isMoving)
        {
            if (needleImage.transform.position.x == rightBound.transform.position.x)
            {
                moveRight = false;
            }
            else if (needleImage.transform.position.x == leftBound.transform.position.x)
            {
                moveRight = true;
            }
        }
    }

    private void PositionTargetX()
    {
        // Randomize the X position of the target within the bounds
        float minX = leftBound.transform.position.x;
        float maxX = rightBound.transform.position.x;
        float randomX = Random.Range(minX, maxX);
        targetX.transform.position = new Vector3(randomX, targetX.transform.position.y, targetX.transform.position.z);
    }

    private void StopAndInject()
    {
        isMoving = false; // This stops any further movement updates
        StartCoroutine(MoveNeedleDownward());
    }

    private IEnumerator MoveNeedleDownward()
    {
        Vector3 startPosition = needleImage.transform.position;
        float endY = leftBound.transform.position.y - downwardOffset; // Now using leftBound's Y position minus the offset as the target

        while (needleImage.transform.position.y > endY)
        {
            float step = needleSpeed * Time.deltaTime;
            needleImage.transform.position = new Vector3(startPosition.x, needleImage.transform.position.y - step, startPosition.z);
            yield return null;
        }

        CheckInjectionAccuracy();
    }

    private void CheckInjectionAccuracy()
    {
        float xDistance = Mathf.Abs(needleImage.transform.position.x - targetX.transform.position.x);
        if (xDistance <= successRadius)
        {
            Debug.Log("Injection Successful!");
        }
        else
        {
            Debug.Log("Injection Failed!");
        }
        OnInjectionComplete();
    }

    private void OnInjectionComplete()
    {
        Debug.Log("Injection Complete!");
        // Reset or handle the game ending here
    }
}
