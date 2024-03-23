using System.Collections;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(MoveBetweenPoints());
    }

    IEnumerator MoveBetweenPoints()
    {
        while (true)
        {
            yield return StartCoroutine(MoveToPoint(pointB.position));
            yield return new WaitForSeconds(1f); // Wait at point B

            yield return StartCoroutine(MoveToPoint(pointA.position));
            yield return new WaitForSeconds(1f); // Wait at point A
        }
    }

    IEnumerator MoveToPoint(Vector3 target)
    {
        animator.SetBool("isWalking", true);

        // Keep moving until very close to the target
        while (Vector3.Distance(transform.position, target) > 0.001f) // Reduced threshold
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }

        // Directly set position to target to prevent getting stuck due to floating-point issues
        transform.position = target;

        animator.SetBool("isWalking", false);
    }
}
