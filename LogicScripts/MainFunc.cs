using System;
using System.Collections.Generic;

/// <summary>
/// Displaying basic functionality and how certain objects work
/// </summary>

public class MainFunc
{

    private static CardStack InstantiateStack(ref CardStack cs, int cell1, int cell2)
    {
        var xr = new XlsCardReader();

        xr.WriteStack(cs, cell1, cell2);
        return cs;
    }

    public static Board InstantiateBoard()
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

        var xr = new XlsSpaceReader();

        List<Space.Property> properties = xr.WritePropertySpaces();
        List<Space.Utility> utilities = xr.WriteUtilitySpaces();
        List<Space.Station> stations = xr.WriteStationSpaces();
        Space.Go go = xr.WriteGoSpace();
        Space.IncomeTax incomeTax = xr.WriteIncomeTaxSpace();
        Space.FreeParking freeParking = xr.WriteFreeParkingSpace();
        Space.SuperTax superTax = xr.WriteSuperTaxSpace();
        Space.GoToJail goToJail = xr.WriteGoToJailSpace();
        Space.JustVisiting justVisiting = xr.WriteJustVisitingSpace();
        List<Space.OpportunityKnocks> opportunityKnocksSpace = xr.WriteOpportunityKnocksSpace();
        List<Space.PotLuck> potLucksSpace = xr.WritePotLuckSpaces();

        return new Board(properties, utilities, stations, go, incomeTax, freeParking, superTax,
            goToJail, justVisiting, opportunityKnocksSpace, potLucksSpace, oppKnocks, potLuck);
    }

public static void Main(string[] args)
    {
        // Instantiates 2 card decks, all spaces of the board and contains them all in one class
        var board = InstantiateBoard();
        
        //Prints all spaces and cards
        board.getDetails();
    }
}