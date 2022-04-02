using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOptionsManager : MonoBehaviour
{
    public static float musicVolume { get; private set; }
    public static float sfxVolume { get; private set; }

    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;
        
        SoundManager.Instance.UpdateMixerVolume();
    }
    
    public void OnSFXSliderValueChange(float value)
    {
        sfxVolume = value;
        
        SoundManager.Instance.UpdateMixerVolume();
    }
}
