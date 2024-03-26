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
    public SpellingMinigame spellingMinigame; // Assign this in the inspector
    public Score scoreScript; // Add this line to hold a reference to the Score script




    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentDestinationPoint = initialPoint;
        MoveToInitialPoint();
        
    }

    void Update()
    {
        // Add checks to ensure the NavMeshAgent is enabled and has a path
        if (!isWaiting && !agent.pathPending && agent.isOnNavMesh && agent.enabled && agent.hasPath && agent.remainingDistance < 0.5f)
        {
            if (!hasArrivedAtWaitingRoom)
            {
                bool foundPoint = CheckAndMoveToOptionalPoint();
                hasArrivedAtWaitingRoom = true;

                if (!foundPoint)
                {
                    isWaiting = true;
                    Debug.Log("No available points found. NPC has arrived at the initial point and is now waiting.");
                }
            }
            else
            {
                if (!isWaiting)
                {
                    Debug.Log("NPC has arrived at the optional point and is now waiting.");
                    hasArrivedAtOptionalPoint = true;
                    isWaiting = true;
                }
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
        // if (hasArrivedAtOptionalPoint && other.CompareTag("Doctor"))
        // {
        //     Debug.Log(hasArrivedAtOptionalPoint);
        //     PointManager.Instance.SetPointAvailability(currentDestinationPoint, true);
        //     // Triggered when a "doctor" tagged object comes near the NPC after it has arrived at an optional point
        //     MoveToAnotherPointAndDespawn();
        // }

        if (hasArrivedAtOptionalPoint && other.CompareTag("Doctor"))
        {
            spellingMinigame.StartMinigame(); // Start the spelling minigame
            spellingMinigame.SetNPC(this); // This line sets the reference to this NPC instance in the spelling minigame

            // Optionally pause NPC actions here
        }
    }



    public void MoveToAnotherPointAndDespawn()
    {
        // Move back to initial point then despawn
        PointManager.Instance.SetPointAvailability(currentDestinationPoint, true);
        agent.destination = initialPoint.position;
        Debug.Log("NPC moving away and despawning.");

        // Increment the score by 10
        if(scoreScript != null) // Check if the scoreScript reference is set
        {
            scoreScript.updateScore(10); // Increase the score by 10
        }
        else
        {
            Debug.LogError("Score script reference not set in NPCWandering.");
        }

        Destroy(gameObject, 10); // Waits x seconds before destroying
    }


    public void ResetAfterRagdoll()
    {
        agent.destination = currentDestinationPoint.position;
    }

}
