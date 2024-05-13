using System.Drawing;
using System.Security;
using UnityEngine;
using UnityEngine.AI;

public enum PatientState
{
    MovingToWaitingRoom,
    Waiting,
    MovingToExamRoom,
    InExamRoom,
    PlayingMinigame,
    Despawning
}


public class PatientAI : MonoBehaviour
{
    public Transform despawnPoint;
    public Transform waitingRoom;
    public Transform[] examRooms;
    private NavMeshAgent agent;
    private Transform currentDestinationPoint; 
    public SpellingMinigame spellingMinigame;
    public FetchMinigame fetchMinigame;
    public Score scoreScript; 
    private bool hasStartedMinigame = false;
    public int selectedMinigameIndex;
    private bool fetchMinigameEnded = false; 
    private Animator animator;
    public string desiredPill; 
    public PatientState currentState;
    private bool isDespawningInitiated = false;
    public ArrowInputMinigame arrowInputMinigame; 
    public VitalsMinigame vitalsMinigame;
    public VaccineMinigame vaccineMinigame;
    AudioManager audioManager;

    public static int patientsSpawned = 0;



    void Start()
    {
        currentState = PatientState.MovingToWaitingRoom;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        fetchMinigameEnded = false;

        selectedMinigameIndex = Random.Range(0, 5); 
        //selectedMinigameIndex = 0;
        if(selectedMinigameIndex == 1)
        {
            desiredPill = ChooseRandomPill();
        }
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;  // Ensures physics does not move these parts
        }
    }


    void Update()
    {
        switch (currentState)
        {
            case PatientState.MovingToWaitingRoom:
                HandleMovingToWaitingRoomState();
                break;
            case PatientState.Waiting:
                HandleWaitingState();
                break;
            case PatientState.MovingToExamRoom:
                HandleMovingToExamRoomState();
                break;
            case PatientState.InExamRoom:
                HandleInExamRoomState();
                break;
            case PatientState.PlayingMinigame:
                HandlePlayingMinigameState();
                break;
            case PatientState.Despawning:
                HandleDespawningState();
                break;
        }
    }

    void HandleMovingToWaitingRoomState()
    {

        agent.destination = waitingRoom.position;
        animator.SetBool("IsRunning", true);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = PatientState.Waiting;
            // Perform any arrival logic here
        }
    }

    void HandleWaitingState()
    {
        animator.SetBool("IsRunning", false);
        if (CheckAndMoveToOptionalPoint())
        {
            currentState = PatientState.MovingToExamRoom;

        }
    }

    void HandleMovingToExamRoomState()
    {      
        
        animator.SetBool("IsRunning", true);
        // Transition to InExamRoom when arrived
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = PatientState.InExamRoom;
            // Perform any arrival logic here
        }
    }

    void HandleInExamRoomState()
    {
        animator.SetBool("IsRunning", false);
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
        }
    }

    void HandlePlayingMinigameState()
    {
        // Logic for playing a minigame
        // Transition to Despawning when minigame ends
    }

    void HandleDespawningState()
    {
        animator.SetBool("IsRunning", true);
        // Logic for despawning
        // Transition to Despawned when arrived
        if (!isDespawningInitiated && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            isDespawningInitiated = true;
            MoveToPointAndDespawn();
        }
    }

    bool CheckAndMoveToOptionalPoint()
    {
        foreach (var point in examRooms)
        {
            if (RoomManager.Instance.IsRoomAvailable(point))
            {
                Debug.Log("number of patients spawned: " + patientsSpawned);
                
                if(patientsSpawned <= 6){
                    // Disable the NavMeshAgent to perform the teleport
                    agent.enabled = false;
                    transform.position = point.position;  // Teleport the NPC to the room
                    agent.enabled = true;  // Re-enable the NavMeshAgent if needed later

                    // Update the current state and destination point
                    currentDestinationPoint = point;  // Store the current destination
                    RoomManager.Instance.SetRoomAvailability(point, false);  // Mark the room as unavailable

                    // Log the teleportation for debugging
                    Debug.Log($"Teleported to point: {point.name}");

                    // Update the state to be in the exam room directly
                    currentState = PatientState.InExamRoom;

                    return true;  // Indicate successful teleportation
                }

                else if(patientsSpawned > 6)
                {
                    agent.isStopped = false;
                    RoomManager.Instance.SetRoomAvailability(point, false); // Mark the point as unavailable
                    agent.destination = point.position;
                    currentDestinationPoint = point; // Store the current destination
                    Debug.Log($"Moving to point: {point.name}");
                    return true; // Point found
                }
                
            }
        }
        return false;  // No available room found
    }


    public void StartSelectedMinigame()
    {
        if (!hasStartedMinigame)
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
                case 2:
                    Debug.Log("Starting Arrow Input Minigame.");
                    arrowInputMinigame.StartMinigame();
                    arrowInputMinigame.SetNPC(this);
                    break;
                case 3:
                    Debug.Log("Starting Vitals Minigame.");
                    vitalsMinigame.StartMinigame();
                    vitalsMinigame.SetNPC(this);
                    break;
                case 4:
                    Debug.Log("Starting Vaccine Minigame.");
                    vaccineMinigame.StartMinigame();
                    vaccineMinigame.SetNPC(this);
                    break;
            }
        }
    }

    private string ChooseRandomPill()
    {
        string[] pills = { "Red Pill", "Blue Pill", "Green Pill" };
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
                audioManager.PlaySFX(audioManager.miniGameOneCorrectAnswer);
                currentState = PatientState.Despawning;
                Destroy(other.gameObject); // Destroy the correct pill GameObject
            }
            else
            {
                Debug.Log(other.gameObject.name + " is colliding. It should be " + desiredPill);
                Debug.Log("Incorrect pill. I am now unalive");
                audioManager.PlaySFX(audioManager.death);
                Unalive(); // NPC despawns
                Destroy(other.gameObject); // Destroy the correct pill GameObject
                // Optionally, handle the case for incorrect pill delivery (e.g., provide feedback to the player)
            }
        }
    }

    public void Unalive()
    {
        // Disable the Animator
        if (animator != null)
        {
            animator.enabled = false;
        }

        // Activate the ragdoll by enabling the Rigidbody components and setting isKinematic to false
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;  // This allows physics to take control
        }

        scoreScript.UpdateScore(-10);
        scoreScript.UpdatePatientsLost(1);

        // Optionally disable the NavMeshAgent if it interferes with the ragdoll physics
        if (agent != null)
        {
            agent.enabled = false;
        }

        // Log the despawn and manage room availability
        Debug.Log("NPC unalive.");
        RoomManager.Instance.SetRoomAvailability(currentDestinationPoint, true);
        // Destroy the gameObject after some delay (if needed to see the ragdoll effect)
        Destroy(gameObject, 10); // Adjust time as necessary
        RoomManager.Instance.UnregisterNPC(transform);

    }


    public void MoveToPointAndDespawn()
    {
        // Free up the room the NPC was using
        RoomManager.Instance.SetRoomAvailability(currentDestinationPoint, true);
        // Update this line to use despawnPoint instead of waitingRoom
        agent.destination = despawnPoint.position; 
        scoreScript.UpdateScore(10);
        scoreScript.UpdatePatientsSaved(1);
        Debug.Log("NPC moving to despawn point and will be despawned.");

        Destroy(gameObject, 10); // Waits 10 seconds before destroying the NPC
        RoomManager.Instance.UnregisterNPC(transform);

    }
}
