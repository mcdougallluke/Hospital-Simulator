using System.Drawing;
using UnityEngine;
using UnityEngine.AI;


public class PatientAI : MonoBehaviour
{
    public Transform waitingRoom;
    public Transform[] examRooms;
    private NavMeshAgent agent;
    private bool isWaiting = false;
    private bool hasArrivedAtExamRoom = false;
    private bool hasArrivedAtWaitingRoom = false;
    private Transform currentDestinationPoint; 
    public SpellingMinigame spellingMinigame;
    public FetchMinigame fetchMinigame;
    public Score scoreScript; 
    private bool hasStartedMinigame = false;
    private int selectedMinigameIndex; // 0 for spelling, 1 for touch and despawn
    private bool fetchMinigameEnded = false; // Add this field



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentDestinationPoint = waitingRoom;
        MoveToWaitingRoom();
        fetchMinigameEnded = false; // Ensure it's false at the start
        
        // Randomly select a minigame for the NPC
        selectedMinigameIndex = Random.Range(0, 2);
        
    }

    void Update()
    {
        if(hasArrivedAtWaitingRoom && isWaiting && !hasArrivedAtExamRoom)
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
            else
            {
                // This condition is met when the NPC arrives at an optional point
                // Log the arrival only once
                if (!isWaiting) // This ensures the message is logged only once upon arrival
                {
                    Debug.Log("NPC has arrived at the optional point and is now waiting.");
                    hasArrivedAtExamRoom = true; // NPC has arrived at the optional point

                }
                isWaiting = true; // NPC will now wait here
            }
        }    
    }


    void MoveToWaitingRoom()
    {
        agent.destination = waitingRoom.position;
    }

    bool CheckAndMoveToOptionalPoint()
    {
        foreach (var point in examRooms)
        {
            if (RoomManager.Instance.IsRoomAvailable(point))
            {
                RoomManager.Instance.SetRoomAvailability(point, false); // Mark the point as unavailable
                agent.destination = point.position;
                currentDestinationPoint = point; // Store the current destination
                Debug.Log($"Moving to point: {point.name}");
                return true; // Point found
            }
        }

        // If this point is reached, no available points were found
        return false; // No point found
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter called. SelectedMinigameIndex: {selectedMinigameIndex}, HasStartedMinigame: {hasStartedMinigame}");

        if (hasArrivedAtExamRoom && other.CompareTag("Doctor") && !hasStartedMinigame)
        {

            hasStartedMinigame = true;

            if (selectedMinigameIndex == 0)
            {
                Debug.Log("Starting Spelling Minigame.");
                spellingMinigame.StartMinigame();
                spellingMinigame.SetNPC(this);
            }
            else if (selectedMinigameIndex == 1) // This is the fetch minigame
            {
                Debug.Log("Starting Fetch Minigame.");
                fetchMinigame.StartMinigame();
                fetchMinigame.SetNPC(this); // Ensure the NPC is set for the fetchMinigame
            }
        }

        if (other.CompareTag("Item") && selectedMinigameIndex == 1 && hasStartedMinigame && !fetchMinigameEnded)
        {
            Debug.Log("Item delivered to NPC, ending Fetch Minigame.");
            fetchMinigame.OnItemDelivered(); // Call to end the fetch minigame
            fetchMinigameEnded = true; // Prevent future execution
        }
    }




    public void MoveToPointAndDespawn()
    {
        // Move back to initial point then despawn
        RoomManager.Instance.SetRoomAvailability(currentDestinationPoint, true);
        agent.destination = waitingRoom.position;
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
