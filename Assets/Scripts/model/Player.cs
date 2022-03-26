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
    public bool allowed_to_buy;
    public int in_jail;
    public List<Space.Purchasable> owned_spaces;

    public Player(string name,Token token,bool isHuman,Color color,int start_money = 1500)
    {
        this.name = name;
        this.token = token;
        this.isHuman = isHuman;
        this.cash = start_money;
        this.color = color;
        this.getOutOfJailCardsNo = 0;
        this.allowed_to_buy = false;
        this.in_jail = 0;
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

    public int totalValueOfAssets()
    {
        int total = 0;
        foreach(Space.Purchasable space in owned_spaces)
        {
            if(space is Space.Property)
            {
                if(((Space.Property)(space)).noOfHouses == 5)
                {
                    total += ((Space.Property)(space)).hotel_cost + 4*((Space.Property)(space)).house_cost;
                } else {
                    total += ((Space.Property)(space)).noOfHouses*((Space.Property)(space)).house_cost;
                }
            }
            if(space.isMortgaged)
            {
                total += space.cost/2;
            } else {
                total += space.cost;
            }
        }
        total += this.cash;
        return total;
    }
}
}