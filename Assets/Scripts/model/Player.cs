using System;
using System.Collections.Generic;
using UnityEngine;

namespace Model{
    /// <summary>
    /// The outcome of <see cref="Player"/>'s decision
    /// </summary>
public enum Decision_outcome {
    SUCCESSFUL, NOT_ENOUGH_MONEY, NOT_ENOUGH_ASSETS,
    NOT_ALL_PROPERTIES_IN_GROUP,DIFFERENCE_IN_HOUSES,NO_HOUSES,MAX_HOUSES,
    OTHER, NONE }
    /// <summary>
    /// The reason of taking decison by a <see cref="Player"/>
    /// </summary>
public enum Decision_trigger {
    GOTOJAIL, PAYMONEY, BUYPROPERTY, BID, INJAIL, OK, PAYORCARD, UNSPECIFIED }
/// <summary>
/// This class represents a PT player, both human and AI.
/// </summary>
[System.Serializable]
public class Player
{
    /// <summary>bool value representig if the  Player  is human</summary>
    public bool isHuman;
    /// <summary>Player 's name</summary>
    public string name;
    /// <summary>Player 's token</summary>
    public Token token;
    /// <summary>Player 's moeny</summary>
    public int cash;
    /// <summary>Number of 'Get Out Of Jail Free' cards held by <c>Player</c></summary>
    public int getOutOfJailCardsNo;
    /// <summary>Player 's coler</summary>
    protected int color;
    /// <summary>bool value indicating if  Player  is allowed to purchase</summary>
    public bool allowed_to_buy;
    /// <summary>number of turns the  Player  has to spend in jail</summary>
    public int in_jail;
    /// <summary>Player 's current position</summary>
    public int position;
    /// <summary>List of spaces owned by  Player</summary>
    public List<Space.Purchasable> owned_spaces;

