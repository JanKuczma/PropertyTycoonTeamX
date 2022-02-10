using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using OfficeOpenXml.Drawing.Chart;

public class Player
{
    private string name;
    private Piece token;
    private int cash;

     public Player()
     {
         Console.WriteLine();
       Console.WriteLine("Enter a name");
       this.name = Console.ReadLine();
       Console.WriteLine("Welcome " + name +"!");
       Console.WriteLine();
     }

     public Player(Player p)
     {
         name = p.GetName();
         token = p.GetToken();
     }

     public void PickPiece(ref List<Piece> pieces)
     {
         Console.WriteLine("Pick a piece: ");
         Console.WriteLine();
         for (int i = 0; i < pieces.Count; i++)
         {
             Console.WriteLine(i+1 + ". " + pieces[i]);
         }

         var choice = Int32.Parse(Console.ReadLine());
         choice -= 1;
         token = pieces.ElementAt(choice);
         pieces.RemoveAt(choice);
         Console.WriteLine();
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

