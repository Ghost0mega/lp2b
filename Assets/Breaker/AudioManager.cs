using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum AudioType_BrickBreaker
{
    // Brick breaker
    bounce,
    die,
    gameover,
    destroy,
    start,
    win,
    bouncewall,


}

public enum AudioSourceType_BrickBreaker
{
    player,
    game
}
public class AudioManager_BrickBreaker : MonoBehaviour
{
    public static AudioManager_BrickBreaker Instance { get; private set; }
    public float volume = .1f;
    public AudioSource playerSound;
    public AudioSource gameSound;

    [System.Serializable]
    public struct AudioData_BrickBreaker
    {
        public AudioClip clip;
        public AudioType_BrickBreaker type;
    }
    public AudioData_BrickBreaker[] audioClips;
    void Start()
    {
        gameSound.volume = volume;
        playerSound.volume = volume;
    }
    private void Awake()
    {
        Instance = this;
    }

    public void PlaySound(AudioType_BrickBreaker type, AudioSourceType_BrickBreaker sourceType)
    {

        AudioClip clip = getClip(type);

        if (!clip)
        {
            Debug.LogError($"AudioManager : aucun clip assigné pour {type}");
            return;
        }

        if (sourceType == AudioSourceType_BrickBreaker.player)
        {
            playerSound.PlayOneShot(clip);
        }
        else if (sourceType == AudioSourceType_BrickBreaker.game)
        {
            gameSound.PlayOneShot(clip);
        }
    }

    AudioClip getClip(AudioType_BrickBreaker type)
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
