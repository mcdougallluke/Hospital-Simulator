using UnityEngine;
using UnityEngine.UI;

public class VitalSignsGame : MonoBehaviour
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

    private float targetBP, targetHR, targetTemp;

    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        minigameUI.SetActive(false);
        SetRandomTargets();
        RandomizeSliderPositions();
        stabilizeButton.onClick.AddListener(CheckVitals); // Attach the CheckVitals method to the button click event
    }

    void SetRandomTargets()
    {
        targetBP = Random.Range(1, 10);  // Generate targets within 1-9
        targetHR = Random.Range(1, 10);
        targetTemp = Random.Range(1, 10);

        // Position arrow markers
        bloodPressureArrow.rectTransform.anchoredPosition = new Vector2(CalculateMarkerPosition(bloodPressureSlider, targetBP), bloodPressureArrow.rectTransform.anchoredPosition.y);
        heartRateArrow.rectTransform.anchoredPosition = new Vector2(CalculateMarkerPosition(heartRateSlider, targetHR), heartRateArrow.rectTransform.anchoredPosition.y);
        temperatureArrow.rectTransform.anchoredPosition = new Vector2(CalculateMarkerPosition(temperatureSlider, targetTemp), temperatureArrow.rectTransform.anchoredPosition.y);
    }

    void RandomizeSliderPositions()
    {
        bloodPressureSlider.value = Random.Range(1, 10); // Random starting positions within 1-9
        heartRateSlider.value = Random.Range(1, 10);
        temperatureSlider.value = Random.Range(1, 10);
    }

    float CalculateMarkerPosition(Slider slider, float targetValue)
    {
        // Calculate the position of the target marker on the slider
        float handleCenter = slider.fillRect.localPosition.x + slider.fillRect.rect.width * (targetValue - slider.minValue) / (slider.maxValue - slider.minValue);
        return handleCenter - slider.handleRect.rect.width * 0.5f; // Center the marker
    }

    public void CheckVitals()
    {
        if (Mathf.Abs(bloodPressureSlider.value - targetBP) < acceptableRange &&
            Mathf.Abs(heartRateSlider.value - targetHR) < acceptableRange &&
            Mathf.Abs(temperatureSlider.value - targetTemp) < acceptableRange)
        {
            Debug.Log("Patient stabilized!");
        }
        else
        {
            Debug.Log("Patient lost!");
        }
    }
}
