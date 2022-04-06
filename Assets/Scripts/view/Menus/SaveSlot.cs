using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    public Text players_title;
    public Text players;
    public Text date;
    public Text empty;
    public Image raffles;

    public GameData.Wrapper data;

    void Awake()
    {
        data = GameData.Wrapper.loadGame(gameObject.name);
        if(data == null) { MakeEmpty(); }
        else { ShowInfo(); }
    }

    public void MakeEmpty()
    {
        players_title.gameObject.SetActive(false);
        players.gameObject.SetActive(false);
        date.gameObject.SetActive(false);
        raffles.gameObject.SetActive(false);
        empty.gameObject.SetActive(true);
    }

    public void ShowInfo()
    {
        players_title.gameObject.SetActive(true);
        players.gameObject.SetActive(true);
        date.gameObject.SetActive(true);
        raffles.gameObject.SetActive(true);
        empty.gameObject.SetActive(false);
        this.players.text = "";
        if(data == null) { MakeEmpty(); return; }
        foreach(Model.Player player in data.players)
        {
            this.players.text += player.name + "\n";
        }
        this.players.text = this.players.text.Substring(0,this.players.text.Length-1);
        this.date.text = data.saveDate;
        if(data.starWarsTheme) { raffles.sprite = Resources.Load<Sprite>("Kingsleys/kingsley_yoda2"); } else { raffles.sprite = Resources.Load<Sprite>("Kingsleys/kingsley monopoly man"); }
        
    }
    
}
