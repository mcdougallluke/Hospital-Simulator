 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Michsky.MUIP;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    private int currentSelection = 0;
    public GameObject[] menuOptions;
    private ButtonManager[] buttonManagers; // Array to hold ButtonManager components for menu options
    public ScoreData scoreData;
    public RoomManager roomManager;

    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
          
            if (GameIsPaused)
            {

                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (GameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentSelection = (currentSelection - 1 + menuOptions.Length) % menuOptions.Length;
                UpdateSelection();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentSelection = (currentSelection + 1) % menuOptions.Length;
                UpdateSelection();
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                ExecuteSelection();
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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        audioManager.PlaySFX(audioManager.buttonPressed);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        audioManager.PlaySFX(audioManager.buttonPressed);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
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
}