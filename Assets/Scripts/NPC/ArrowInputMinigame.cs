using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
using System.Collections;
using System.Collections.Generic;

public class ArrowInputMinigame : MonoBehaviour
{
    public Text sequenceDisplay; // Reference to the Text UI element
    private int currentIndex = 0;
    private KeyCode[] correctSequence = new KeyCode[6];
    private bool isMinigameActive = false;
    public PatientAI patientAI;
    public PlayerMovementAdvanced playerMovementAdvanced;
    AudioManager audioManager;


     private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void StartMinigame()
    {
        playerMovementAdvanced.SetPlayerFreeze(true);
        correctSequence = new KeyCode[] { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow };
        currentIndex = 0;
        isMinigameActive = true;
        
        // Update UI
        UpdateSequenceDisplay();

        Debug.Log("Arrow Input Minigame Started. Follow the sequence: " + GetSequenceAsString());
    }

    private void UpdateSequenceDisplay()
    {
        sequenceDisplay.text = ""; // Clear previous text
        for (int i = 0; i < correctSequence.Length; i++)
        {
            // Highlight the current arrow in the sequence
            if (i == currentIndex)
            {
                sequenceDisplay.text += "<color=red>" + KeyCodeToArrow(correctSequence[i]) + "</color> ";
            }
            else
            {
                sequenceDisplay.text += KeyCodeToArrow(correctSequence[i]) + " ";
            }
        }
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
            case KeyCode.UpArrow: return "↑";
            case KeyCode.DownArrow: return "↓";
            case KeyCode.LeftArrow: return "←";
            case KeyCode.RightArrow: return "→";
            default: return key.ToString();
        }
    }

    void Update()
    {
        if (isMinigameActive && Input.anyKeyDown)
        {
            if (Input.GetKeyDown(correctSequence[currentIndex]))
            {
                currentIndex++;
                UpdateSequenceDisplay(); // Update the display with the new current index
                Debug.Log("Correct! Continue...");

                if (currentIndex >= correctSequence.Length)
                {
                    Debug.Log("Sequence complete! Minigame won.");
                    audioManager.PlaySFX(audioManager.miniGameOneCorrectAnswer);
                    EndMinigame(true); // Minigame success
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) ||
                     Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                Debug.Log("Wrong input! Minigame failed.");
                EndMinigame(false); // Minigame failure
            }
        }
    }

    void EndMinigame(bool success)
    {
        isMinigameActive = false;
        playerMovementAdvanced.SetPlayerFreeze(false);
        
        // Clear the sequence display text
        sequenceDisplay.text = "";

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
