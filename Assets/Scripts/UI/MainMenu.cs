using UnityEngine;
using UnityEngine.SceneManagement;
using Michsky.MUIP;

public class MainMenu : MonoBehaviour
{
    public GameObject[] menuOptions;
    private int currentSelection = 0;
    private ButtonManager[] buttonManagers; // Array to hold ButtonManager components for menu options
    public ScoreData scoreData;
    void Start()
    {
        //ENABLE CURSOR MOVEMENT
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        buttonManagers = new ButtonManager[menuOptions.Length];

        // Get ButtonManager components for each menu option
        for (int i = 0; i < menuOptions.Length; i++)
        {
            buttonManagers[i] = menuOptions[i].GetComponent<ButtonManager>();
        }

        // Set the initial selection
        UpdateSelection();
    }

    void Update()
    {
        // Handle arrow key input for navigation
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
        // Handle selection confirmation
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteSelection();
        }
    }

    private void UpdateSelection()
    {
        for (int i = 0; i < menuOptions.Length; i++)
        {
            if (buttonManagers[i] != null)
            {
                // Highlight the currently selected option
                if (i == currentSelection)
                {
                    buttonManagers[i].normalCG.alpha = 0;
                    buttonManagers[i].highlightCG.alpha = 1;
                    buttonManagers[i].disabledCG.alpha = 0;
                }
                else
                {
                    // Reset other options
                    buttonManagers[i].normalCG.alpha = 1;
                    buttonManagers[i].highlightCG.alpha = 0;
                    buttonManagers[i].disabledCG.alpha = 0;
                }
            }
        }
    }

    private void ExecuteSelection()
    {
        // Get the name of the currently selected button
        string buttonName = menuOptions[currentSelection].name;

        // Execute the action associated with the current button name
        switch (buttonName)
        {
            case "Play Button": 
                PlayGame();
                break;
            case "Credits Button": 
                Credits();
                break;
            case "Quit Button": 
                QuitGame();
                break;
            case "Back Button": 
                Back();
                break;
            case "MainMenu Button": 
                LoadMainMenu();
                break;
            case "Tutorial":
                Tutorial();
                break;
                // Add cases for other buttons as needed
        }
    }

    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void PlayGame()
    {
        audioManager.PlaySFX(audioManager.buttonPressed);
        scoreData.score = 0;
        scoreData.patientsSaved = 0;
        scoreData.patientsLost = 0;
        Cursor.visible = true;
        SceneManager.LoadScene(1);
    }
    public void Credits()
    {
        audioManager.PlaySFX(audioManager.buttonPressed);
        scoreData.score = 0;
       // Debug.Log("Going to credits scene.");
        SceneManager.LoadScene(2);
    }
    public void QuitGame()
    {
        audioManager.PlaySFX(audioManager.buttonPressed);
        Application.Quit();
        Debug.Log("Quiting Game.");
    }

    public void Back()
    {
        audioManager.PlaySFX(audioManager.buttonPressed);
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadMainMenu()
    {
        audioManager.PlaySFX(audioManager.buttonPressed);
        SceneManager.LoadScene("MainMenu");
    }
    public void Tutorial()
    {
        audioManager.PlaySFX(audioManager.buttonPressed);
        //Debug.Log("Going to tutorial page 1");
        SceneManager.LoadScene("TutorialPage1");
    }
}