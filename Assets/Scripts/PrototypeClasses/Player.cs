using System;
using System.Collections.Generic;
using System.Linq;

namespace PrototypeModel{
public class Player
{
    private string name;
    private Piece token;
    private int cash;
    private Card getOutOfJail;

    public Player()
    {
        Console.WriteLine();
        Console.WriteLine("Enter a name");
        this.name = Console.ReadLine();
        Console.WriteLine("Welcome " + name +"!");
        Console.WriteLine();
        cash = 0;
        getOutOfJail = null;
    }

    public void setToken(Piece piece)
    {
        token = piece;
    }

    public void ReceiveCash(int cash)
    {
        this.cash += cash;
    }
     
    /*
     * Getters and ToString override
     */
    public string GetName()
    {
        return name;
    }

    public Piece GetToken()
    {
        return token;
    }

    public override string ToString()
    {
        return name + " is using the " + token + "\n" + name +" has £" + cash + "\n";
    }
}
}