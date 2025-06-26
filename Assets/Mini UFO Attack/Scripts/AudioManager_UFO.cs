using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum AudioType_UFO
{

    shoot,
    die,
    gameover,
    destroyEnemy1,
    destroyEnemy2,
    shootEnemy1,
    shootEnemy2,
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

    [Header("Source dynamique")]
    public int maxSources = 32;             // ← # de sons « ennemi » qu’on accepte
    private AudioSource[] pool;             // tableau d’AudioSource
    private int next = 0;                   // index circulaire

    [System.Serializable]
    public struct AudioData_UFO {
        public AudioClip clip;
        public AudioType_UFO type;
    }
    public AudioData_UFO[] audioClips;      // tu continues d’utiliser ton tableau
                                            // + ta fonction getClip(type)

    void Awake ()
    {
        Instance = this;

        /* ----- on fabrique le tableau de sources dynamiques ----- */
        pool = new AudioSource[maxSources];

        for (int i = 0; i < maxSources; i++)
        {
            var src = gameObject.AddComponent<AudioSource>();  // crée le composant
            src.playOnAwake  = false;
            src.spatialBlend = 0f;   // 100 % 2D :contentReference[oaicite:0]{index=0}
            pool[i] = src;
        }

        /* ----- sources fixes en 2D aussi ----- */
        playerSound.spatialBlend = 0f;
        gameSound.spatialBlend   = 0f;
    }

    /* ---------- API UI / musique ---------- */
    public void PlayUISound(AudioType_UFO type, AudioSourceType_UFO chan = AudioSourceType_UFO.game)
    {
        AudioClip clip = getClip(type);
        if (!clip) return;

        var src = (chan == AudioSourceType_UFO.player) ? playerSound : gameSound;
        src.PlayOneShot(clip);              // superpose sans couper
    }

    /* ---------- API « ennemi » 2D ---------- */
    public void PlayEnemy(AudioType_UFO type, float vol = 1f)
    {
        AudioClip clip = getClip(type);
        if (!clip) return;

        AudioSource src = pool[next];
        next = (next + 1) % pool.Length;    // on avance (ceci remplace la Queue)

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
