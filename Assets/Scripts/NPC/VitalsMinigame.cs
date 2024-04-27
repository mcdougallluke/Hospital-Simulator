using UnityEngine;
using UnityEngine.UI;

public class VitalsMinigame : MonoBehaviour, IPausable
{
    public float acceptableRange = 0.5f;  // Tight control given the scale of 1 to 9
    public GameObject minigameUI;
    public Slider bloodPressureSlider;
    public Slider heartRateSlider;
    public Slider temperatureSlider;

    public Image bloodPressureArrow;
    public Image heartRateArrow;
    public Image temperatureArrow;

    public Button stabilizeButton;  // Reference to the Stabilize button
    public PatientAI patientAI;
    public PlayerMovementAdvanced playerMovementAdvanced;
    private float targetBP, targetHR, targetTemp;

    private AudioManager audioManager;
    public PlayerManager playerManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        playerManager = FindObjectOfType<PlayerManager>();
    }

    public void Start()
    {
        minigameUI.SetActive(false);
        stabilizeButton.onClick.AddListener(CheckVitals);
    }

    public void StartMinigame() {
        playerMovementAdvanced.SetPlayerFreeze(true);
        FindObjectOfType<PauseMenu>().SetActivePausable(this);
        minigameUI.SetActive(true);
        playerManager.isGamePaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SetRandomTargets();
        RandomizeSliderPositions();
        Debug.Log("Vital Signs Minigame Started. Set the vitals to stabilize the patient.");
    }

    private void SetRandomTargets()
    {
        targetBP = Random.Range(0, 21);  
        targetHR = Random.Range(0, 21);
        targetTemp = Random.Range(0, 21);

        Debug.Log("Target values set to: BP: " + targetBP + ", HR: " + targetHR + ", Temp: " + targetTemp);

        // Position arrow markers by temporarily setting slider values
        SetArrowPosition(bloodPressureSlider, bloodPressureArrow, targetBP);
        SetArrowPosition(heartRateSlider, heartRateArrow, targetHR);
        SetArrowPosition(temperatureSlider, temperatureArrow, targetTemp);
    }

    private void RandomizeSliderPositions()
    {
        bloodPressureSlider.value = Random.Range(0, 21);
        heartRateSlider.value = Random.Range(0, 21);
        temperatureSlider.value = Random.Range(0, 21);
        Debug.Log("Values set to: BP: " + bloodPressureSlider.value + ", HR: " + heartRateSlider.value + ", Temp: " + temperatureSlider.value);
    }

    private void SetArrowPosition(Slider slider, Image arrow, float targetValue)
    {
        float originalValue = slider.value;
        slider.value = targetValue;
        Canvas.ForceUpdateCanvases();
        Vector3 handlePosition = slider.handleRect.localPosition;
        arrow.rectTransform.anchoredPosition = new Vector2(handlePosition.x, arrow.rectTransform.anchoredPosition.y);
        slider.value = originalValue;
        Canvas.ForceUpdateCanvases();
    }


    public void CheckVitals()
    {
        if (Mathf.Abs(bloodPressureSlider.value - targetBP) < acceptableRange &&
            Mathf.Abs(heartRateSlider.value - targetHR) < acceptableRange &&
            Mathf.Abs(temperatureSlider.value - targetTemp) < acceptableRange)
        {
            EndMinigame(true);
            audioManager.PlaySFX(audioManager.miniGameOneCorrectAnswer);
            Debug.Log("Patient stabilized!");
        }
        else
        {
            EndMinigame(false);
            audioManager.PlaySFX(audioManager.death);
            Debug.Log("Patient lost!");
        }
    }

    public void SetNPC(PatientAI npc)
    {
        patientAI = npc;
    }

    public void EndMinigame(bool success) {
        playerMovementAdvanced.SetPlayerFreeze(false);
        playerManager.isGamePaused = false;
        minigameUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (success)
        {
            patientAI.currentState = PatientState.Despawning;
        }
        else 
        {
            patientAI.Unalive();
        }
    }

    public void OnGamePaused()
    {

    }

    public void OnGameResumed()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerManager.isGamePaused = true;
    }
}
