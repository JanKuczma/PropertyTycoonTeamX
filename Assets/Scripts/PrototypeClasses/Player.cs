using System;
using System.Collections.Generic;

namespace Model{
public class Player
{
    public string name;
    public Token token;
    public int cash;
    public int getOutOfJailCardsNo;
    public List<Space> owned_spaces;
    public bool isHuman;

    public Player(string name,Token token,bool isHuman,int start_money = 1500)
    {
        this.name = name;
        this.owned_spaces = new List<Space>();
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