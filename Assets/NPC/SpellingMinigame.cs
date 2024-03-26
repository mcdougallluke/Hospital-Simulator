using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required for setting focus
using System.Collections;

public class SpellingMinigame : MonoBehaviour
{
    public GameObject minigameUI; // Assign in inspector
    public InputField inputField; // Assign in inspector
    public Text wordText; // Assign in inspector
    public Button submitButton; // Assign in inspector
    private string currentWord = "example"; // Word to spell, could be randomized from a list

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
            // Handle correct spelling (e.g., allow NPC to move)
        }
        else
        {
            Debug.Log("SpellingMinigame: Incorrect spelling.");
            // Handle incorrect spelling (e.g., try again or fail)
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        minigameUI.SetActive(false); // Optionally hide the UI after submission
        Debug.Log("SpellingMinigame: Minigame UI deactivated.");
    }
}
