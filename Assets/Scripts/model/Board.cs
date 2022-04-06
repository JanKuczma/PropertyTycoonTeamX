using System.Collections;
using System.Collections.Generic;

namespace Model{
///<summary>
/// This class represents a PT board
///</summary>
/// <param name="spaces">Array of 40 spaces of the board</param>
/// <param name="parkingFees">The amount of money currently gathered on 'Free Parking'</param>

[System.Serializable]
public class Board
{
    public Space[] spaces;
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

///<returns>
/// Returns a list of all spaces type of Station
///</returns>
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

///<returns>
/// Returns a list of all spaces type of Utility
///</returns>
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
///<returns>
/// Returns a list of all spaces type of Property in given color Group
///</returns>
///<param name="group">Color Group of the properties in question<>
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

}
}