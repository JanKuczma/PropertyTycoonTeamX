using System;
using System.Collections.Generic;

public class Program
{
    private static CardStack InstantiateStack(ref CardStack cs, int cell1, int cell2)
    {
        var xr = new XlsCardReader();

        xr.WriteStack(cs, cell1, cell2);
        return cs;
    }
    
    
    private static List<Piece> InstantiatePieces()
    {
        var pieces = new List<Piece>();
        var boot = new Piece("Boot", "BootPlaceholder.obj");
        var smartphone = new Piece("Smartphone", "SmartphonePlaceholder.obj");
        var ship = new Piece("Ship", "ShipPlaceholder.obj");
        var hatstand = new Piece("Hatstand", "HatstandPlaceholder.obj");
        var cat = new Piece("Cat", "CatPlaceholder.obj");
        var iron = new Piece("Iron", "IronPlaceholder.obj");
        
        pieces.Add(boot);
        pieces.Add(smartphone);
        pieces.Add(ship);
        pieces.Add(hatstand);
        pieces.Add(cat);
        pieces.Add(iron);
        
        return pieces;
    }
    
    private static List<Player> InstantiatePlayers()
    {
        //Proper error handling to be done later
        var input = 2;
        var players = new List<Player>();
        for (var i = 0; i < input; i++)
        {
            players.Add(new Player());
        }
        return players;
    }
    
    private static void PickPieces(ref List<Player> players, ref List<Piece> pieces)
    {
        foreach(var p in players)
        {
            p.PickPiece(ref pieces);
        }
    }
    
    public static Board InstantiateBoard()
    {
        
        // Cell numbers on the spreadsheet
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

        List<Piece> pieces = InstantiatePieces();

        var players = InstantiatePlayers();

        PickPieces(ref players, ref pieces);
        // Pieces have now been picked and assigned to Player so any remaining can be discarded by not being passed
        // to the board
        
        return new Board(properties, utilities, stations, go, incomeTax, freeParking, superTax,
            goToJail, justVisiting, opportunityKnocksSpace, potLucksSpace, oppKnocks, potLuck, players);
    }
    
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Property Tycoon\n");
        // Instantiates 2 card decks, all spaces of the board, pieces and players and contains them all in one class
        var board = InstantiateBoard();
        
        //Prints everything
        board.getDetails();
    }
}