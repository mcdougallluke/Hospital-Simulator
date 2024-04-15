using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
using System.Collections;
using System.Collections.Generic;

public class ArrowInputMinigame : MonoBehaviour
{
    private int currentIndex = 0;
    private KeyCode[] correctSequence = new KeyCode[6];
    private bool isMinigameActive = false;
    public PatientAI patientAI; // Reference to the PatientAI script
    public PlayerMovementAdvanced playerMovementAdvanced;
    AudioManager audioManager;


    public void StartMinigame()
    {
        playerMovementAdvanced.SetPlayerFreeze(true);
        // Define a fixed sequence or randomize it
        correctSequence = new KeyCode[] { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow };
        currentIndex = 0;
        isMinigameActive = true;
        
        // Log the sequence for the player
        Debug.Log("Arrow Input Minigame Started. Follow the sequence: " + GetSequenceAsString());
    }
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void SetNPC(PatientAI npc)
    {
        patientAI = npc;
    }

    private string GetSequenceAsString()
    {
        string sequenceString = "";
        foreach (KeyCode key in correctSequence)
        {
            sequenceString += KeyCodeToArrow(key) + " ";
        }
        return sequenceString.Trim();
    }

    private string KeyCodeToArrow(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.UpArrow:
                return "↑";
            case KeyCode.DownArrow:
                return "↓";
            case KeyCode.LeftArrow:
                return "←";
            case KeyCode.RightArrow:
                return "→";
            default:
                return key.ToString();
        }
    }

    void Update()
{
    if (isMinigameActive)
    {
        // Only listen for arrow key inputs
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Check if the pressed key matches the current key in the sequence
            if (Input.GetKeyDown(correctSequence[currentIndex]))
            {
                currentIndex++;
                Debug.Log("Correct! Continue...");

                // Check if the sequence is complete
                if (currentIndex >= correctSequence.Length)
                {
                    Debug.Log("Sequence complete! Minigame won.");
                    audioManager.PlaySFX(audioManager.miniGameOneCorrectAnswer);
                    EndMinigame(true); // Minigame success
                }
            }
            else
            {
                Debug.Log("Wrong input! Minigame failed.");
                EndMinigame(false); // Minigame failure
            }
        }
    }
}

void EndMinigame(bool success)
{
    isMinigameActive = false;
    playerMovementAdvanced.SetPlayerFreeze(false);

    if (success)
    {
        patientAI.currentState = PatientState.Despawning;
    }
    else
    {
        // Handle the minigame failure case (optional)
    }
}
}
