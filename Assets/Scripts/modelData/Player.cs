using System;
using System.Collections.Generic;
using UnityEngine;

namespace Model{
public class Player
{
    public bool isHuman;
    public string name;
    public Token token;
    public int cash;
    public int getOutOfJailCardsNo;
    public Color color;
    public List<Space.Purchasable> owned_spaces;

    public Player(string name,Token token,bool isHuman,Color color,int start_money = 1500)
    {
        this.name = name;
        this.token = token;
        this.isHuman = isHuman;
        this.cash = start_money;
        this.color = color;
        this.getOutOfJailCardsNo = 0;
        this.owned_spaces = new List<Space.Purchasable>();
    }


    public void ReceiveCash(int cash)
    {
        this.cash += cash;
    }
}
}