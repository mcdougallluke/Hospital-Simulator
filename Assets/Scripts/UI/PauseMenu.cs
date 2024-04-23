 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Michsky.MUIP;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    private int currentSelection = 0;
    public GameObject[] menuOptions;
    private ButtonManager[] buttonManagers; // Array to hold ButtonManager components for menu options
    public ScoreData scoreData;
    public RoomManager roomManager;
    public Canvas canvasObject1;
    public Canvas canvasObject2;
    public GameObject arrowMinigameObject;
    AudioManager audioManager;
    public InputField inputField;
    public GameObject ExamRoomPanel;
    public PlayerManager playerManager;

    public GameObject menuContainer;  
    public GameObject settingsContainer;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        playerManager = FindObjectOfType<PlayerManager>();
    }

    void Start()
    {
        //REMOVE CURSOR MOVEMENT
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        buttonManagers = new ButtonManager[menuOptions.Length];

        // Get ButtonManager components for each menu option
        for (int i = 0; i < menuOptions.Length; i++)
        {
            buttonManagers[i] = menuOptions[i].GetComponent<ButtonManager>();
        }

        menuContainer.SetActive(true);
        settingsContainer.SetActive(false);
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                if (settingsContainer.activeSelf)  // If in settings menu, go back to main menu
                {
                    HideSettings();
                }
                else  // Otherwise, resume game or show main menu
                {
                    if (pauseMenuUI.activeSelf)
                    {
                        Resume();
                    }
                    else
                    {
                        Pause();
                    }
                }
            }
            else
            {
                Pause();
            }
        }
    }

    private void UpdateSelection()
    {
        for (int i = 0; i < menuOptions.Length; i++)
        {
            if (buttonManagers[i] != null)
            {
                if (i == currentSelection)
                {
                    // Set the button's state properties for highlight effect
                    buttonManagers[i].normalCG.alpha = 0; // the normal state appearance
                    buttonManagers[i].highlightCG.alpha = 1; // the highlight state appearance
                    buttonManagers[i].disabledCG.alpha = 0; // the disabled state appearance
                }
                else
                {
                    // Reset other buttons' state properties
                    buttonManagers[i].normalCG.alpha = 1;
                    buttonManagers[i].highlightCG.alpha = 0;
                    buttonManagers[i].disabledCG.alpha = 0;
                }
            }
        }
    }
    private void ExecuteSelection()
    {
        switch (currentSelection)
        {
            case 0: // Resume
                Resume();
                break;
            case 1: // Load Menu
                LoadMenu();
                break;
            case 2: // Quit Game
                QuitGame();
                break;
        }
    }

    public void Resume()
    {
        audioManager.PlaySFX(audioManager.buttonPressed);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        inputField.interactable = true;
        canvasObject1.enabled = true;
        arrowMinigameObject.SetActive(true);
        ExamRoomPanel.SetActive(true);
        if (playerManager != null)
            playerManager.isGamePaused = false;  // Update PlayerManager's pause state
    }

    public void Pause()
    {
        audioManager.PlaySFX(audioManager.buttonPressed);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        inputField.interactable = false;
        canvasObject1.enabled = false;
        arrowMinigameObject.SetActive(false);
        ExamRoomPanel.SetActive(false);
        if (playerManager != null)
            playerManager.isGamePaused = true;  // Update PlayerManager's pause state
    }


    public void LoadMenu()
    {
        audioManager.PlaySFX(audioManager.buttonPressed);
        RoomManager.Instance.ResetAllRoomsAvailability();
        scoreData.score = 0;
        Time.timeScale = 1f;
        Debug.Log("Loading main menu");
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        audioManager.PlaySFX(audioManager.buttonPressed);
        RoomManager.Instance.ResetAllRoomsAvailability();
        scoreData.score = 0;
        Debug.Log("Quitting game...");
        Application.Quit();
    }


    public void ShowSettings()
    {
        audioManager.PlaySFX(audioManager.buttonPressed);
        menuContainer.SetActive(false);
        settingsContainer.SetActive(true);
    }

    public void HideSettings()
    {
        audioManager.PlaySFX(audioManager.buttonPressed);
        settingsContainer.SetActive(false);
        menuContainer.SetActive(true);
    }

}