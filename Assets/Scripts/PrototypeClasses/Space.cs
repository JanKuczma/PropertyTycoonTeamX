/// <summary>
///First draft. Everything in this class is subject to change.
///
/// All spaces on the board will be an object of this class.
/// Methods need filling in and thinking about with some testing, this is just a rough outline.
/// </summary>
namespace Model{
public abstract class Space
{
    public int position;
    public string name;
    public SqType type;

    public class PurchasableSpace : Space
    {
        public Player owner;
        public int cost;
        public int rentAmount;
                //trigger on player arriving
        public void collectRent(Player player)
        {
            //something like 
            // if (owner)
            // {
            //     player.payRent(owner);
            // }
        }

        public override string ToString()
        {
            return position + " " + name + " £" + cost;
        }
    }

    public class Property : PurchasableSpace
    {
        int noOfHouses;
        
        public Property(int position, string name, int cost, int rentAmount)
        {
            this.position = position;
            this.name = name;
            this.cost = cost;
            this.rentAmount = rentAmount;
            this.noOfHouses = 0;
            this.type = SqType.PROPERTY;
        }
        //add property methods
    }

    public class Utility : PurchasableSpace
    {
        public Utility(int position, string name, SqType type, int cost, int rentAmount)
        {
            this.position = position;
            this.name = name;
            this.cost = cost;
            this.rentAmount = rentAmount;
            this.type = type;
        }
        //add utility methods
    }

    public class Station : PurchasableSpace
    {
        public Station(int position, string name, int cost)
        {
            this.position = position;
            this.name = name;
            this.cost = cost;
            this.rentAmount = 25;
            this.owner = null;
            this.type = SqType.STATION;
        }
    }
    
    public class Tax : Space
    {
        public int amount;
        public Tax(int position, string name, SqType type, int amount)
        {
            this.position = position;
            this.name = name;
            this.amount = amount;
            this.type = type;
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

    public class FreeParking : Space
    {
        private int collectedFines;
        
        public FreeParking(int position, string name)
        {
            this.position = position;
            this.name = name;
            collectedFines = 0;
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
    
    public class Chance : Space
    {
        public Chance(int position, SqType type, string name)
        {
            this.position = position;
            this.name = name;
            this.type = type;
        }

        public override string ToString()
        {
            return position + " " + name + " (CardStack space)";
        }
    }
}
}