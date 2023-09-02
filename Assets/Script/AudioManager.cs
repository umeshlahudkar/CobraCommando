using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    [SerializeField] private AudioClip backgroundAudioClip;
    [SerializeField] private AudioClip fireAudioClip;
    [SerializeField] private AudioClip buttonClickAudioClip;

    private void Awake()
    {
        backgroundAudioSource.clip = backgroundAudioClip;
        backgroundAudioSource.loop = true;

        backgroundAudioSource.Play();
    }

    public void PlayFireSound()
    {
        sfxAudioSource.PlayOneShot(fireAudioClip);
    }

    public void PlayButtonClickSound()
    {
        //sfxAudioSource.PlayOneShot(buttonClickAudioClip);
    }
}
