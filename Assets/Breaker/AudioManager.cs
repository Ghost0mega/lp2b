using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum AudioType
{

    bounce,
    die,
    gameover,
    destroy,
    start,
    win,
    bouncewall
}

public enum AudioSourceType
{
    player,
    game
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public float volume = 1.0f;
    public AudioSource playerSound;
    public AudioSource gameSound;

    [System.Serializable]
    public struct AudioData
    {
        public AudioClip clip;
        public AudioType type;
    }
    public AudioData[] audioClips;
    void Start()
    {
        gameSound.volume = volume;
        playerSound.volume = volume;
    }
    private void Awake()
    {
        Instance = this;
    }

    public void PlaySound(AudioType type, AudioSourceType sourceType)
    {

        AudioClip clip = getClip(type);

        if (!clip)
        {
            Debug.LogError($"AudioManager : aucun clip assigné pour {type}");
            return;
        }

        if (sourceType == AudioSourceType.player)
        {
            playerSound.PlayOneShot(clip);
        }
        else if (sourceType == AudioSourceType.game)
        {
            gameSound.PlayOneShot(clip);
        }
    }

    AudioClip getClip(AudioType type)
    {
        foreach (var audioData in audioClips)
        {
            if (audioData.type == type)
            {
                return audioData.clip;
            }
        }
        return null;
    }
}
