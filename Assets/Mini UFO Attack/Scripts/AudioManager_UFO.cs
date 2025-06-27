using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum AudioType_UFO
{

    shoot,
    die,
    gameover,
    shooterShootEnemy,
    LaserShootEnemy,
    MissileShootEnemy,
    hitEnemy,
    shoothitPlayer,
    LaserhitPlayer,
    MissilehitPlayer,
    DestroyEnemyShooter,
    DestroyEnemyLaser,
    DestroyEnemyMissile,
    powerup,
    start,
    win,
}

public enum AudioSourceType_UFO
{
    player,

    game
    
}
public class AudioManager_UFO : MonoBehaviour
{
    public static AudioManager_UFO Instance { get; private set; }

    [Header("Sources fixes")]
    public AudioSource playerSound;
    public AudioSource gameSound;
    public float vol = 1f;    
    [Header("Source dynamique")]
    public int maxSources = 32;             // ← # de sons « ennemi » qu’on accepte
    private AudioSource[] enemieSource;             // tableau d’AudioSource
    private int next = 0;                   // index circulaire

    [System.Serializable]
    public struct AudioData_UFO {
        public AudioClip clip;
        public AudioType_UFO type;
    }
    
    public AudioData_UFO[] audioClips;     

    void Awake ()
    {
        Instance = this;

        enemieSource = new AudioSource[maxSources];

        for (int i = 0; i < maxSources; i++)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake  = false;
            src.spatialBlend = 0f;
            enemieSource[i] = src;
        }

        playerSound.spatialBlend = 0f;
        gameSound.spatialBlend   = 0f;
    }

    public void PlayUISound(AudioType_UFO type, AudioSourceType_UFO chan = AudioSourceType_UFO.game)
    {
        AudioClip clip = getClip(type);
        if (!clip) return;

        var src = (chan == AudioSourceType_UFO.player) ? playerSound : gameSound;
        src.PlayOneShot(clip);              // superpose sans couper
    }

    public void PlayEnemy(AudioType_UFO type)
    {
        AudioClip clip = getClip(type);
        if (!clip) return;

        AudioSource src = enemieSource[next];
        next = (next + 1) % enemieSource.Length;    // on avance (ceci remplace la Queue)

        src.Stop();                         // si la voix rejoue trop vite
        src.clip   = clip;
        src.volume = vol;
        src.Play();
    }

    AudioClip getClip(AudioType_UFO type)
    {
        foreach (var a in audioClips) if (a.type == type) return a.clip;
        Debug.LogWarning($"Pas de clip pour {type}");
        return null;
    }
}
