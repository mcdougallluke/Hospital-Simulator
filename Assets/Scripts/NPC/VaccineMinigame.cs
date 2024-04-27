using UnityEngine;
using System.Collections;

public class VaccineMinigame : MonoBehaviour
{
    public GameObject minigameUI;
    public GameObject needleImage;
    public GameObject leftBound;
    public GameObject rightBound;
    public GameObject targetX;
    public Vector3 initialNeedlePosition;
    public float needleSpeed = 200.0f;
    public float downwardOffset = -65.0f;
    public float successRadius = 10.0f;

    private bool isMoving = true;
    private bool moveRight = true;
    private bool gameIsActive = false;

    public PatientAI patientAI;
    public PlayerMovementAdvanced playerMovementAdvanced;
    private AudioManager audioManager;
    public PlayerManager playerManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        initialNeedlePosition = needleImage.transform.position;
    }

    private void Start()
    {
        minigameUI.SetActive(false);
    }

    public void StartMinigame() {
        ResetMinigame(); // Reset the game to the initial state before starting
        gameIsActive = true;
        playerMovementAdvanced.SetPlayerFreeze(true);
        minigameUI.SetActive(true);
        playerManager.freezeCamera = true;
        Debug.Log("Vaccine Minigame Started. Press Enter to inject the vaccine.");
    }

    private void Update()
    {
        if (gameIsActive) {
            if (isMoving)
            {
                MoveNeedle();
            }

            if (Input.GetKeyDown(KeyCode.Return) && isMoving && !PauseMenu.GameIsPaused)
            {
                StopAndInject();
            }
        }
    }

    private void MoveNeedle()
    {
        float targetX = moveRight ? rightBound.transform.position.x : leftBound.transform.position.x;
        float step = needleSpeed * Time.deltaTime;
        float newX = Mathf.MoveTowards(needleImage.transform.position.x, targetX, step);

        if (isMoving)
        {
            needleImage.transform.position = new Vector3(newX, needleImage.transform.position.y, needleImage.transform.position.z);
        }

        if (isMoving)
        {
            if (needleImage.transform.position.x == rightBound.transform.position.x)
            {
                moveRight = false;
            }
            else if (needleImage.transform.position.x == leftBound.transform.position.x)
            {
                moveRight = true;
            }
        }
    }

    private void PositionTargetX()
    {
        float minX = leftBound.transform.position.x;
        float maxX = rightBound.transform.position.x;
        float randomX = Random.Range(minX, maxX);
        targetX.transform.position = new Vector3(randomX, targetX.transform.position.y, targetX.transform.position.z);
    }

    private void StopAndInject()
    {
        isMoving = false;
        StartCoroutine(MoveNeedleDownward());
    }

    private IEnumerator MoveNeedleDownward()
    {
        Vector3 startPosition = needleImage.transform.position;
        float endY = leftBound.transform.position.y - downwardOffset;

        while (needleImage.transform.position.y > endY)
        {
            float step = needleSpeed * Time.deltaTime;
            needleImage.transform.position = new Vector3(startPosition.x, needleImage.transform.position.y - step, startPosition.z);
            yield return null;
        }

        CheckInjectionAccuracy();
    }

    private void CheckInjectionAccuracy()
    {
        gameIsActive = false;
        float xDistance = Mathf.Abs(needleImage.transform.position.x - targetX.transform.position.x);
        if (xDistance <= successRadius)
        {
            Debug.Log("Injection Successful!");
            audioManager.PlaySFX(audioManager.miniGameOneCorrectAnswer);
            EndMinigame(true);
        }
        else
        {
            Debug.Log("Injection Failed!");
            audioManager.PlaySFX(audioManager.death);
            EndMinigame(false);
        }
    }

    public void SetNPC(PatientAI npc)
    {
        patientAI = npc;
    }

    private void ResetMinigame()
    {
        isMoving = true;
        moveRight = true;
        needleImage.transform.position = initialNeedlePosition;
        PositionTargetX();
    }

    private void EndMinigame(bool success) {
        minigameUI.SetActive(false);
        playerManager.freezeCamera = false;
        playerMovementAdvanced.SetPlayerFreeze(false);
        if (success)
        {
            patientAI.currentState = PatientState.Despawning;
        }
        else
        {
            patientAI.Unalive();
        }
    }
}
