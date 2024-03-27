using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required for setting focus
using System.Collections;
using System.Collections.Generic;

public class SpellingMinigame : MonoBehaviour
{
    public GameObject minigameUI; // Assign in inspector
    public InputField inputField; // Assign in inspector
    public Text wordText; // Assign in inspector
    public Button submitButton; // Assign in inspector
    private string currentWord; // Word to spell, will be randomized from a list
    public PatientAI patientAI;
    public PlayerMovementAdvanced playerMovementAdvanced;
    private List<string> words = new List<string> { "chlamydia", "spondylitis", "hypothyroidism", "schizophrenia", "tuberculosis", "psoriasis", "gonorrhea", "syphilis", "hepatitis Z"};

    void Start()
    {
        minigameUI.SetActive(false);
        submitButton.onClick.AddListener(CheckSpelling);
        inputField.onValueChanged.AddListener(ValidateInput);
        inputField.onEndEdit.AddListener(delegate { OnInputFieldSubmit(inputField.text); });
    }

    public void StartMinigame()
    {
        currentWord = words[Random.Range(0, words.Count)];
        wordText.text = $"You probably have: {currentWord}";
        inputField.text = "";
        minigameUI.SetActive(true);
        playerMovementAdvanced.SetPlayerFreeze(true);
        StartCoroutine(SetInputFieldFocus());
        ValidateInput(inputField.text); // Ensure the button's correct state is set at minigame start
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
            patientAI.MoveToPointAndDespawn();
        }
        else
        {
            Debug.Log("SpellingMinigame: Incorrect spelling.");
        }
        playerMovementAdvanced.SetPlayerFreeze(false);
        minigameUI.SetActive(false);
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
        }
        // Deactivate the input field after checking
        inputField.DeactivateInputField();
    }


    // New method to validate input and toggle the submit button's interactability
    void ValidateInput(string input)
    {
        submitButton.interactable = input.ToLower().Equals(currentWord.ToLower());
    }
}
