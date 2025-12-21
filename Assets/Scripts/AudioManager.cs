using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;


    [Header("General Audio Source")]
    public AudioSource generalAudioSource;
    public List<AudioClip> hitClips;

    public AudioClip backgroundMusicClip;

    public AudioClip buttonClickClip;

    public List<AudioClip> hororSoundClips;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayBackgroundMusic()
    {
        if (generalAudioSource != null && backgroundMusicClip != null)
        {
            generalAudioSource.clip = backgroundMusicClip;
            generalAudioSource.loop = true;
            generalAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Background music source or clip is null!");
        }
    }

    public void PlayButtonClickSound()
    {
        if (generalAudioSource != null && buttonClickClip != null)
        {
            generalAudioSource.PlayOneShot(buttonClickClip);
        }
        else
        {
            Debug.LogWarning("Button click source or clip is null!");
        }
    }

    public void PlayRandomHororSound()
    {
        if (generalAudioSource == null || hororSoundClips.Count == 0)
        {
            Debug.LogWarning("No horror sound sources or clips available!");
            return;
        }

        int clipIndex = Random.Range(0, hororSoundClips.Count);

        AudioClip clip = hororSoundClips[clipIndex];

        if (generalAudioSource != null && clip != null)
        {
            Debug.Log($"Playing random horror sound: {clip.name}");
            generalAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Selected audio source or clip is null!");
        }
    }

    public void PlayRandomHitSound()
    {
        if (generalAudioSource == null || hitClips.Count == 0)
        {
            Debug.LogWarning("No hit sources or clips available!");
            return;
        }

        int clipIndex = Random.Range(0, hitClips.Count);

        AudioClip clip = hitClips[clipIndex];

        if (generalAudioSource != null && clip != null)
        {
            Debug.Log($"Playing random hit sound: {clip.name}");
            generalAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Selected audio source or clip is null!");
        }
    }


}
