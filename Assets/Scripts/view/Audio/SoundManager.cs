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
    public static float musicVolume = .5f;
    public static float sfxVolume = .7f;
    public bool starWarsTheme;
    private void Awake()
    {
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

    private void OnLoadCallBack(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.buildIndex);
        if (scene.buildIndex > 2)
        {
            starWarsTheme = GameObject.Find("GameData").GetComponent<GameData>().starWarsTheme;
            PlayGameMusic();
        }
        else
        {
            PlayMenuMusic();
        }
    }

    void Start()
    {
        musicMixerGroup.audioMixer.SetFloat("Music Volume", Mathf.Log10(musicVolume) * 20);
        soundMixerGroup.audioMixer.SetFloat("SFX Volume", Mathf.Log10(sfxVolume) * 20);
        SceneManager.sceneLoaded += this.OnLoadCallBack;
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
        s.source.Play();
    }

    public void PlayAndStopOthers(string clipname)
    {
        Sound s = Array.Find(sounds, dummySound => dummySound.clipName == clipname);
        if (s == null)
        {
            Debug.LogError("Sound: " + clipname + " does not exist!");
            return;
        }

        StopAllTracks();

        s.source.Play();
    }

    private void StopAllTracks()
    {
        foreach (Sound song in sounds)
        {
            if (song.source.isPlaying)
            {
                song.source.Stop();
            }
        }
    }

    public void PlayClickSound()
    {
        Play("Click");
    }

    public void PlayDiceSound(int i)
    {
        //if (!checkForDiceSound())
        //{
        sounds[8+i].source.PlayOneShot(sounds[8+i].audioClip);
       // }
    }

    public bool checkForDiceSound()
    {
        bool soundPlaying = false;
            foreach (Sound s in sounds.Skip(9).Take(7))
            {
                if (s.source.isPlaying)
                {
                    return true;
                }
            }
            return soundPlaying;
    }

    //this method needs to be refactored so that it is called outside of controller's FixedUpdate
    public void checkPlayerStatus(Player player)
    {
        if (player.position == 31)
        {
            if (!sounds[4].source.isPlaying)
            {
                PlayAndStopOthers("Jail");    
            }
        } else if (player.in_jail == 0)
        {
            if (!starWarsTheme)
            {
                if (!sounds[1].source.isPlaying && !sounds[2].source.isPlaying && !sounds[3].source.isPlaying)
                {
                    Debug.Log("playing classic music");
                    PlayGameMusic();
                }
            }
            else
            {
                if (!sounds[17].source.isPlaying && !sounds[18].source.isPlaying)
                {
                    Debug.Log("playing star wars music");
                    PlayGameMusic();
                }
            }
        }
        if (player.in_jail > 0 && !sounds[4].source.isPlaying)
        {
            if (!sounds[4].source.isPlaying)
            {
                PlayAndStopOthers("Jail");    
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

        if (!starWarsTheme)
        {
            Random r = new Random();
            int rand = r.Next(1, 4);
            PlayAndStopOthers(sounds[rand].clipName);
        }
        else
        {
            Random r = new Random();
            int rand = r.Next(17, 19);
            PlayAndStopOthers(sounds[rand].clipName);    
        }
    }

    private void PlayMenuMusic()
    {
        if (!sounds[0].source.isPlaying)
        {
            PlayAndStopOthers("Menu");    
        }
        
    }

    public void PlayPurchaseSound()
    {
        sounds[18].source.PlayOneShot(sounds[18].audioClip);
    }

    public void PlayIncomeSound()
    {
        sounds[19].source.PlayOneShot(sounds[19].audioClip);
    }
}

