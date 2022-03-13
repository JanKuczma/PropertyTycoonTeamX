namespace Model2{
public class Board
{
    public Space[] spaces;
    //public CardStack OpportunityKnocks;
    //public CardStack PotLuck;
    public Board()
    {
        spaces = new Space[40];
    }
    public Board(Space[] spaces)
    {
        this.spaces = spaces;
    }

    public static CardStack loadOppurtunityKnocksData()
    {
        return null;
    }
    public static CardStack loadPotLuckData()
    {   
        return null;
    }

    public void saveOppurtunityKnocksData()
    {

    }
    public void savePotLuckData()
    {

    }
}

}