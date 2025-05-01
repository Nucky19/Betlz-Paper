using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMManager : MonoBehaviour
{
    
    public AudioClip backgroundMusic;
    public float volume = 0.5f;
    public bool loop = true;
    public bool persistBetweenScenes = true;

    private AudioSource audioSource;

    void Awake()
    {
        // Asegurar que solo haya una instancia si se persiste entre escenas
        if (persistBetweenScenes)
        {
            if (FindObjectsOfType<BMManager>().Length > 1)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.playOnAwake = false;
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public void PauseMusic()
    {
        if (audioSource.isPlaying)
            audioSource.Pause();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        audioSource.volume = volume;
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }
}


