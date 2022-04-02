using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPopUp : View.PopUp
{
    public Slider sfx;
    public Slider music;
    public SoundManager soundManager;

    public static OptionsPopUp Create(Transform parent)
    {
        OptionsPopUp popUp = Instantiate(Asset.OptionsPopUpPreFab, parent).GetComponent<OptionsPopUp>();
        popUp.btn1.onClick.AddListener(() => popUp.closePopup());
        popUp.soundManager = GameObject.FindGameObjectWithTag("GameMusic").GetComponent<SoundManager>();
        return popUp;
    }

    public void ChangeMusicVolume(float value)
    {
        SoundManager.musicVolume = value;
        Debug.Log("vol: " + SoundManager.musicVolume);
        soundManager.musicMixerGroup.audioMixer.SetFloat("Music Volume", Mathf.Log10(value) * 20);
    }
    
    public void ChangeSFXVolume(float value)
    {
        SoundManager.sfxVolume = value;
        Debug.Log("sfx: " + SoundManager.sfxVolume);
        soundManager.soundMixerGroup.audioMixer.SetFloat("SFX Volume", Mathf.Log10(value) * 20);
    }
}
