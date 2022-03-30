/// <summary>
///First draft. Everything in this class is subject to change.
///
/// All spaces on the board will be an object of this class.
/// Methods need filling in and thinking about with some testing, this is just a rough outline.
/// </summary>
namespace Model{
public abstract class Space
{
    public SqType type;
    public int position;
    public string name;
    public abstract class Purchasable : Space
    {
        public Player owner;
        public int cost;
        public int[] rents;
        public bool isMortgaged;

        public override string ToString()
        {
            return position + " " + name + " £" + cost;
        }
        public abstract int rent_amount(Board board);
        public Decision_outcome mortgage()
        {
            owner.ReceiveCash(cost/2);
            isMortgaged = true;
            return Decision_outcome.SUCCESSFUL;
        }

        public Decision_outcome pay_off_mortgage()
        {
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

    public class Go : Space
    {
        public int amount;
        public Go(int position, string name, int amount = 200)
        {
            this.position = position;
            this.name = name;
            this.amount = amount;
            this.type = SqType.GO;
        }
    }

    public class VisitJail : Space
    {
        public VisitJail(int position, string name)
        {
            this.position = position;
            this.name = name;
            this.type = SqType.JAILVISIT;
        }
    }

    public class GoToJail : Space
    {
        public GoToJail(int position, string name)
        {
            this.position = position;
            this.name = name;
            this.type = SqType.GOTOJAIL;
        }
    }

    public class Property : Purchasable
    {
        public int noOfHouses;
        public Group group;
        public int house_cost;
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
        //add property methods

        override public int rent_amount(Board board)
        {
            if(board.ownedPropertiesInGroup(this.group,owner).Count == board.allPropertiesInGroup(group).Count)
            {
                //check if all houses are level 0
                int temp_no_houses = 0;
                foreach(Space.Property property in board.ownedPropertiesInGroup(group,owner))
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
                return rents[noOfHouses]; // otherwise just return rent shon on the card
            }
        }
        
        public Decision_outcome buyHouse(Board board)
        {
            if(board.allPropertiesInGroup(this.group).Count != board.ownedPropertiesInGroup(this.group,this.owner).Count)
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
        public int differenceInHouses(Board board)
        {
            foreach(Model.Space.Property prop in board.ownedPropertiesInGroup(group,owner))
            {
                if(prop.noOfHouses != noOfHouses) { return noOfHouses - prop.noOfHouses; }
            }
            return 0;
        }

    }

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
            if(board.ownedUtilities(owner).Count == board.allUtilities().Count)
            {
                return rents[1];    // 10 times dice result
            } else {
                return rents[0];    // 4 times dice result
            }
        }
    }

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
            if (board.ownedStations(owner).Count == 0) { return 0; }
            return rents[board.ownedStations(owner).Count-1]; // depending how many stations player has
        }
    }

    public class Tax : Space
    {
        public int amount;
        public Tax(int position, string name, int amount)
        {
            this.position = position;
            this.name = name;
            this.amount = amount;
            this.type = SqType.TAX;
        }
        
        //Something like
        // public void TaxPlayer(ref Player p, ref FreeParking parkingSpace)
        // {
        //     var x = p.PayMoney(200); // return 200 from player cash
        //     parkingSpace.add(x);
        // }
        
        public override string ToString()
        {
            return position + " " + name + " (action space)";
        }
    }

    public class PotLuck : Space
    {
        public PotLuck(int position, string name)
        {
            this.position = position;
            this.name = name;
            this.type = SqType.POTLUCK;
        }
    }

    public class Chance : Space
    {
        public Chance(int position, string name)
        {
            this.position = position;
            this.name = name;
            this.type = SqType.CHANCE;
        }
    }

    public class FreeParking : Space
    {
        public int collectedFines;
        
        public FreeParking(int position, string name)
        {
            this.position = position;
            this.name = name;
            this.type = SqType.PARKING;
        }
        
        public override string ToString()
        {
            return position + " " + name + " (action space)";
        }
    } 
}
}