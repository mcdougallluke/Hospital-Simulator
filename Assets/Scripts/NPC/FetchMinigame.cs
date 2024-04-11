using UnityEngine;

public class FetchMinigame : MonoBehaviour
{
    public PatientAI patientAI; // Reference to the PatientAI script

    // Call this method to start the minigame
    public void StartMinigame()
    { 
    }

    public void SetNPC(PatientAI npc)
    {
        patientAI = npc;
    }

    // New method to be called when the doctor/player brings the correct item
    public void OnItemDelivered()
    {
        patientAI.currentState = PatientState.Despawning;
    }

}