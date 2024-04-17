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
    public int selectedMinigameIndex; // 0 for spelling, 1 for touch and despawn
    private bool fetchMinigameEnded = false; 
    private Animator animator;
    public string desiredPill; 
    public PatientState currentState;
    private bool isDespawningInitiated = false;
    public ArrowInputMinigame arrowInputMinigame; 
    AudioManager audioManager;


    void Start()
    {
        currentState = PatientState.MovingToWaitingRoom;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Get the Animator component
        fetchMinigameEnded = false;

        //selectedMinigameIndex = Random.Range(0, 3); 
        selectedMinigameIndex = 2;
        if(selectedMinigameIndex == 1)
        {
            desiredPill = ChooseRandomPill();
        }
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
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

        animator.SetBool("IsRunning", true);
        agent.destination = waitingRoom.position;

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
            animator.SetBool("IsRunning", true);
            currentState = PatientState.InExamRoom;
        }
    }

    void HandleMovingToExamRoomState()
    {   
        // Transition to InExamRoom when arrived
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = PatientState.InExamRoom;
            // Perform any arrival logic here
        }
    }

    void HandleInExamRoomState()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            animator.SetBool("IsRunning", false);
        }
    }

    void HandlePlayingMinigameState()
    {
        // Logic for playing a minigame
        // Transition to Despawning when minigame ends
    }

    void HandleDespawningState()
    {
        // Logic for despawning
        // Transition to Despawned when arrived
        if (!isDespawningInitiated && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            isDespawningInitiated = true;
            MoveToPointAndDespawn();
        }
    }

    // void Update()
    // {

    //     bool isMoving = agent.velocity.magnitude > 0.1f; // Adjust the threshold as needed
    //     animator.SetBool("IsRunning", isMoving);

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
                audioManager.PlaySFX(audioManager.miniGameOneCorrectAnswer);
                currentState = PatientState.Despawning;
                Destroy(other.gameObject); // Destroy the correct pill GameObject
            }
            else
            {
                Debug.Log(other.gameObject.name + " is colliding. It should be " + desiredPill);
                Debug.Log("Incorrect pill. I am now unalive");
                Unalive(); // NPC despawns
                // Optionally, handle the case for incorrect pill delivery (e.g., provide feedback to the player)
            }
        }
    }

    public void Unalive()
    {
        // Free up the room the NPC was using
        RoomManager.Instance.SetRoomAvailability(currentDestinationPoint, true);
        // Update this line to use despawnPoint instead of waitingRoom
        
        Debug.Log("NPC unalive.");

        // Increment the score by 10
        if(scoreScript != null) // Check if the scoreScript reference is set
        {
            scoreScript.updateScore(-10); // decrease the score by 10
        }
        else
        {
            Debug.LogError("Score script reference not set in NPC.");
        }

        Destroy(gameObject, 10); // Waits 10 seconds before destroying the NPC
    }

    public void MoveToPointAndDespawn()
    {
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
}
