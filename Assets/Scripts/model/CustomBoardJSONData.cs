using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Model
{
    public class CustomBoardJSONData
    {
        [System.Serializable]
        private class CustomBoardWrapper
        {
            [SerializeField] public CustomSpaceWrapper[] spaces = new CustomSpaceWrapper[40];
        }

        [System.Serializable]
        private class CustomSpaceWrapper
        {
            public int position;
            public string name;
            public string type_action;
            public int cost_amount;
            public string group;
            public int[] rents;
            public int house_cost;
            public int hotel_cost;
        }

        public static Board loadBoard(string json_format)
        {
            CustomBoardWrapper boardData = JsonUtility.FromJson<CustomBoardWrapper>(json_format);
            Board board = new Board();
            foreach(CustomSpaceWrapper sD in boardData.spaces)
            {
                SqType type = (SqType)System.Enum.Parse(typeof(SqType),sD.type_action);
                switch(type)
                {
                    case SqType.PROPERTY:
                    board.spaces[sD.position-1] = new Space.Property(sD.position,sD.name,sD.cost_amount,
                        (Group)System.Enum.Parse(typeof(Group),sD.group),sD.rents,sD.house_cost,sD.hotel_cost);
                    break;
                    case SqType.STATION:
                    board.spaces[sD.position-1] = new Space.Station(sD.position,sD.name,sD.cost_amount,sD.rents);
                    break;
                    case SqType.UTILITY:
                    board.spaces[sD.position-1] = new Space.Utility(sD.position,sD.name,sD.cost_amount,sD.rents);
                    break;
                    case SqType.TAX:
                    board.spaces[sD.position-1] = new Space.Tax(sD.position,sD.name,sD.cost_amount);
                    break;
                    case SqType.POTLUCK:
                    board.spaces[sD.position-1] = new Space.PotLuck(sD.position,sD.name);
                    break;
                    case SqType.CHANCE:
                    board.spaces[sD.position-1] = new Space.Chance(sD.position,sD.name);
                    break;
                    case SqType.GO:
                    board.spaces[sD.position-1] = new Space.Go(sD.position,sD.name,sD.cost_amount);
                    break;
                    case SqType.PARKING:
                    board.spaces[sD.position-1] = new Space.FreeParking(sD.position,sD.name);
                    break;
                    case SqType.GOTOJAIL:
                    board.spaces[sD.position-1] = new Space.GoToJail(sD.position,sD.name);
                    break;
                    case SqType.JAILVISIT:
                    board.spaces[sD.position-1] = new Space.VisitJail(sD.position,sD.name);
                    break;
                }
            }
            return board;
        }

        // private static CustomSpaceWrapper spaceToWrapper(Space space)
        // {
        //     var spData = new CustomSpaceWrapper();
        //     
        // }
        
        // private static CustomSpaceWrapper spaceToWrapper(Space space)
        // {
        //     CustomSpaceWrapper spData = new CustomSpaceWrapper();
        //     spData.position = space.position;
        //     spData.name = space.name;
        //     spData.type_action = space.type.ToString();
        //     switch (space.type)
        //     {
        //         case SqType.PROPERTY:
        //             spData.cost_amount = ((Space.Property) space).cost;
        //             spData.group = ((Space.Property) space).group.ToString();
        //             spData.rents = ((Space.Property) space).rents;
        //             spData.house_cost = ((Space.Property) space).house_cost;
        //             spData.hotel_cost = ((Space.Property) space).hotel_cost;
        //             break;
        //         case SqType.STATION:
        //             spData.cost_amount = ((Space.Station) space).cost;
        //             spData.rents = ((Space.Station) space).rents;
        //             break;
        //         case SqType.UTILITY:
        //             spData.cost_amount = ((Space.Utility) space).cost;
        //             spData.rents = ((Space.Utility) space).rents;
        //             break;
        //         case SqType.TAX:
        //             spData.cost_amount = ((Space.Tax) space).amount;
        //             break;
        //         case SqType.GO:
        //             spData.cost_amount = ((Space.Go) space).amount;
        //             break;
        //     }
        //     return spData;
        // }
    }
}