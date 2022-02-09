using System;
using System.Collections.Generic;
using System.Security.Permissions;
using OfficeOpenXml.Drawing.Chart;

public class Player
{
    private string name;
    private Piece token; 

     public Player()
     {
       Console.WriteLine("Enter a name");
       this.name = Console.ReadLine();
       Console.WriteLine("Welcome " + name +"!");
       Console.WriteLine();
     }

     //Throws an error
     public void PickPiece(ref List<Piece> pieces)
     {
         Console.WriteLine("Pick a piece: ");
         Console.WriteLine();
         for (int i = 0; i < pieces.Count; i++)
         {
             Console.WriteLine(i + " " + pieces[i]);
         }

         var choice = Console.Read();
         token = pieces[choice];
     }

     public override string ToString()
     {
         return name + " using " + token;
     }
}

