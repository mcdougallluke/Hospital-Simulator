using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----- Audio Source -----")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("----- Audio Clip -----")]
    public AudioClip background;
    public AudioClip walking;
    public AudioClip jump;
    public AudioClip pickupItem;

    public void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
}
