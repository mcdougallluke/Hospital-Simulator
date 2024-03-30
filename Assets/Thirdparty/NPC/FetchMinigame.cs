using UnityEngine;

public class FetchMinigame : MonoBehaviour
{
    public GameObject minigameUI; // Assign this in the inspector
    private PatientAI patientAI; // Reference to the PatientAI script

    public void StartMinigame(PatientAI patient)
    {
        patientAI = patient;
        minigameUI.SetActive(true);
        // Initialize your minigame here (e.g., set up the game environment, display instructions, etc.)
        Debug.Log("Fetch Minigame started!");
    }

    // Call this method to end the minigame
    public void EndMinigame()
    {
        minigameUI.SetActive(false);
        patientAI.MoveToPointAndDespawn();
        // Handle the minigame completion here (e.g., cleanup, rewarding the player, etc.)
        Debug.Log("Fetch Minigame ended!");
    }
}
