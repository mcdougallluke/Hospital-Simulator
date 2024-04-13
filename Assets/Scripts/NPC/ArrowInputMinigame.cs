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
        if (isMinigameActive && Input.anyKeyDown)
        {
            if (Input.GetKeyDown(correctSequence[currentIndex]))
            {
                currentIndex++;
                Debug.Log("Correct! Continue...");

                if (currentIndex >= correctSequence.Length)
                {
                    Debug.Log("Sequence complete! Minigame won.");
                    isMinigameActive = false;
                    patientAI.currentState = PatientState.Despawning;
                    playerMovementAdvanced.SetPlayerFreeze(false);
                }
            }
            else
            {
                Debug.Log("Wrong input! Minigame failed.");
                isMinigameActive = false;
                playerMovementAdvanced.SetPlayerFreeze(false);
                // Additional failure logic
            }
        }
    }
}
