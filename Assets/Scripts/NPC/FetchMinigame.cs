using UnityEngine;

public class FetchMinigame : MonoBehaviour, IPausable
{
    public PatientAI patientAI; // Reference to the PatientAI script
    public PlayerManager playerManager; // Reference to the PlayerManager script

    // Call this method to start the minigame
    public void Awake() {
        playerManager = FindObjectOfType<PlayerManager>();
    }
    public void StartMinigame()
    {
        FindObjectOfType<PauseMenu>().SetActivePausable(this);
    }

    public void SetNPC(PatientAI npc)
    {
        patientAI = npc;
    }

    public void OnGamePaused()
    {

    }

    public void OnGameResumed()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerManager.freezeCamera = false;
    }
}