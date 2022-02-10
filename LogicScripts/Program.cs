using System;
using System.Collections.Generic;

public class Program
{
    /*
     * Creates a new card reader object, writes the name and description for each element of the card stack
     * onto a Card object and appends that to a CardStack
     * The completed CardStack is then shuffled before being returned
     */
    private static CardStack InstantiatePotLuckStack()
    {
        // Cell numbers on the spreadsheet
        const int potLuckDetailsBegin = 5;
        const int potLuckDetailsEnd = 21;
        var xr = new XlsCardReader();
        var potLuck = xr.WriteStack(potLuckDetailsBegin, potLuckDetailsEnd);

        potLuck.ShuffleStack();
        
        return potLuck;
    }
   /*
    * Creates a new card reader object, writes the name and description for each element of the card stack
    * onto a Card object and appends that to a CardStack
    * The completed CardStack is then shuffled before being returned
    */
    private static CardStack InstantiateOppKnocksStack()
    {
        // Cell numbers on the spreadsheet
        const int oppKnocksDetailsBegin = 25;
        const int oppKnocksDetailsEnd = 40;

        var xr = new XlsCardReader();
        var oppKnocks = xr.WriteStack(oppKnocksDetailsBegin, oppKnocksDetailsEnd);
        
        oppKnocks.ShuffleStack();
        
        return oppKnocks;
    }
        
    /*
     * Instantiates each possible piece as specified by Mr Raffles
     * Returns them in a List
     */
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
    
    /*
     * Instantiates Player objects. Currently, dach Player constructor reads user input for a name.
     */
    private static List<Player> InstantiatePlayers()
    {
        //Proper error handling to be done later, if necessary
        Console.Write("Please enter the number of players!\n>");
        var input = Int32.Parse(Console.ReadLine());
        var players = new List<Player>();
        for (var i = 0; i < input; i++)
        {
            players.Add(new Player());
        }
        return players;
    }
    
    /*
     * Each Player has the opportunity to pick a Piece.
     * By having the lists passed by reference, the overhead of this operation is reduced.
     * After a piece is picked, it is automatically removed from the available choices for the next player
     */
    private static void PickPieces(ref List<Player> players, ref List<Piece> pieces)
    {
        foreach(var p in players)
        {
            p.PickPiece(ref pieces);
        }
    }
    
    /*
     * Creates both card stacks, writes each space of the Board into an object and saves them into separate lists
     * Gets all pieces and players
     * Board object is returned containing all of the details 
     */
    public static Board InstantiateBoard(ref List<Player> players)
    {
        // Get cards
        var potLuck = InstantiatePotLuckStack();
        var oppKnocks = InstantiateOppKnocksStack();

        // Get board spaces
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

        // Get pieces
        List<Piece> pieces = InstantiatePieces();

        // Get players
        PickPieces(ref players, ref pieces);
        // Each Piece has now been picked and assigned to a Player so any remaining can be discarded
        
        return new Board(properties, utilities, stations, go, incomeTax, freeParking, superTax,
            goToJail, justVisiting, opportunityKnocksSpace, potLucksSpace, oppKnocks, potLuck);
    }
    
    public static void Main()
    {
        Console.WriteLine("Welcome to Property Tycoon\n");
        // By having the players separate from Board, methods can be called on them from Main directly 
        var players = InstantiatePlayers();
        // Instantiates 2 card decks, all spaces of the board and pieces and contains them all in one class
        var board = InstantiateBoard(ref players);

        var bank = new Bank();
        bank.starterMonies(ref players);

        foreach (var player in players)
        {
            Console.WriteLine(player.ToString());
        }
        
        // //Prints everything
        // board.GetDetails();
    }
}