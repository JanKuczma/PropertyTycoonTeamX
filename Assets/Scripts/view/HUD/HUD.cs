
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
public class HUD : MonoBehaviour
{
    Dictionary<Model.Player,PlayerTab> player_tabs = new Dictionary<Model.Player,PlayerTab>();
    Dictionary<int,PurchasableCard> propertyCards = new Dictionary<int, PurchasableCard>();
    public Button FinishTurnButton;
    // current PopUp, in future can be changed to queue/stack of PopUps
    public OptionPopUp currentPopUp = null;
    public PropertyManager currentManager = null;
    public Image jail_bars;

    public void Create_player_tabs(List<Model.Player> players,Model.Board board)
    {
        foreach(Model.Space space in board.spaces)
        {
            switch(space.type)
            {
                case SqType.PROPERTY:
                propertyCards.Add(space.position, PropertyCard.Create((Model.Space.Property)space,FindObjectOfType<Canvas>().transform));
                break;
                case SqType.UTILITY:
                propertyCards.Add(space.position, UtilityCard.Create((Model.Space.Utility)space,FindObjectOfType<Canvas>().transform));
                break;
                case SqType.STATION:
                propertyCards.Add(space.position, StationCard.Create((Model.Space.Station)space,FindObjectOfType<Canvas>().transform));
                break;
            }
        }
        float interval = (-140*GetComponentInParent<RectTransform>().sizeDelta.x/1920)*(players.Count-1);
        for(int i = 0; i < players.Count; i++)
        {
            player_tabs.Add(players[i],PlayerTab.Create(transform,players[i],propertyCards));
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
            StartCoroutine(tab.hide(GetComponentInParent<RectTransform>().sizeDelta.y));
        }
    }

    public void set_current_player_tab(Model.Player player)
    {
        hide_tabs();
        StartCoroutine(player_tabs[player].halfPopUp(GetComponentInParent<RectTransform>().sizeDelta.y));
    }

    public void UpdatePlayersTabInfo()
    {
        foreach(KeyValuePair<Model.Player,View.PlayerTab> entry in player_tabs)
        {
            entry.Value.setMoney(entry.Key.cash);
        }
    }
}
}
