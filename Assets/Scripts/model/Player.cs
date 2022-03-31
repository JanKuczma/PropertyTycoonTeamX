using System;
using System.Collections.Generic;
using UnityEngine;

namespace Model{
public enum Decision_outcome { SUCCESSFUL, NOT_ENOUGH_MONEY, NOT_ENOUGH_ASSETS,
                                NOT_ALL_PROPERTIES_IN_GROUP,DIFFERENCE_IN_HOUSES,NO_HOUSES,MAX_HOUSES,
                                GO_TO_JAIL, NONE }
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

    public void go_to_jail()
    {
        this.in_jail = 2;
    }
    public void ReceiveCash(int cash)
    {
        this.cash += cash;
    }

    public Decision_outcome PayCash(int amount, Player recipient = null, Board board = null)
    {
        if(totalValueOfAssets() < amount) { return Decision_outcome.NOT_ENOUGH_ASSETS; }
        if(amount > this.cash) { return Decision_outcome.NOT_ENOUGH_MONEY; }
        if(board != null) { this.cash -= amount; board.parkingFees+=amount; return Decision_outcome.SUCCESSFUL; }
        if(recipient == null) { this.cash -= amount; return Decision_outcome.SUCCESSFUL; }
        recipient.ReceiveCash(amount);
        this.cash -= amount;
        return Decision_outcome.SUCCESSFUL;
    }

    public Decision_outcome SellProperty(Space.Purchasable property, Board board)
    {
        if(property is Model.Space.Property)
        {
            if(((Space.Property)(property)).differenceInHouses(board) != 0)
            {
                return Decision_outcome.DIFFERENCE_IN_HOUSES;
            } else {
                if(property.isMortgaged)
                {
                    ReceiveCash(property.cost/2);
                    property.isMortgaged = false;
                } else { ReceiveCash(property.cost); }
                owned_spaces.Remove(property);
                property.owner = null;
                return Decision_outcome.SUCCESSFUL;
            }
        }
        else {
            if(property.isMortgaged)
            {
                ReceiveCash(property.cost/2);
                property.isMortgaged = false;
            } else { ReceiveCash(property.cost); }
            owned_spaces.Remove(property);
            property.owner = null;
            return Decision_outcome.SUCCESSFUL;
        } 
    }

    public Decision_outcome BuyProperty(Space.Purchasable property)
    {
        if(cash < property.cost)
            {
                return Decision_outcome.NOT_ENOUGH_MONEY;
            } else {
                PayCash(property.cost);
                owned_spaces.Add(property);
                property.owner = this;
                return Decision_outcome.SUCCESSFUL;
            } 
    }
    public Decision_outcome BuyProperty(Space.Purchasable property, int cost)
    {
        if(cash < cost)
            {
                return Decision_outcome.NOT_ENOUGH_MONEY;
            } else {
                PayCash(cost);
                owned_spaces.Add(property);
                property.owner = this;
                return Decision_outcome.SUCCESSFUL;
            } 
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