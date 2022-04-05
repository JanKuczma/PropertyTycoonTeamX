using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;
using UnityEngine.UI;
using view.Sound;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Random = System.Random;

[System.Serializable]
public class SoundManager : MonoBehaviour
{

    [SerializeField] public AudioMixerGroup musicMixerGroup;
    [SerializeField] public AudioMixerGroup soundMixerGroup;
    [SerializeField] public Sound[] sounds;
    private string CurrentMusicID;
    public static float musicVolume = .5f;
    public static float sfxVolume = .7f;
    public Button[] currentButtons;
    public Dictionary<Button,bool> clickedButtons;

    private void Awake()
    {
        AddButtonClicks();
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

    void Start()
    {
        musicMixerGroup.audioMixer.SetFloat("Music Volume", Mathf.Log10(musicVolume) * 20);
        soundMixerGroup.audioMixer.SetFloat("SFX Volume", Mathf.Log10(sfxVolume) * 20);
    }

    public void AddButtonClicks()
    {
        foreach (Button button in FindObjectsOfType<Button>())
        {
            Debug.Log(button.name);
            //currentButtons.Push(button);
            Debug.Log("current Buttons Count:" + currentButtons.Length);
        }
        // Debug.Log("finished finding buttons");
        //
        // foreach (var cButton in clickedButtons)
        // {
        //     Debug.Log("Adding Clicks");
        //     if (!cButton.Value)
        //     {
        //         Debug.Log("Click Added to" + cButton.Key.name);
        //         cButton.Key.onClick.AddListener(() => Play("Click"));
        //         cButton.Value.Equals(true);
        //     }
        // }
        Debug.Log("Finished Adding Clicks");
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex < 3)
        {
            if (!CheckForPlayingSong())
            {
                Play("Menu");
            }
        }
        else
        {
            sounds[0].source.Stop();
            if (!CheckForPlayingSong())
            {
                PlayGameMusic();
            }
        }
    }

    public bool CheckForPlayingSong()
    {
        bool songIsPlaying = false;
        while (!songIsPlaying)
        {
            foreach (Sound s in sounds)
            {
                if (s.source.isPlaying)
                {
                    songIsPlaying = true;
                }
            }
            break;
        }

        return songIsPlaying;
    }

    public void Play(string clipname)
    {
        Sound s = Array.Find(sounds, dummySound => dummySound.clipName == clipname);
        if (s == null)
        {
            Debug.LogError("Sound: " + clipname + " does not exist!");
            return;
        }

        foreach (Sound song in sounds)
        {
            if (song.source.isPlaying)
            {
                song.source.Stop();
            }
        }

        s.source.Play();
    }

    public void checkPlayerStatus(Player player)
    {
        if (player.position == 31)
        {
            if (!sounds[4].source.isPlaying)
            {
                Play("Jail");    
            }
        } else if (player.in_jail == 0 && !sounds[1].source.isPlaying && !sounds[2].source.isPlaying && !sounds[3].source.isPlaying)
        {
            PlayGameMusic();
        }
        if (player.in_jail > 0 && !sounds[4].source.isPlaying)
        {
            if (!sounds[4].source.isPlaying)
            {
                Play("Jail");    
            }
        }
        //
        // if (player.hasnomoney)
        // {
        //     Play("Bankrupt");
        // }
        
    }

    private void PlayGameMusic()
    {
        Random r = new Random();
        int rand = r.Next(1, 4);
        Play(sounds[rand].clipName);
    }
}

