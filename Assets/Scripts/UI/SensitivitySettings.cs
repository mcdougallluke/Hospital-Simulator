using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySettings : MonoBehaviour
{
    public Slider sensitivitySlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("CameraSensitivity"))
        {
            sensitivitySlider.value = PlayerPrefs.GetFloat("CameraSensitivity");
        }
        else
        {
            sensitivitySlider.value = 1f;  // Default value
        }

        sensitivitySlider.onValueChanged.AddListener(HandleSensitivityChange);
    }

    public void HandleSensitivityChange(float sensitivity)
    {
        PlayerPrefs.SetFloat("CameraSensitivity", sensitivity);
        PlayerPrefs.Save();
    }
}

