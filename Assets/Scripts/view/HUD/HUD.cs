using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;

public class HUD : MonoBehaviour
{
    Dictionary<Model.Player,PlayerTab> player_tabs = new Dictionary<Model.Player,PlayerTab>();

    public void Create_player_tabs(List<Model.Player> players)
    {
        float interval = (-140*GetComponentInParent<RectTransform>().sizeDelta.x/1920)*(players.Count-1);
        for(int i = 0; i < players.Count; i++)
        {
            player_tabs.Add(players[i],PlayerTab.Create(transform,players[i].name,players[i].token,players[i].color));
            player_tabs[players[i]].GetComponent<RectTransform>().anchoredPosition = new Vector2(interval,-650*GetComponentInParent<RectTransform>().sizeDelta.x/1920);
            interval += (280*GetComponentInParent<RectTransform>().sizeDelta.x/1920);
        }
    }

    public void sort_tabs(List<Model.Player> players)
    {
        float interval = (-140*GetComponentInParent<RectTransform>().sizeDelta.x/1920)*(players.Count-1);
        for(int i = 0; i < players.Count; i++)
        {
            player_tabs[players[i]].GetComponent<RectTransform>().anchoredPosition = new Vector2(interval,player_tabs[players[i]].GetComponent<RectTransform>().anchoredPosition.y);
            interval += (280*GetComponentInParent<RectTransform>().sizeDelta.x/1920);
        }
    }

    public void hide_tabs()
    {
        foreach(PlayerTab tab in player_tabs.Values)
        {
            tab.hide(GetComponentInParent<RectTransform>().sizeDelta.y);
        }
    }

    public void set_current_player_tab(Model.Player player)
    {
        hide_tabs();
        player_tabs[player].halfPopUp(GetComponentInParent<RectTransform>().sizeDelta.y);
    }

}
