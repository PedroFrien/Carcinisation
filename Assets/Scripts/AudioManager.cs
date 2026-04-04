using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    public string name;

    [Range(0f, 5f)]
    public float volume = 1f;
    [Range(.1f, 3f)]
    public float pitch = 1f;

    [Range(0.1f, 3f)]
    public float speed = 1f;

    [HideInInspector]
    public AudioSource source;

    [HideInInspector]
    public HashSet<GameObject> playingObjects = new HashSet<GameObject>();
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] private bool effectsMute;
    [SerializeField] private bool musicMute;

    [Range(0f, 1f)]
    [SerializeField] private float effectsVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 1f;

    public Sound[] sounds, music;

    public static AudioManager instance;
    private Sound currentMusic;

    [SerializeField] private Slider effectsSlider;
    [SerializeField] private Slider musicSlider;

    [SerializeField] private LayerMask environmentLayer;

    void Awake()
    {
        //if (instance == null)
        //    instance = this;
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        //DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * effectsVolume * (effectsMute ? 0 : 1);
            s.source.pitch = s.pitch;
        }

        foreach (Sound m in music)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;
            m.source.volume = m.volume * musicVolume * (musicMute ? 0 : 1);
            m.source.loop = true;
        }
    }

    // Update effects volume for all sound effects
    public void UpdateEffectsVolume()
    {
        effectsVolume = Mathf.Clamp(effectsSlider.value, 0, musicSlider.maxValue);
        foreach (Sound s in sounds)
        {
            if (s.source != null)
            {
                s.source.volume = s.volume * effectsVolume * (effectsMute ? 0 : 1);
            }
        }
    }

    // Update music volume for all music tracks
    public void UpdateMusicVolume()
    {
        musicVolume = Mathf.Clamp(musicSlider.value, 0, musicSlider.maxValue);
        foreach (Sound m in music)
        {
            if (m.source != null)
            {
                m.source.volume = m.volume * musicVolume * (musicMute ? 0 : 1);
            }
        }
    }

    // Toggle mute for effects
    //public void ToggleEffectsMute(bool mute)
    //{
    //    effectsMute = mute;
    //    UpdateEffectsVolume(effectsVolume); // This will apply the mute state
    //}

    //// Toggle mute for music
    //public void ToggleMusicMute(bool mute)
    //{
    //    musicMute = mute;
    //    UpdateMusicVolume(musicVolume); // This will apply the mute state
    //}

    public void PlaySound(string name, Vector3 position, GameObject playingObject)
    {
        if (effectsMute) return;
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null) return;

        if (s.playingObjects.Contains(playingObject))
        {
            s.source.Stop();
            s.playingObjects.Remove(playingObject);
        }

        s.source.transform.position = position;
        s.source.pitch = s.speed;
        s.source.volume = s.volume * effectsVolume; // Apply current effects volume
        s.source.Play();
        s.playingObjects.Add(playingObject);

        StartCoroutine(ResetPlayingState(s, playingObject));
    }

    public void PlayGlobalSound(string name)
    {
        if (effectsMute) return;
        Sound s = Array.Find(sounds, sound => sound.name == name);

        s.source.transform.position = transform.position;
        s.source.pitch = s.speed;
        s.source.volume = s.volume * effectsVolume; // Apply current effects volume
        s.source.Play();
    }

    public void PlayExclusiveSound(string name, Vector3 position, GameObject playingObject)
    {
        if (effectsMute) return;
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s.playingObjects.Contains(playingObject))
        {
            return;
        }

        s.source.transform.position = position;
        s.source.pitch = s.speed;
        s.source.volume = s.volume * effectsVolume; // Apply current effects volume
        s.source.Play();
        s.playingObjects.Add(playingObject);

        StartCoroutine(ResetPlayingState(s, playingObject));
    }

    private IEnumerator ResetPlayingState(Sound s, GameObject playingObject)
    {
        yield return new WaitForSeconds(s.clip.length / s.speed);
        s.playingObjects.Remove(playingObject);
    }

    public void PlayBackgroundMusic(string name)
    {
        StopBackgroundMusic();


        if (musicMute) return;
        Sound m = Array.Find(music, track => track.name == name);
        if (m == null) return;

        if (currentMusic != null)
        {
            currentMusic.source.Stop();
        }

        m.source.volume = m.volume * musicVolume; // Apply current music volume
        m.source.Play();
        currentMusic = m;
    }

    public void StopBackgroundMusic()
    {
        if (currentMusic != null)
        {
            currentMusic.source.Stop();
            currentMusic = null;
        }
    }

    public void ToggleBackgroundMusic()
    {
        if (currentMusic != null)
        {
            StopBackgroundMusic();
        }
        else if (music.Length > 0)
        {
            PlayBackgroundMusic(music[0].name);
        }
    }



    #region World Audio

    //public void PlayWorldSound(string name, Vector3 position, GameObject playingObject, float radius)
    //{

    //    PlaySound(name, position, playingObject);

        
    //    Vector3 worldPos = position;
    //    RaycastHit hit;

    //    if (Physics.Raycast(position, Vector3.down, out hit, Mathf.Infinity, environmentLayer))
    //    {
    //        worldPos = hit.point;
    //    }


    //    Collider[] colliders = Physics.OverlapSphere(worldPos, radius);

    //    foreach (Collider collider in colliders)
    //    {

    //        BaseEnemy enemy = collider.GetComponent<BaseEnemy>();

    //        if (enemy != null)
    //        {
    //            enemy.Investigate(worldPos);
    //        }
    //    }
    //}


    #endregion
}