    public Player(string name,Token token,bool isHuman,int color,int position = 1,int money = 1500)
    {
        this.name = name;
        this.token = token;
        this.isHuman = isHuman;
        this.cash = money;
        this.color = color;
        this.getOutOfJailCardsNo = 0;
        this.allowed_to_buy = false;
        this.in_jail = 0;
        this.position = position;
        this.owned_spaces = new List<Space.Purchasable>();
    }
/// <summary>
/// Send  Player  to jail
/// </summary>
    public void go_to_jail()
    {
        this.position = 11;
        this.in_jail = 2;
    }
/// <summary>
/// Add's specified amount to  Player  money
/// </summary>
/// <param name="cash">The money amount </param>
    public void ReceiveCash(int cash)
    {
        this.cash += cash;
    }
/// <summary>
/// Method used to make <c>Player</c> pay specified amount of money.
/// </summary>
/// <param name="amount">The amount to pay</param>
/// <param name="recipient">Optional: Recipient<br/>NOTE: if <paramref name="board"/> is specfied then <paramref name="recipient"/> does not recieve moeny</param>
/// <param name="board">Optional: If speciified, the amount paid lands on 'Free Parking' space</param>
/// <returns>Decision Outcome</returns>
    public Decision_outcome PayCash(int amount, Player recipient = null, Board board = null)
    {
        if(totalValueOfAssets() < amount) { return Decision_outcome.NOT_ENOUGH_ASSETS; }
        if(amount > this.cash) { return Decision_outcome.NOT_ENOUGH_MONEY; }
        if(board != null) {
            this.cash -= amount;
            board.parkingFees+=amount;
            return Decision_outcome.SUCCESSFUL;
            }
        if(recipient == null) {
            this.cash -= amount;
            return Decision_outcome.SUCCESSFUL;
            }
        recipient.ReceiveCash(amount);
        this.cash -= amount;
        return Decision_outcome.SUCCESSFUL;
    }
/// <summary>
/// Sells specified  Player 's owned space
/// </summary>
/// <param name="property">Property in quuestion</param>
/// <param name="board">The reference to the  Board  to which this space is attached </param>
/// <returns>Decision Outcome</returns>
    public Decision_outcome SellProperty(Space.Purchasable property, Board board)
    {
        if(property is Model.Space.Property)
        {
            if(((Space.Property)(property)).differenceInHouses(board) != 0)
            {
                return Decision_outcome.DIFFERENCE_IN_HOUSES;
            } else {
                if(property.isMortgaged)
                {
                    ReceiveCash(property.cost/2);
                    property.isMortgaged = false;
                } else { ReceiveCash(property.cost); }
                owned_spaces.Remove(property);
                property.owner = null;
                return Decision_outcome.SUCCESSFUL;
            }
        }
        else {
            if(property.isMortgaged)
            {
                ReceiveCash(property.cost/2);
                property.isMortgaged = false;
            } else { ReceiveCash(property.cost); }
            owned_spaces.Remove(property);
            property.owner = null;
            return Decision_outcome.SUCCESSFUL;
        } 
    }
/// <summary>
/// Method used to by purchase by Player the specified space 
/// </summary>
/// <param name="property">Space in question</param>
/// <returns>Decision Outcome</returns>
    public Decision_outcome BuyProperty(Space.Purchasable property)
    {
        if(cash < property.cost)
            {
                return Decision_outcome.NOT_ENOUGH_MONEY;
            } else {
                PayCash(property.cost);
                owned_spaces.Add(property);
                property.owner = this;
                return Decision_outcome.SUCCESSFUL;
            } 
    }
    /// <summary>
    /// Method used to by purchase by Player the specified space and the price
    /// </summary>
    /// <param name="property">Space in question</param>
    /// <param name="cost">The price amount</param>
    /// <returns></returns>
    public Decision_outcome BuyProperty(Space.Purchasable property, int cost)
    {
        if(cash < cost)
            {
                return Decision_outcome.NOT_ENOUGH_MONEY;
            } else {
                PayCash(cost);
                owned_spaces.Add(property);
                property.owner = this;
                return Decision_outcome.SUCCESSFUL;
            } 
    }
/// <summary>
/// Calculates the total amount of assets like houses, hotel, owned space and money
/// </summary>
/// <returns>The total value of assets</returns>
    public int totalValueOfAssets()
    {
        int total = 0;
        foreach(Space.Purchasable space in owned_spaces)
        {
            if(space is Space.Property)
            {
                if(((Space.Property)(space)).noOfHouses == 5)
                {
                    total += ((Space.Property)(space)).hotel_cost + 4*((Space.Property)(space)).house_cost;
                } else {
                    total += ((Space.Property)(space)).noOfHouses*((Space.Property)(space)).house_cost;
                }
            }
            if(space.isMortgaged)
            {
                total += space.cost/2;
            } else {
                total += space.cost;
            }
        }
        total += this.cash;
        return total;
    }

///<returns>
/// Returns a list of all spaces type of Station owned by a player
///</returns>
        public List<Model.Space.Station> ownedStations()
    {
        List<Model.Space.Station> stations = new List<Model.Space.Station>();
        foreach(Model.Space.Purchasable space in owned_spaces)
        {
            if(space is Model.Space.Station)
            {
                stations.Add(((Model.Space.Station)(space)));
            }
        }
        return stations;
    }
///<returns>
/// Returns a list of all spaces type of Utility owned by a player
///</returns>
        public List<Model.Space.Utility> ownedUtilities()
        {
        List<Model.Space.Utility> utilities = new List<Model.Space.Utility>();
        foreach(Model.Space.Purchasable space in owned_spaces)
        {
            if(space is Model.Space.Utility)
            {
                utilities.Add(((Model.Space.Utility)(space)));
            }
        }
        return utilities;
    }
///<param name="group">Property's color group</param> 
///<returns>
/// Returns a list of all spaces type of Property owned by a player of given color  Group 
///</returns>
    public List<Model.Space.Property> ownedPropertiesInGroup(Group group)
    {
        List<Model.Space.Property> spacesInGroup = new List<Model.Space.Property>();
        foreach(Model.Space.Purchasable space in owned_spaces)
        {
            if(space is Model.Space.Property)
            {
                if(((Model.Space.Property)(space)).group == group)
                {
                    spacesInGroup.Add(((Model.Space.Property)(space)));
                }
            }
        }
        return spacesInGroup;
    }
///<returns>Returns Player's color</returns>
    public Color Color()
    {
        Color color;
        if ( ColorUtility.TryParseHtmlString("#"+this.color.ToString("X6")+"FF", out color))
        { return color; }
        else { return UnityEngine.Color.black; }
    }
}
}