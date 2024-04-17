using UnityEngine;
using UnityEngine.UI;
using System;

public class ArrowInputMinigame : MonoBehaviour
{
    public Text sequenceDisplay; // Reference to the Text UI element
    public Image startImage;     // Reference to the Image UI element for the start of the game
    private int currentIndex = 0;
    private KeyCode[] correctSequence = new KeyCode[9]; // 9 key codes for the sequence
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
        GenerateRandomSequence(); // Generate a random sequence of arrow keys
        currentIndex = 0;
        isMinigameActive = true;
        
        // Display the start image
        startImage.gameObject.SetActive(true);  // Make sure the image is active

        // Update UI
        UpdateSequenceDisplay();

        Debug.Log("Arrow Input Minigame Started. Follow the sequence: " + GetSequenceAsString());
    }

    private void GenerateRandomSequence()
    {
        System.Random random = new System.Random();
        KeyCode[] arrows = {KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow};
        
        for (int i = 0; i < correctSequence.Length; i++)
        {
            correctSequence[i] = arrows[random.Next(arrows.Length)]; // Assign a random arrow key
        }
    }

    private void UpdateSequenceDisplay()
    {
        sequenceDisplay.text = ""; // Clear previous text
        for (int i = 0; i < correctSequence.Length; i++)
        {
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
        startImage.gameObject.SetActive(false);  // Hide the image when the minigame ends

        if (success)
        {
            patientAI.currentState = PatientState.Despawning;
        }
        else
        {
            patientAI.Unalive();
            // This needs to be changed.
        }
    }

}
