using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class NPCWander : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    [Range(0, 100)] public float speed;
    [Range(1, 500)] public float walkRadius;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = speed;
        StartCoroutine(Wander());
    }

    private IEnumerator Wander() {
        while (true) {
            Vector3 newPos = RandomNavmeshLocation();
            agent.SetDestination(newPos);
            animator.SetBool("isWalking", true);

            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance) {
                yield return null;
            }
            animator.SetBool("isWalking", false);

            // Wait for a random time between 3 to 6 seconds
            yield return new WaitForSeconds(Random.Range(3, 7));
        }
    }

    private Vector3 RandomNavmeshLocation() {
        Vector3 randomPosition = Random.insideUnitSphere * walkRadius + transform.position;
        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, walkRadius, 1)) {
            return hit.position;
        }
        return transform.position;
    }
}
