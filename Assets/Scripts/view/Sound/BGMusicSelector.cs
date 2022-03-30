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

    // Start is called before the first frame update
    void Start()
    {
        TrackSelector = 0;
        menu_music.Play();
        menu_music.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            menu_music.Stop();
            if (!game_music1.isPlaying && !game_music2.isPlaying && !game_music3.isPlaying)
            {
                TrackSelector = Random.Range(0, 3);

                if (TrackSelector == 0 && TrackHistory != 1)
                {
                    game_music1.Play();
                    TrackHistory = 1;
                }
                else if (TrackSelector == 1 && TrackHistory != 2)
                {
                    game_music2.Play();
                    TrackHistory = 2;
                }
                else if (TrackSelector == 2 && TrackHistory != 3)
                {
                    game_music3.Play();
                    TrackHistory = 3;
                }
            }
        }
    }
}
