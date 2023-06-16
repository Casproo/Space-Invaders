using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    public SoundData[] sounds;

    void Awake ()
    {
        
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        } else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach (SoundData s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string sound)
    {
        SoundData s = Array.Find(sounds, item => item.name == sound);
        s.source.Play();
    }
    public void Stop(string sound)
    {
        SoundData s = Array.Find(sounds, item => item.name == sound);
        s.source.Stop();
    }

}