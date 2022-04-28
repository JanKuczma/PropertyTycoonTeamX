using UnityEngine;


namespace view.Sound
{
        
[System.Serializable]
    public class Sound
    {
        // enums to specify whether each track belongs in the SFX or Music AudioMixerGroup
        public enum AudioTypes { SFX, MUSIC }
        public AudioTypes audioType;
        
        
        public AudioSource source;
        // clip name is used to identify each track (e.g. used in Play(string clipname))
        public string clipName;
        // mp3/wav files are inserted into this attribute within the Unity inspector
        public AudioClip audioClip;
        // defines whether each track should loop when playing (e.g. menu music)
        public bool isLoop;
        // defines whether each track should play on awake (e.g. menu music)
        public bool playOnAwake;

        [Range(0, 1)] public float volume = 0.5f;
    }
}