using System.Drawing;
using UnityEngine;
using UnityEngine.AI;


public class PatientAI : MonoBehaviour
{
    public Transform despawnPoint;
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
    public int selectedMinigameIndex; // 0 for spelling, 1 for touch and despawn
    private bool fetchMinigameEnded = false; 
    private Animator animator;
    public string desiredPill; 
    private bool isDespawning = false; 


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Get the Animator component
        currentDestinationPoint = waitingRoom;
        MoveToWaitingRoom();
        fetchMinigameEnded = false;

        
        selectedMinigameIndex = Random.Range(0, 2);
        if(selectedMinigameIndex == 1)
        {
            desiredPill = ChooseRandomPill();
        }
    }


    void Update()
    {
        if(isDespawning) return; // Add this line to prevent further actions


        bool isMoving = agent.velocity.magnitude > 0.1f; // Adjust the threshold as needed
        animator.SetBool("IsRunning", isMoving);

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
                    agent.velocity = Vector3.zero; // Explicitly stop the agent
                    agent.isStopped = true; // Prevent the agent from recalculating path
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
                    agent.velocity = Vector3.zero; // Explicitly stop the agent
                    agent.isStopped = true; // Stop the agent
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
                agent.isStopped = false;
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

    public void StartSelectedMinigame()
    {
        if (!hasStartedMinigame && hasArrivedAtExamRoom)
        {
            hasStartedMinigame = true;
            switch (selectedMinigameIndex)
            {
                case 0:
                    Debug.Log("Starting Spelling Minigame.");
                    spellingMinigame.StartMinigame();
                    spellingMinigame.SetNPC(this);
                    break;
                case 1:
                    Debug.Log($"Starting Fetch Minigame. Please bring me the {desiredPill}.");
                    fetchMinigame.StartMinigame();
                    fetchMinigame.SetNPC(this);
                    break;
            }
        }
    }

    private string ChooseRandomPill()
    {
        string[] pills = { "RedPill", "BluePill", "GreenPill" };
        int index = Random.Range(0, pills.Length);
        return pills[index];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item") && selectedMinigameIndex == 1 && hasStartedMinigame && !fetchMinigameEnded)
        {

            string collidedObjectName = other.gameObject.name.Split('(')[0].Trim(); // Splits the name and takes the first part

            if (collidedObjectName.Equals(desiredPill))
            {
                Debug.Log("Correct pill delivered to NPC, ending Fetch Minigame.");
                fetchMinigame.OnItemDelivered(); // Call to end the minigame
                fetchMinigameEnded = true; // Prevent future execution
                Destroy(other.gameObject); // Destroy the correct pill GameObject
            }
            else
            {
                Debug.Log(other.gameObject.name + " is colliding. It should be " + desiredPill);
                Debug.Log("Incorrect pill. Please bring the correct one.");
                // Optionally, handle the case for incorrect pill delivery (e.g., provide feedback to the player)
            }
        }
    }

    public void MoveToPointAndDespawn()
    {
        isDespawning = true; // Set despawning state

        agent.isStopped = false;
        // Free up the room the NPC was using
        RoomManager.Instance.SetRoomAvailability(currentDestinationPoint, true);
        // Update this line to use despawnPoint instead of waitingRoom
        agent.destination = despawnPoint.position; 
        Debug.Log("NPC moving to despawn point and will be despawned.");

        // Increment the score by 10
        if(scoreScript != null) // Check if the scoreScript reference is set
        {
            scoreScript.updateScore(10); // Increase the score by 10
        }
        else
        {
            Debug.LogError("Score script reference not set in NPC.");
        }

        Destroy(gameObject, 10); // Waits 10 seconds before destroying the NPC
    }

    public void ResetAfterRagdoll()
    {
        agent.destination = currentDestinationPoint.position;
    }

    public bool HasArrivedAtExamRoom {
    get { return hasArrivedAtExamRoom; }
}   


}
