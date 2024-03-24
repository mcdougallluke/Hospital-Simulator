using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;
    private Animator animator;
    public static bool isWalking = false; // Made public and static for simplicity

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        StartCoroutine(MoveAndWait());
    }

    IEnumerator MoveAndWait()
    {
        while (true)
        {
            agent.SetDestination(target.position);
            isWalking = true;
            animator.SetBool("isWalking", isWalking);

            while (agent.pathPending || agent.remainingDistance > 0.1f)
            {
                yield return null;
            }

            isWalking = false;
            animator.SetBool("isWalking", isWalking);
            yield return new WaitForSeconds(2f);
            break;
        }
    }
}
