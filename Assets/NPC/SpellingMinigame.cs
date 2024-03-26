using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required for setting focus
using System.Collections;
using System.Collections.Generic; // Required for using Lists

public class SpellingMinigame : MonoBehaviour
{
    public GameObject minigameUI; // Assign in inspector
    public InputField inputField; // Assign in inspector
    public Text wordText; // Assign in inspector
    public Button submitButton; // Assign in inspector
    private string currentWord; // Word to spell, will be randomized from a list
    public PatientAI patientAI;
    
    // List of words to use in the minigame
    private List<string> words = new List<string> { "chlamydia", "spondylitis", "hypothyroidism", "schizophrenia", "tuberculosis", "psoriasis", "gonorrhea", "syphilis", "hepatitis Z"};

    void Start()
    {
        Debug.Log("SpellingMinigame: Start called.");
        minigameUI.SetActive(false); // Hide the UI initially
        submitButton.onClick.AddListener(CheckSpelling); // Add click listener to the submit button
    }

    public void StartMinigame()
    {
        Debug.Log("SpellingMinigame: Starting Minigame.");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Randomly select a word from the list
        currentWord = words[Random.Range(0, words.Count)];
        
        wordText.text = $"Spell the word: {currentWord}"; // Set the word in the UI
        inputField.text = ""; // Clear the input field
        minigameUI.SetActive(true); // Show the minigame UI

        StartCoroutine(SetInputFieldFocus());
    }

    IEnumerator SetInputFieldFocus()
    {
        Debug.Log("SpellingMinigame: Setting input field focus.");
        yield return null; // Wait for one frame

        EventSystem.current.SetSelectedGameObject(null); // Clear current selection
        Debug.Log("SpellingMinigame: Current selection cleared.");

        EventSystem.current.SetSelectedGameObject(inputField.gameObject); // Set the input field as the current selected object
        Debug.Log($"SpellingMinigame: Input field selected: {inputField.gameObject.name}");

        // Additional methods to ensure compatibility across different Unity versions and platforms
        inputField.ActivateInputField();
        inputField.Select();
        Debug.Log("SpellingMinigame: Input field activated and selected.");
    }

    void CheckSpelling()
    {
        if (inputField.text.ToLower().Equals(currentWord.ToLower()))
        {
            Debug.Log("SpellingMinigame: Correct spelling!");
            patientAI.MoveToAnotherPointAndDespawn(); // Call MoveToAnotherPointAndDespawn if the spelling is correct
        }
        else
        {
            Debug.Log("SpellingMinigame: Incorrect spelling.");
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        minigameUI.SetActive(false); // Optionally hide the UI after submission
    }

    public void SetNPC(PatientAI npc)
    {
        patientAI = npc; // Method to set the NPC reference
    }

}
