using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;


    [Header("General Audio Source")]
    public AudioSource generalAudioSource;
    public List<AudioClip> hitClips;


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
