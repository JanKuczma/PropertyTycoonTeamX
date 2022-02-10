using System.Collections.Generic;

public class Bank
{
    private int cash;
    private Dictionary<Player, List<Space.Property>> playerItems;

    public Bank()
    {
        cash = 50000;
        playerItems = new Dictionary<Player, List<Space.Property>>();
    }

    public void starterMonies(ref List<Player> players)
    {
        foreach (var player in players)
        {
            player.ReceiveCash(1500);
        }
    }

    private void printMoney()
    {
        cash = 50000;
    }
}
    
