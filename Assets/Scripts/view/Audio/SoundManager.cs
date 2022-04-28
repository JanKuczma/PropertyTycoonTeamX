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
    // MixerGroup GameObjects for game music and SFX to separate the volume sliders
    [SerializeField] public AudioMixerGroup musicMixerGroup;
    [SerializeField] public AudioMixerGroup soundMixerGroup;
    // All sound files used in game stored as Sound objects
    [SerializeField] public Sound[] sounds;
    // Initial volume values for music and SFX (updated upon slider value change)
    public static float musicVolume = .5f;
    public static float sfxVolume = .7f;
    // Boolean set to True if user selects Star Wars theme
    public bool starWarsTheme;
    private void Awake()
    {
        // On awake add each Sound object into the game as an AudioSource
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.loop = s.isLoop;
            s.source.volume = 0.5f;
            // Split music/SFX audio sources into respective MixerGroups
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
    
    // Method is called whenever a new game scene loads, this is to change from menu music once the game starts
    private void OnLoadCallBack(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex > 2)
        {
            starWarsTheme = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>().starWarsTheme;
            PlayGameMusic();
        }
        else
        {
            PlayMenuMusic();
        }
    }

    void Start()
    {
        // Get volume values for each MixerGroup and assign decibel scaling
        musicMixerGroup.audioMixer.SetFloat("Music Volume", Mathf.Log10(musicVolume) * 20);
        soundMixerGroup.audioMixer.SetFloat("SFX Volume", Mathf.Log10(sfxVolume) * 20);
        SceneManager.sceneLoaded += this.OnLoadCallBack;
    }

    
    /// <summary>
    /// Finds desired song and plays it
    /// </summary>
    /// <param name="clipname">Clipname of the desired <paramref name="Sound"/></param>
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

    /// <summary>
    /// Stops all other songs that are playing and plays new song
    /// </summary>
    /// <param name="clipname">Clipname of the desired <paramref name="Sound"/></param>
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

    /// <summary>
    /// Checks each AudioSource, and stops playing if it is currently playing
    /// </summary>
    /// <param name="clipname">Clipname of the desired <paramref name="Sound"/></param>
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

    /// <summary>
    /// Applied to all Buttons on click
    /// </summary>
    public void PlayClickSound()
    {
        Play("Click");
    }
    
    /// <summary>
    /// Plays dice roll sound based on trajectory of collision. Dice sounds are stored in order of ascending clip length for realistic matching
    /// </summary>
    /// <param name="i">Determined by velocity of the dice upon collision></param>
    public void PlayDiceSound(int i)
    {
        sounds[8+i].source.PlayOneShot(sounds[8+i].audioClip);
    }

    /// <summary>
    /// Checks status of the active Player and plays relevant music. Called through FixedUpdate() in controller.
    /// </summary>
    /// <param name="i">Determined by velocity of the dice upon collision></param>
    public void checkPlayerStatus(Player player)
    {
        // if player is on Go To Jail Square
        if (player.position == 31)
        {
            // if Jail music not already playing, play it
            if (!sounds[4].source.isPlaying)
            {
                PlayAndStopOthers("Jail");    
            }
            // if player is on any other square
        } else if (player.in_jail == 0)
        {
            if (!starWarsTheme)
            {
                // if in classic mode and none of the background tracks are playing, play one of them
                if (!sounds[1].source.isPlaying && !sounds[2].source.isPlaying && !sounds[3].source.isPlaying)
                {
                    PlayGameMusic();
                }
            }
            else
            {
                // if in star wars mode and neither of the bg tracks are playing, play one of them
                if (!sounds[17].source.isPlaying && !sounds[18].source.isPlaying)
                {
                    PlayGameMusic();
                }
            }
        }
        // If player is in jail and the jail music isn't playing, play it
        if (player.in_jail > 0 && !sounds[4].source.isPlaying)
        {
            if (!sounds[4].source.isPlaying)
            {
                PlayAndStopOthers("Jail");    
            }
        }
        
        // If player has no remaining assests, play bankrupt music
        if (player.totalValueOfAssets() < 1)
        {
             PlayAndStopOthers("Bankrupt");
        }
         
    }
    /// <summary>
    /// Plays random background game music based on game theme
    /// </summary>
    private void PlayGameMusic()
    {
        // if in classic mode, play a random background song from the 3 classic bg tracks 
        if (!starWarsTheme)
        {
            Random r = new Random();
            int rand = r.Next(1, 4);
            PlayAndStopOthers(sounds[rand].clipName);
        }
        // if in star wars mode, play a random background song from the 2 star wars bg tracks
        else
        {
            Random r = new Random();
            int rand = r.Next(16, 18);
            PlayAndStopOthers(sounds[rand].clipName);    
        }
    }
    /// <summary>
    /// Plays menu music if in menu scene and menu music isn't already playing
    /// </summary>
    private void PlayMenuMusic()
    {
        // if main menu music isn't playing, play it
        if (!sounds[0].source.isPlaying)
        {
            PlayAndStopOthers("Menu");    
        }
        
    }
    
    /// <summary>
    /// Plays cash register sound whenever active player's cash decreases
    /// </summary>
    public void PlayPurchaseSound()
    {
        sounds[18].source.PlayOneShot(sounds[18].audioClip);
    }
    
    /// <summary>
    /// Plays coin drop sound whenever active player's cash increases
    /// </summary>
    public void PlayIncomeSound()
    {
        sounds[19].source.PlayOneShot(sounds[19].audioClip);
    }
}

