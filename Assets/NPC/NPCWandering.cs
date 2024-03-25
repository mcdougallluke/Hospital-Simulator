using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class NPCWandering : MonoBehaviour
{
    public Transform initialPoint;
    public Transform[] optionalPoints;
    private NavMeshAgent agent;
    private bool isWaiting = false;
    private bool hasArrivedAtOptionalPoint = false;
    private bool hasArrivedAtWaitingRoom = false;
    private Transform currentDestinationPoint; 

    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToInitialPoint();
    }

    void Update()
    {
        if(hasArrivedAtWaitingRoom && isWaiting && !hasArrivedAtOptionalPoint)
        {
            bool foundPoint = CheckAndMoveToOptionalPoint();
            
            if(foundPoint)
            {
                isWaiting = false;

            }

        }

        if (!isWaiting && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (!hasArrivedAtWaitingRoom)
            {
                bool foundPoint = CheckAndMoveToOptionalPoint();
                hasArrivedAtWaitingRoom = true;

                // Only set isWaiting to true if no points are available
                if (!foundPoint)
                {
                    isWaiting = true; // NPC will now wait here if no optional points are available
                    Debug.Log("No available points found. NPC has arrived at the initial point and is now waiting.");
                }
            }
            // Removed the else block that was incorrectly logging the arrival at an optional point

            else
            {
                // This condition is met when the NPC arrives at an optional point
                // Log the arrival only once
                if (!isWaiting) // This ensures the message is logged only once upon arrival
                {
                    Debug.Log("NPC has arrived at the optional point and is now waiting.");
                    hasArrivedAtOptionalPoint = true; // NPC has arrived at the optional point

                }
                isWaiting = true; // NPC will now wait here
            }
        }    
    }

    void MoveToInitialPoint()
    {
        agent.destination = initialPoint.position;
    }

    bool CheckAndMoveToOptionalPoint()
    {
        foreach (var point in optionalPoints)
        {
            if (PointManager.Instance.IsPointAvailable(point))
            {
                PointManager.Instance.SetPointAvailability(point, false); // Mark the point as unavailable
                agent.destination = point.position;
                currentDestinationPoint = point; // Store the current destination
                Debug.Log($"Moving to point: {point.name}");
                return true; // Point found
            }
        }

        // If this point is reached, no available points were found
        return false; // No point found
    }


    void TryMoveToOptionalPointAgain()
    {
        foreach (var point in optionalPoints)
        {
            if (PointManager.Instance.IsPointAvailable(point))
            {
                CancelInvoke("TryMoveToOptionalPointAgain"); // Stop checking
                PointManager.Instance.SetPointAvailability(point, false);
                agent.destination = point.position;
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasArrivedAtOptionalPoint && other.CompareTag("Doctor"))
        {
            Debug.Log(hasArrivedAtOptionalPoint);
            PointManager.Instance.SetPointAvailability(currentDestinationPoint, true);
            // Triggered when a "doctor" tagged object comes near the NPC after it has arrived at an optional point
            MoveToAnotherPointAndDespawn();
        }
    }

    void MoveToAnotherPointAndDespawn()
    {
        // Example: Move back to the initial point, then despawn
        agent.destination = initialPoint.position;
        
        // Despawn logic (e.g., disable, destroy, or return to a pool)
        Debug.Log("NPC moving away and despawning.");
        Destroy(gameObject, 5); // Waits 5 seconds before destroying, adjust as needed
    }
}
