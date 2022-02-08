using System;

/// <summary>
/// Displaying basic functionality and how certain objects work
/// </summary>

public class MainFunc
{

    public static CardStack InstantiateStack(ref CardStack cs, int cell1, int cell2)
    {
        var xr = new XlsReader();

        xr.WriteStack(cs, cell1, cell2);
        return cs;
    }
    
    public static void Main(string[] args)
    {
        // No magic numbers necessary
        const int potLuckDetailsBegin = 5;
        const int potLuckDetailsEnd = 21;
        const int oppKnocksDetailsBegin = 25;
        const int oppKnocksDetailsEnd = 40;
        
        var potLuck = new CardStack();
        var oppKnocks = new CardStack();

        InstantiateStack(ref potLuck, potLuckDetailsBegin, potLuckDetailsEnd);
        InstantiateStack(ref oppKnocks, oppKnocksDetailsBegin, oppKnocksDetailsEnd);
        
        
        
        potLuck.ShuffleStack();
        potLuck.PrintCards();
        
        Console.WriteLine("\n\n");
        
        oppKnocks.ShuffleStack();
        oppKnocks.PrintCards();
    }
}