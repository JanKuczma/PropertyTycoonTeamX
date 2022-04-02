using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using view.Sound;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Random = System.Random;

[System.Serializable]
public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;

    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup soundMixerGroup;
    [SerializeField] Sound[] sounds;
    private string CurrentMusicID;

    public int lastSongIndex;

    private void Awake()
    {
        Instance = this;
        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.loop = s.isLoop;
            s.source.volume = 0.5f;

            switch (s.audioType)
            {
                case Sound.AudioTypes.SFX:
                    s.source.outputAudioMixerGroup = soundMixerGroup;
                    break;
                
                case Sound.AudioTypes.MUSIC:
                    s.source.outputAudioMixerGroup = musicMixerGroup;
                    break;
            }

            if (s.playOnAwake)
            {
                s.source.Play();
            }
        }
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            Stop(sounds[0].clipName);
            if (!sounds[1].source.isPlaying && !sounds[2].source.isPlaying && !sounds[3].source.isPlaying )
            {
                ChangeMusic(sounds.Skip(1).Take(3).ToArray());
            }
        }
        else
        {
            if (!sounds[0].source.isPlaying)
            {
                Play(sounds[0].clipName);
            }
        }
    }

    public void ChangeMusic(Sound[] sounds)
    {
        foreach (Sound s in sounds)
        {
            if (s.clipName == CurrentMusicID)
            {
                s.source.Stop();
                break;
            }
        }

        Random r = new Random();
        int rand = r.Next(0, sounds.Length);
        Play(sounds[rand].clipName);
    }

    public void Play(string clipname)
    {
        Sound s = Array.Find(sounds, dummySound => dummySound.clipName == clipname);
        if (s == null)
        {
            Debug.LogError("Sound: " + clipname + " does not exist!");
            return;
        }
        s.source.Play();
    }
    
    public void Stop(string clipname)
    {
        Sound s = Array.Find(sounds, dummySound => dummySound.clipName == clipname);
        if (s == null)
        {
            Debug.LogError("Sound: " + clipname + " does not exist!");
            return;
        }
        s.source.Stop();
    }

    public void UpdateMixerVolume()
    {
        musicMixerGroup.audioMixer.SetFloat("Music Volume", Mathf.Log10(SoundOptionsManager.musicVolume) * 20);
        soundMixerGroup.audioMixer.SetFloat("SFX Volume", Mathf.Log10(SoundOptionsManager.sfxVolume) * 20);
    }
}

