using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HUD : MonoBehaviour
{
    Color[] colors = {Color.blue,Color.red,Color.green,Color.yellow,Color.cyan,Color.magenta};
    Dictionary<Model.Player,PlayerTab> player_tabs = new Dictionary<Model.Player,PlayerTab>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Create_player_tabs(List<Model.Player> players)
    {
        int interval = 1920 / (players.Count+1);
        for(int i = 0; i < players.Count; i++)
        {
            player_tabs.Add(players[i],PlayerTab.Create(transform,players[i].name,players[i].token,colors[i]));
            player_tabs[players[i]].GetComponent<RectTransform>().anchoredPosition = new Vector2((i+1)*interval-960,-550);
        }
    }

    public void sort_tabs(List<Model.Player> players)
    {
        int interval = 1920 / (players.Count+1);
        for(int i = 0; i < players.Count; i++)
        {
            player_tabs[players[i]].GetComponent<RectTransform>().anchoredPosition = new Vector2((i+1)*interval-960,-550);
        }
    }

    public void hide_tabs()
    {
        foreach(PlayerTab tab in player_tabs.Values)
        {
            tab.hide();
        }
    }

    public void set_current_player_tab(Model.Player player)
    {
        hide_tabs();
        player_tabs[player].popUp();
    }
}
