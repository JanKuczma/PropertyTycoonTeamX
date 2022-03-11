using System;
using System.Collections.Generic;

namespace Model{
public class Player
{
    public bool isHuman;
    public string name;
    public Token token;
    public int cash;
    public int getOutOfJailCardsNo;
    public List<Space.PurchasableSpace> owned_spaces;

    public Player(string name,Token token,bool isHuman,int start_money = 1500)
    {
        this.name = name;
        this.owned_spaces = new List<Space.PurchasableSpace>();
        this.cash = start_money;
        this.getOutOfJailCardsNo = 0;
        this.isHuman = isHuman;
    }


    public void ReceiveCash(int cash)
    {
        this.cash += cash;
    }
}
}