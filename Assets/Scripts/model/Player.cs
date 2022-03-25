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

    public bool PayCash(int amount, Player player = null)
    {
        if(amount > this.cash) { return false; }
        if(player == null) { this.cash -= amount; return true; }
        player.ReceiveCash(amount);
        this.cash -= amount;
        return true;
    }

    public bool SellProperty(Space.Purchasable property)
    {
        //if property.isMortgaged then: no pasa nada, return false;
        owned_spaces.Remove(property);
        ReceiveCash(property.cost);
        return true; 
    }

    public bool BuyProperty(Space.Purchasable property)
    {
        //if property.cost > this.cash: no pasa nada, return false;
        owned_spaces.Add(property);
        PayCash(property.cost);
        return true; 
    }
}
}