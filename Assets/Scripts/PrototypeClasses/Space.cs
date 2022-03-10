/// <summary>
///First draft. Everything in this class is subject to change.
///
/// All spaces on the board will be an object of this class.
/// Methods need filling in and thinking about with some testing, this is just a rough outline.
/// </summary>
namespace Model{
public abstract class Space
{
    private int position;
    private string name;

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
        }
        //add property methods
    }

    public class Utility : PurchasableSpace
    {
        public Utility(int position, string name, int cost, int rentAmount)
        {
            this.position = position;
            this.name = name;
            this.cost = cost;
            this.rentAmount = rentAmount;
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
        }
    }
    
    public class Tax : Space
    {
        int amount;
        public Tax()
        {
            
        }
        public Tax(int position, string name, int amount)
        {
            this.position = position;
            this.name = name;
            this.amount = amount;
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

        public FreeParking()
        {
            
        }
        
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
        private CardStack oppKnocks;

        public Chance(int position, string name)
        {
            this.position = position;
            this.name = name;
        }

        public void AddCards(CardStack c)
        {
            oppKnocks = c;
        }
        
        // public void dispenseCard(ref Player p)
        // {
        //     //trigger action to remove card from list, show to player and then add to end of list
        // }
        public override string ToString()
        {
            return position + " " + name + " (CardStack space)";
        }
    }
}
}