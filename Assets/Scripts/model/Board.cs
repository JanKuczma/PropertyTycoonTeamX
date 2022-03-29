using System.Collections;
using System.Collections.Generic;

namespace Model{
public class Board
{
    public Space[] spaces;
    //public CardStack OpportunityKnocks;
    //public CardStack PotLuck;
    public int parkingFees;
    public Board()
    {
        spaces = new Space[40];
        parkingFees = 0;
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


    public List<Model.Space.Station> allStations()
    {
        List<Model.Space.Station> stations = new List<Model.Space.Station>();
        foreach(Model.Space space in this.spaces)
        {
            if(space is Model.Space.Station)
            {
                stations.Add(((Model.Space.Station)(space)));
            }
        }
        return stations;
    }

        public List<Model.Space.Station> ownedStations(Player player)
    {
        List<Model.Space.Station> stations = new List<Model.Space.Station>();
        foreach(Model.Space.Purchasable space in player.owned_spaces)
        {
            if(space is Model.Space.Station)
            {
                stations.Add(((Model.Space.Station)(space)));
            }
        }
        return stations;
    }

    public List<Model.Space.Utility> allUtilities()
    {
        List<Model.Space.Utility> utilities = new List<Model.Space.Utility>();
        foreach(Model.Space space in this.spaces)
        {
            if(space is Model.Space.Utility)
            {
                utilities.Add(((Model.Space.Utility)(space)));
            }
        }
        return utilities;
    }

        public List<Model.Space.Utility> ownedUtilities(Player player)
    {
        List<Model.Space.Utility> utilities = new List<Model.Space.Utility>();
        foreach(Model.Space.Purchasable space in player.owned_spaces)
        {
            if(space is Model.Space.Utility)
            {
                utilities.Add(((Model.Space.Utility)(space)));
            }
        }
        return utilities;
    }

    public List<Model.Space.Property> allPropertiesInGroup(Group group)
    {
        List<Model.Space.Property> spacesInGroup = new List<Model.Space.Property>();
        foreach(Model.Space space in this.spaces)
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
    public List<Model.Space.Property> ownedPropertiesInGroup(Group group, Player player)
    {
        List<Model.Space.Property> spacesInGroup = new List<Model.Space.Property>();
        foreach(Model.Space.Purchasable space in player.owned_spaces)
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

}
}