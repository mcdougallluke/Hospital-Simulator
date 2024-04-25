using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("----- Audio Source -----")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource footstepsSource;

    [Header("----- Audio Clip -----")]
    public AudioClip walking;
    public AudioClip running;
    public AudioClip jump;
    public AudioClip pickupItem;
    public AudioClip dropItem;
    public AudioClip grapple;
    public AudioClip miniGameOneCorrectAnswer;
    public AudioClip buttonPressed;
    public AudioClip death;
    public AudioClip sprint;

    [Header("----- Background Music -----")]
    public AudioClip gameBackground;
    public AudioClip mainMenuBackground;
    public AudioClip creditsBackground;
    public AudioClip endBackground;

    private Dictionary<string, AudioClip> sceneBackgroundMusic = new Dictionary<string, AudioClip>();
    private static AudioManager instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            SFXSource.volume = 0.25f;
            sceneBackgroundMusic.Add("HospitalMap 1", gameBackground);
            sceneBackgroundMusic.Add("MainMenu", mainMenuBackground);
            sceneBackgroundMusic.Add("Credits", creditsBackground);
            sceneBackgroundMusic.Add("GameOver", endBackground);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene exists in the sceneBackgroundMusic dictionary
        if (sceneBackgroundMusic.ContainsKey(scene.name))
        {
            musicSource.clip = sceneBackgroundMusic[scene.name];
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == jump)
        {
            SFXSource.volume = 0.1f;
        }
        else if (clip == death)
        {
            SFXSource.volume = 0.2f;
        }
        SFXSource.PlayOneShot(clip);
    }

    public void PlayMovementSound(AudioClip clip)
    {
        footstepsSource.PlayOneShot(clip);
    }

    /*void Update()
    {
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && !Input.GetMouseButton(1))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                footstepsSource.clip = running;
                footstepsSource.enabled = true;
                if (!footstepsSource.isPlaying)
                {
                    footstepsSource.Play();
                }
                Debug.Log("RUNNNNNNNNNNNNN");
            }
            else
            {
                footstepsSource.clip = walking;
                footstepsSource.enabled = true;
                if (!footstepsSource.isPlaying)
                {
                    footstepsSource.Play();
                }
                Debug.Log("WAAGADF");
            }
        }
        else
        {
            footstepsSource.Stop(); // Stop the playback of the audio clip
            footstepsSource.enabled = false;
        }
    }
    */
}