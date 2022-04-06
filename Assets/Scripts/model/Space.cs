namespace Model{
/// <summary>
/// Abstract class that is a super class of all spaces of the <c>Board</c>
/// </summary>
[System.Serializable]
public abstract class Space
{
    /// <summary>
    /// Type of the space
    /// </summary>
    public SqType type;
    /// <summary>
    /// Space's position (1 to 40)
    /// </summary>
    public int position;
    /// <summary>
    /// Space's name
    /// </summary>
    public string name;
    /// <summary>
    /// Abstract class that is a super class of all spaces that can be purchased
    /// </summary>
    [System.Serializable]
    public abstract class Purchasable : Space
    {
        /// <summary>
        /// The <c>Player</c> that owns this space
        /// </summary>
        public Player owner;
        /// <summary>
        /// Space's price
        /// </summary>
        public int cost;
        /// <summary>
        /// For <c>Property</c>: List of rent amounts depending on level of the house.<br/>
        /// For <c>Station</c>: List of rent amounts depending number of station owned.<br/>
        /// For <c>Utility</c>: List of rent multipilers depending on sumber of utilities owned.<br/>
        /// </summary>
        public int[] rents;
        /// <summary>
        /// Boolean value inidicating if space is inder mortgage
        /// </summary>
        public bool isMortgaged;
        /// <param name="board">The reference to the <c>Board</c> to which this space is attached</param> 
        /// <returns>
        /// The due rent amount.<br/>For <c>Space.Utility</c> returns the rent multiplier.
        /// </returns>
        public abstract int rent_amount(Board board);
        /// <summary>
        /// Set's the space as mortgaged
        /// </summary>
        /// <returns>
        /// If the space is already mortgaged returns <c>Decision_outcome.OTHER</c>, otherwise <c>Decision_outcome.SUCCESSFUL</c>. 
        /// </returns>
        public Decision_outcome mortgage()
        {
            if(isMortgaged) { return Decision_outcome.OTHER; }
            owner.ReceiveCash(cost/2);
            isMortgaged = true;
            return Decision_outcome.SUCCESSFUL;
        }
        /// <summary>
        /// Pay's of the mortgage - change <c>isMortgaged</c> to <c>false</c>
        /// </summary>
        /// <returns>
        /// If the space is not mortgaged returns <c>Decision_outcome.OTHER</c>, if <c>owner</c> has not enough money returns <c>Decision_outcome.NOT_ENOUGH_MONEY</c>, otherwise <c>Decision_outcome.SUCCESSFUL</c>. 
        /// </returns>
        public Decision_outcome pay_off_mortgage()
        {
            if(!isMortgaged) { return Decision_outcome.OTHER; }
            if(owner.cash < cost/2)
            {
                return Decision_outcome.NOT_ENOUGH_MONEY;
            } else {
                owner.PayCash(cost/2);
                isMortgaged = false;
                return Decision_outcome.SUCCESSFUL;
            }
        }

    }

    [System.Serializable]
    public class Go : Space
    {
        /// <summary>
        /// The amount of money received by a <c>Player</c> when passing.
        /// </summary>
        public int amount;
        public Go(int position, string name, int amount = 200)
        {
            this.position = position;
            this.name = name;
            this.amount = amount;
            this.type = SqType.GO;
        }
    }

    [System.Serializable]
    public class VisitJail : Space
    {
        public VisitJail(int position, string name)
        {
            this.position = position;
            this.name = name;
            this.type = SqType.JAILVISIT;
        }
    }

    [System.Serializable]
    public class GoToJail : Space
    {
        public GoToJail(int position, string name)
        {
            this.position = position;
            this.name = name;
            this.type = SqType.GOTOJAIL;
        }
    }

    [System.Serializable]
    public class Property : Purchasable
    {
        /// <summary>
        /// Number of houses on this property
        /// </summary>
        public int noOfHouses;
        /// <summary>
        /// Color gorup to which this property belongs
        /// </summary>
        public Group group;
        /// <summary>
        /// A single house cost
        /// </summary>
        public int house_cost;
        /// <summary>
        /// Hotel cost
        /// </summary>
        public int hotel_cost;
        
        public Property(int position, string name, int cost, Group group, int[] rents, int house_cost, int hotel_cost)
        {
            this.position = position;
            this.name = name;
            this.cost = cost;
            this.noOfHouses = 0;
            this.rents = rents;
            this.type = SqType.PROPERTY;
            this.group = group;
            this.house_cost = house_cost;
            this.hotel_cost = hotel_cost;
            this.isMortgaged = false;
        }

        override public int rent_amount(Board board)
        {
            //check if owns all properties in this Group
            if(owner.ownedPropertiesInGroup(this.group).Count == board.allPropertiesInGroup(group).Count)
            {
                //check if all houses are level 0
                int temp_no_houses = 0;
                foreach(Space.Property property in owner.ownedPropertiesInGroup(group))
                {
                    temp_no_houses += property.noOfHouses;
                }
                if(temp_no_houses == 0)
                {
                    return rents[0]*2; // doubled rent
                } else {
                    return rents[noOfHouses]; // it is not doubled if some properties have been developed
                }
            } else {
                return rents[noOfHouses]; // otherwise just return rent shown on the property card
            }
        }
        /// <summary>
        /// This method is used to build/buy a house on this property
        /// </summary>
        /// <param name="board">The reference to the <c>Board</c> to which this space is attached</param>
        /// <returns> Appropriate <c>Decision_outcome</c> value.</returns>
        public Decision_outcome buyHouse(Board board)
        {
            if(board.allPropertiesInGroup(this.group).Count != owner.ownedPropertiesInGroup(this.group).Count)
            {
                return Decision_outcome.NOT_ALL_PROPERTIES_IN_GROUP;
            }
            else if(this.differenceInHouses(board) == 1)
            {
                return Decision_outcome.DIFFERENCE_IN_HOUSES;
            } 
            else if(noOfHouses == 5)
            {
                return Decision_outcome.MAX_HOUSES;
            }
            else if(house_cost > owner.cash)
            {
                return Decision_outcome.NOT_ENOUGH_MONEY;
            }
            else if(noOfHouses == 4 && hotel_cost > owner.cash)
            {
                return Decision_outcome.NOT_ENOUGH_MONEY;
            } else {
                if(noOfHouses == 4)
                {
                    owner.PayCash(hotel_cost);
                } else {
                    owner.PayCash(house_cost);
                }
                noOfHouses += 1;
                return Decision_outcome.SUCCESSFUL;
            }
        }
        /// <summary>
        /// This method is used to demolish/sell a house on this property
        /// </summary>
        /// <param name="board">The reference to the <c>Board</c> to which this space is attached</param>
        /// <returns> Appropriate <c>Decision_outcome</c> value.</returns>
        public Decision_outcome sellHouse(Board board)
        {
            if(differenceInHouses(board) == -1)
            {
                return Decision_outcome.DIFFERENCE_IN_HOUSES;
            } 
            else if(noOfHouses == 0)
            {
                return Decision_outcome.NO_HOUSES;
            } else {
                if(noOfHouses == 5)
                {
                    owner.ReceiveCash(hotel_cost);
                } else {
                    owner.ReceiveCash(house_cost);
                }
                noOfHouses -= 1;
                return Decision_outcome.SUCCESSFUL;
            }
        }
        /// <summary>
        /// Checks if there's difference in number of houses on properties of this Property group
        /// </summary>
        /// <param name="board">The reference to the <c>Board</c> to which this space is attached</param>
        /// <returns>
        /// <value>0</value> if no difference<br/>
        /// <value>1</value> if there's property that has less houses<br/>
        /// <value>-1</value> if there's property that has more houses<br/>
        /// </returns>
        public int differenceInHouses(Board board)
        {
            foreach(Model.Space.Property prop in owner.ownedPropertiesInGroup(group))
            {
                if(prop.noOfHouses != noOfHouses) { return noOfHouses - prop.noOfHouses; }
            }
            return 0;
        }

    }

    [System.Serializable]
    public class Utility : Purchasable
    {
        public Utility(int position, string name, int cost, int[] rents)
        {
            this.position = position;
            this.name = name;
            this.cost = cost;
            this.type = SqType.UTILITY;
            this.rents = rents;
            this.isMortgaged = false;
        }
            //add utility methods
        public override int rent_amount(Board board)
        {
            if(owner.ownedUtilities().Count == board.allUtilities().Count)
            {
                return rents[1];    // 10 times dice result
            } else {
                return rents[0];    // 4 times dice result
            }
        }
    }

    [System.Serializable]
    public class Station : Purchasable
    {
        public Station(int position, string name, int cost, int[] rents)
        {
            this.position = position;
            this.name = name;
            this.cost = cost;
            this.owner = null;
            this.type = SqType.STATION;
            this.rents = rents;
            this.isMortgaged = false;
        }
        override public int rent_amount(Board board)
        {
            if (owner.ownedStations().Count == 0) { return 0; }
            return rents[owner.ownedStations().Count-1]; // depending how many stations player has
        }
    }

    [System.Serializable]
    public class Tax : Space
    {
        /// <summary>
        /// The tax amount
        /// </summary>
        public int amount;
        public Tax(int position, string name, int amount)
        {
            this.position = position;
            this.name = name;
            this.amount = amount;
            this.type = SqType.TAX;
        }
        
    }

    [System.Serializable]
    public class PotLuck : Space
    {
        public PotLuck(int position, string name)
        {
            this.position = position;
            this.name = name;
            this.type = SqType.POTLUCK;
        }
    }

    [System.Serializable]
    public class Chance : Space
    {
        public Chance(int position, string name)
        {
            this.position = position;
            this.name = name;
            this.type = SqType.CHANCE;
        }
    }

    [System.Serializable]
    public class FreeParking : Space
    {
        public int collectedFines;
        
        public FreeParking(int position, string name)
        {
            this.position = position;
            this.name = name;
            this.type = SqType.PARKING;
        }
    } 
}
}