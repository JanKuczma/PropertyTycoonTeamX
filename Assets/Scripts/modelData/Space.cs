/// <summary>
///First draft. Everything in this class is subject to change.
///
/// All spaces on the board will be an object of this class.
/// Methods need filling in and thinking about with some testing, this is just a rough outline.
/// </summary>
namespace Model2{
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
        public abstract void collectRent(Player player);

        public override string ToString()
        {
            return position + " " + name + " £" + cost;
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
        }
        override public void collectRent(Player player)
        {
            //something like 
            // if (owner)
            // {
            //     player.payRent(owner);
            // }
        }
        //add property methods
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
        }
        override public void collectRent(Player player)
        {
            //something like 
            // if (owner)
            // {
            //     player.payRent(owner);
            // }
        }
        //add utility methods
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
        }
        override public void collectRent(Player player)
        {
            //something like 
            // if (owner)
            // {
            //     player.payRent(owner);
            // }
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
            this.collectedFines = 0;
            this.type = SqType.PARKING;
        }

        // //Something like
        // public void dispenseFunds(ref Player p)
        // {
        //     p.collectMoney(collectedFines);
        //     collectedFines = 0;
        // }

        public void addFine(int fine)
        {
            collectedFines += fine;
        }
        
        public override string ToString()
        {
            return position + " " + name + " (action space)";
        }
    } 
}
}