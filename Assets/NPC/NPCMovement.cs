using UnityEngine;
using UnityEngine.AI; // Important for working with NavMeshAgent

public class MoveAgent : MonoBehaviour
{
    public Transform target; // Assign this in the inspector with the target location
    private NavMeshAgent agent;

    void Start()
    {
        // Get the NavMeshAgent component attached to this GameObject
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Check if the spacebar was pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Set the destination of the agent to the target's position
            agent.SetDestination(target.position);
        }
    }
}
