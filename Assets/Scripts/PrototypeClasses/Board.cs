using System;
using System.Collections.Generic;
namespace Model{
public class Board
{
    //Board spaces
    private List<Space.Property> properties;
    private List<Space.Utility> utilities;
    private List<Space.Station> stations;
    private Space.Go go;
    private Space.IncomeTax incomeTax;
    private Space.FreeParking freeParking;
    private Space.SuperTax superTax;
    private Space.GoToJail goToJail;
    private Space.JustVisiting justVisiting;
    private List<Space.OpportunityKnocks> opportunityKnocksSpace;
    private List<Space.PotLuck> potLucksSpace;
    
    //Bankasd
    
    
    //Card decks
    private CardStack oppKnocks;
    private CardStack potLuck;

    public Board(
        List<Space.Property> properties, 
        List<Space.Utility> utilities,
        List<Space.Station> stations,
        Space.Go go,
        Space.IncomeTax incomeTax,
        Space.FreeParking freeParking,
        Space.SuperTax superTax,
        Space.GoToJail goToJail,
        Space.JustVisiting justVisiting,
        List<Space.OpportunityKnocks> opportunityKnocksSpace,
        List<Space.PotLuck> potLucksSpace,
        CardStack oppKnocks,
        CardStack potLuck
    )
    {
        this.properties = properties;
        this.utilities = utilities;
        this.stations = stations;
        this.go = go;
        this.incomeTax = incomeTax;
        this.freeParking = freeParking;
        this.superTax = superTax;
        this.goToJail = goToJail;
        this.justVisiting = justVisiting;
        this.opportunityKnocksSpace = opportunityKnocksSpace;
        this.potLucksSpace = potLucksSpace;
        this.oppKnocks = oppKnocks;
        this.potLuck = potLuck;
    }
    
    

    public void ShufflePotLuck()
    {
        potLuck.ShuffleStack();
    }

    public void ShuffleOppKnocks()
    {
        oppKnocks.ShuffleStack();
    }
    
    // Just for testing
    public void GetDetails()
    {
        //Print board spaces
        Console.WriteLine("Properties:");
        foreach (var p in properties)
        {
            Console.WriteLine(p.ToString());
        }
        Console.WriteLine();
        Console.WriteLine("Utilities:");
        foreach (var u in utilities)
        {
            Console.WriteLine(u.ToString());
        }
        Console.WriteLine();
        Console.WriteLine("Stations");
        foreach (var s in stations)
        {
            Console.WriteLine(s.ToString());
        }
        Console.WriteLine();
        Console.WriteLine("Action Spaces");
        Console.WriteLine(go.ToString());
        Console.WriteLine(incomeTax.ToString());
        Console.WriteLine(freeParking.ToString());
        Console.WriteLine(superTax.ToString());
        Console.WriteLine(goToJail.ToString());
        Console.WriteLine(justVisiting.ToString());
        Console.WriteLine();
        Console.WriteLine("Opp Knocks Space");
        foreach (var o in opportunityKnocksSpace)
        {
            Console.WriteLine(o.ToString());
        }
        Console.WriteLine();
        Console.WriteLine("Pot Luck Space");
        foreach (var p in potLucksSpace)
        {
            Console.WriteLine(p.ToString());
        }
        Console.WriteLine();
        
        // Print cards
        Console.WriteLine("Opp Knocks Cards");
        oppKnocks.PrintCards();
        Console.WriteLine();
        Console.WriteLine("Pot Luck Cards");
        potLuck.PrintCards();
        Console.WriteLine();
    }
}
}