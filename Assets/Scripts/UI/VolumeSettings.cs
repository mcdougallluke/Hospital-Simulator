using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    //private bool isInitialized = false;

    void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume") && PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
            SaveVolume(); // Save the default volume settings
        }
        //isInitialized = true;
    }

    public void SetMusicVolume()
    {
       // if (!isInitialized) return; // Avoid changing volume during initialization
        float volume = musicSlider.value;
        myMixer.SetFloat("Background Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
        SaveVolume(); // Save the new volume settings
    }

    public void SetSFXVolume()
    {
     //   if (!isInitialized) return; // Avoid changing volume during initialization
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        SaveVolume(); // Save the new volume settings
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        SetMusicVolume(); // Apply the loaded volume settings
        SetSFXVolume(); // Apply the loaded volume settings
    }

    private void SaveVolume()
    {
        PlayerPrefs.Save(); // Ensure PlayerPrefs are saved after changing volume
    }
}
