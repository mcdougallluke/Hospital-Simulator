using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SpellingMinigame : MonoBehaviour, IPausable
{
    public GameObject minigameUI; // Assign in inspector
    public InputField inputField; // Assign in inspector
    public Text wordText; // Assign in inspector
    public Button submitButton; // Assign in inspector
    private string currentWord; // Word to spell, will be randomized from a list
    public PatientAI patientAI;
    public PlayerMovementAdvanced playerMovementAdvanced;
    private List<string> words = new List<string> { "chlamydia", "spondylitis", "hypothyroidism", "schizophrenia", "tuberculosis", "psoriasis", "gonorrhea", "syphilis", "ebola", "rabies", "smallpox", "blackdeath", "cholera", "typhus", "measles", "scurvy", "leukemia", "anthrax", "malaria", "Pneumonoultramicroscopicsilicovolcanoconiosis"};
    public Text overlayText; // Assign this in the inspector to the Text component that overlays the InputField

    AudioManager audioManager;
    private bool isGameComplete = false;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    
    void Start()
    {
        minigameUI.SetActive(false);
        isGameComplete = false;
        submitButton.onClick.AddListener(CheckSpelling);
        inputField.onValueChanged.AddListener(ValidateInput);
        inputField.onEndEdit.AddListener(delegate { OnInputFieldSubmit(inputField.text); });
    }

    public void StartMinigame()
    {
        FindObjectOfType<PauseMenu>().SetActivePausable(this);
        isGameComplete = false;
        currentWord = words[Random.Range(0, words.Count)];
        inputField.text = ""; // Clear the input field
        overlayText.text = "<color=#808080FF>" + currentWord + "</color>"; // Set overlay text in grey

        minigameUI.SetActive(true);
        playerMovementAdvanced.SetPlayerFreeze(true);
        StartCoroutine(SetInputFieldFocus());
    }

    IEnumerator SetInputFieldFocus()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(inputField.gameObject);
        inputField.ActivateInputField();
        inputField.Select();
    }

    void Update()
    {
        // Continuously check if the input field needs to be refocused
        RefocusInputField();
    }

    void RefocusInputField()
    {
        if (EventSystem.current.currentSelectedGameObject != inputField.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(inputField.gameObject);
        }
    }

    void CheckSpelling()
    {
        if (inputField.text.ToLower().Equals(currentWord.ToLower()))
        {
            patientAI.currentState = PatientState.Despawning;
        }
        else
        {
            Debug.Log("SpellingMinigame: Incorrect spelling.");
        }
        playerMovementAdvanced.SetPlayerFreeze(false);
        minigameUI.SetActive(false);
        isGameComplete = true;
    }

    public void SetNPC(PatientAI npc)
    {
        patientAI = npc;
    }

    private void OnInputFieldSubmit(string input)
    {
        // Check if the Enter or Keypad Enter key is pressed and the submit button is interactable
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && submitButton.interactable)
        {
            CheckSpelling();
            audioManager.PlaySFX(audioManager.miniGameOneCorrectAnswer);
        }
        // Deactivate the input field after checking
        inputField.DeactivateInputField();
    }

    void ValidateInput(string input)
    {
        // Ensure all input is treated in lowercase to standardize comparisons
        string lowerInput = input.ToLower();

        // Initialize the length of correct input
        int correctChars = 0;

        // Compare each character inputted to the corresponding character in the target word
        for (int i = 0; i < lowerInput.Length; i++)
        {
            if (i < currentWord.Length && lowerInput[i] == currentWord.ToLower()[i])
                correctChars++;
            else
                break; // Stop the loop if a character does not match
        }

        // If the input length exceeds the correct characters or mismatches, revert it
        if (lowerInput.Length > correctChars)
        {
            inputField.text = lowerInput.Substring(0, correctChars); // Only allow correct input
            inputField.caretPosition = correctChars; // Reset the caret position
        }
        else
        {
            // Update the overlay text with correct letters in black and remaining in grey
            string correctPart = "<color=#000000FF>" + currentWord.Substring(0, correctChars) + "</color>";
            string remainingPart = "<color=#808080FF>" + currentWord.Substring(correctChars) + "</color>";
            overlayText.text = correctPart + remainingPart;

            // Enable submit button only when the entire word is correctly typed
            submitButton.interactable = (correctChars == currentWord.Length);
        }
    }

    public void OnGamePaused()
    {

    }

    public void OnGameResumed()
    {
        if (!isGameComplete)
        {
            playerMovementAdvanced.SetPlayerFreeze(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
            RefocusInputField();
        }
    }

    public void playSound()
    {
        audioManager.PlaySFX(audioManager.miniGameOneCorrectAnswer);
    }
}
