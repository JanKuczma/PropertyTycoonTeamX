using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMusicSelector : MonoBehaviour
{

    public AudioSource menu_music;
    public AudioSource game_music1;
    public AudioSource game_music2;
    public AudioSource game_music3;
    public AudioSource auction_music;
    public AudioSource podium_music;
    public AudioSource jail_music;
    public AudioSource bankrupt_music;

    public int TrackSelector;
    public int TrackHistory;

    public TurnState state = TurnState.NONE;

    // Start is called before the first frame update
    void Start()
    {
        menu_music.maxDistance = 1;
        game_music1.maxDistance = 1;
        game_music2.maxDistance = 1;
        game_music3.maxDistance = 1;
        //auction_music.maxDistance = 1;
        //podium_music.maxDistance = 1;
        jail_music.maxDistance = 1;
        //bankrupt_music.maxDistance = 1;
        TrackSelector = 0;
        FadeIn(menu_music, 3.0);
        menu_music.loop = true;
    }

    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            FadeOut(menu_music, 5.0);
            if (!game_music1.isPlaying && !game_music2.isPlaying && !game_music3.isPlaying)
            {
                TrackSelector = Random.Range(0, 3);

                if (TrackSelector == 0 && TrackHistory != 1)
                {
                    FadeIn(game_music1, 2.0);
                    TrackHistory = 1;
                }
                else if (TrackSelector == 1 && TrackHistory != 2)
                {
                    FadeIn(game_music2, 2.0);
                    TrackHistory = 2;
                }
                else if (TrackSelector == 2 && TrackHistory != 3)
                {
                    FadeIn(game_music3, 2.0);
                    TrackHistory = 3;
                }
            }
        }
    }

    public void UpdateGameState(TurnState state)
    {
        this.state = state;
    }

    public void JailMusic()
    {
        FadeOut(game_music1, 3.0);
        FadeOut(game_music2, 3.0);
        FadeOut(game_music3, 3.0);
        FadeIn(jail_music, 3.0);
    }

    public void FadeOut(AudioSource audio_track, double fadeTime)
    {
        StartCoroutine(InvokeFadeOut(audio_track, fadeTime));
    }

    private IEnumerator InvokeFadeOut(AudioSource audio_track, double fadeTime)
    {
            float startVolume = audio_track.volume;
            while (audio_track.volume > 0)
            {
                audio_track.volume -= startVolume * Time.deltaTime / (float)fadeTime;

                yield return null;
            }

            audio_track.Stop();
        }

    public void FadeIn(AudioSource audio_track, double fadeTime)
    {
        StartCoroutine(InvokeFadeIn(audio_track, fadeTime));
    }

    private IEnumerator InvokeFadeIn(AudioSource audio_track, double fadeTime)
    {
        float startVolume = (float)0.05;
        audio_track.volume = startVolume;
        audio_track.Play();
        while (audio_track.volume < audio_track.maxDistance)
        {
            audio_track.volume += startVolume * Time.deltaTime / (float)fadeTime;

            yield return null;
        }
    }
    
}
