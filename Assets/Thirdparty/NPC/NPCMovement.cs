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

    public IEnumerator MoveAndWait()
    {
        while (true)
        {
            if (agent.enabled) // Only attempt to move if the agent is enabled
            {
                agent.SetDestination(target.position);
                isWalking = true;
                animator.SetBool("isWalking", isWalking);

                // Wait until the path is calculated and the agent has reached the destination
                while (agent.pathPending || (agent.enabled && agent.remainingDistance > 0.1f))
                {
                    yield return null;
                }
            }

            isWalking = false;
            animator.SetBool("isWalking", isWalking);
            yield return new WaitForSeconds(2f); // Adjust the wait time as needed for your game's pacing
            // Removed the break statement to allow continuous execution
        }
    }

}
