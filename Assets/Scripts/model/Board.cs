using System.Collections;
using System.Collections.Generic;

namespace Model{
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

    public int calc_rent(Space.Property space, Player player)
    {
        
        if(ownedPropertiesInGroup(space.group,player).Count == allPropertiesInGroup(space.group).Count)
        {
            //check if all houses are level 0
            int temp_no_houses = 0;
            foreach(Space.Property property in ownedPropertiesInGroup(space.group,player))
            {
                temp_no_houses += property.noOfHouses;
            }
            if(temp_no_houses == 0)
            {
                return space.rents[0]*2; // doubled rent
            } else {
                return space.rents[space.noOfHouses]; // it is not doubled if some properties have been developed
            }
        } else {
            return space.rents[space.noOfHouses]; // otherwise just return rent shon on the card
        }
    }
    public int calc_rent(Space.Utility space, Player player)
    {
        if(ownedUtilities(player).Count == allUtilities().Count)
        {
            return space.rents[1];    // 10 times dice result
        } else {
            return space.rents[0];    // 4 times dice result
        }
    }
    public int calc_rent(Space.Station space, Player player)
    {
        return space.rents[ownedStations(player).Count-1]; // depending how many stations player has
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