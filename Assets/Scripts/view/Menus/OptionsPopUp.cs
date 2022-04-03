using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsPopUp : View.PopUp
{
    public Slider sfx;
    public Slider music;
    public Slider tokenSpeed;
    public SoundManager soundManager;

    public static OptionsPopUp Create(Transform parent)
    {
        OptionsPopUp popUp = Instantiate(Asset.OptionsPopUpPreFab, parent).GetComponent<OptionsPopUp>();
        popUp.btn1.onClick.AddListener(() => popUp.closePopup());
        popUp.soundManager = GameObject.FindGameObjectWithTag("GameMusic").GetComponent<SoundManager>();
        return popUp;
    }

    public void Start()
    {
        sfx.value = SoundManager.sfxVolume;
        music.value = SoundManager.musicVolume;
        tokenSpeed.value = View.Piece.SPEED;
    }

    public void ChangeMusicVolume(float value)
    {
        SoundManager.musicVolume = value;
        soundManager.musicMixerGroup.audioMixer.SetFloat("Music Volume", Mathf.Log10(value) * 20);
    }
    
    public void ChangeSFXVolume(float value)
    {
        SoundManager.sfxVolume = value;
        soundManager.soundMixerGroup.audioMixer.SetFloat("SFX Volume", Mathf.Log10(value) * 20);
    }

    public void ChangeTokenSpeed(float value)
    {
        View.Piece.SPEED = value;
    }
}
