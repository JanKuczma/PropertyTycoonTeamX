/// <summary>
///First draft. Everything in this class is subject to change.
///
/// All spaces on the board will be an object of this class.
/// Methods need filling in and thinking about with some testing, this is just a rough outline.
/// </summary>
public abstract class Space
{
    private int position;
    private string name;
    private bool canBeBought;

    public class Property : Space
    {
        private Player owner;
        private bool owned;
        private int cost;
        private int rentAmount;
        
        public Property(int position, string name, int cost, int rentAmount)
        {
            this.position = position;
            this.name = name;
            this.canBeBought = true;
            this.cost = cost;
            this.rentAmount = rentAmount;
        }
        
        //trigger on player arriving
        public void collectRent(Player player)
        {
            //something like 
            // if (owned)
            // {
            //     player.payRent(owner);
            // }
        }

        public override string ToString()
        {
            return position + " " + name + " £" + cost;
        }
    }

    public class Utility : Space
    {
        private int cost;
        public Utility(int position, string name, int cost)
        {
            this.position = position;
            this.name = name;
            this.canBeBought = true;
            this.cost = cost;
        }
        //add utility methods
        public void collectRent(Player p)
        {
            //give owner cash from p
        }

        public override string ToString()
        {
            return position + " " + name + " £" + cost;
        }
    }

    public class Station : Space
    {
        private Player owner;
        private bool owned;
        private int cost;
        private int rentAmount;
        public Station(int position, string name, int cost)
        {
            this.position = position;
            this.name = name;
            this.cost = cost;
            this.rentAmount = 25;
            this.canBeBought = true;
        }

        public void collectRent(Player p)
        {
            //give owner cash from p
        }

        public override string ToString()
        {
            return position + " " + name + " £" + cost;
        }
    }
    
    public class Go : Space
    {
        public Go()
        {
            
        }
        public Go(int position, string name)
        {
            this.position = position;
            this.name = name;
            this.canBeBought = false;
        }
        
        // Something like
        // public int passGo(Player player)
        // {
        //      player may be passed by ref
        //     player.receiveMoney(200);
        // }

        public override string ToString()
        {
            return position + " " + name + " (action space)";
        }
    }
    
        public class IncomeTax : Space
        {
            public IncomeTax()
            {
                
            }
            public IncomeTax(int position, string name)
            {
                this.position = position;
                this.name = name;
                this.canBeBought = false;
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
                this.canBeBought = false;
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

        public class SuperTax : Space
        {
            private int taxAmount;

            public SuperTax()
            {
                
            }
            public SuperTax(int position, string name)
            {
                this.position = position;
                this.name = name;
                this.canBeBought = false;
                taxAmount = 100;
            }

            // public void TaxPlayer(ref Player player, ref FreeParking parking)
            // {
            //     var x = player.PayMoney(taxAmount);
            //     parking.addFine(x);
            // }
            public override string ToString()
            {
                return position + " " + name + " (action space)";
            }
        }
        
    public class GoToJail : Space
    {
        private int cost;

        public GoToJail()
        {
            
        }
        
        public GoToJail(int position, string name)
        {
            this.position = position;
            this.name = name;
            this.canBeBought = false;
        }
        //add methods
        public override string ToString()
        {
            return position + " " + name + " (action space)";
        }
    }

    public class JustVisiting : Space
    {

        public JustVisiting()
        {
            
        }
        public JustVisiting(int position, string name)
        {
            this.position = position;
            this.name = name;
            this.canBeBought = false;
        }
        //Create invisible wall method for just visiting
        public override string ToString()
        {
            return position + " " + name + " (action space)";
        }
    }
    
    
    
    public class OpportunityKnocks : Space
    {
        private CardStack oppKnocks;

        public OpportunityKnocks(int position, string name)
        {
            this.position = position;
            this.name = name;
            this.canBeBought = false;
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

    public class PotLuck : Space
    {
        private CardStack potLuck;

        public PotLuck(int position, string name)
        {
            this.position = position;
            this.name = name;
            this.canBeBought = false;
        }
        
        public void AddCards(CardStack c)
        {
            potLuck = c;
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