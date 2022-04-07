using UnityEngine;

namespace Model
{
    /// <summary>
    /// Class used load/save <see cref="Board"/> data from/to json format.
    /// </summary>
    public static class BoardData
    {
        /// <summary>
        /// Wrapper class for <c>Board</c> data.
        /// </summary>
        /*
            Because <see cref="UnityEngine.JsonUtility"/> does not support Dictionaries
            nor polymorphism, the <c>Board</c> data has to be wrapped into simpler format
        */
        [System.Serializable]
        private class BoardWrapper
        {
            [SerializeField] public SpaceWrapper[] spaces = new SpaceWrapper[40];
        }
        /// <summary>
        /// Wrapper class for <c>Space</c> data
        /// </summary>
        [System.Serializable]
        private class SpaceWrapper
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
        /// <summary>
        /// Loads the data from a JSON format
        /// </summary>
        /// <returns><c>Board</c> object of the board.</returns>
        public static Board LoadBoard()
        {
            var defaultBoard = Asset.board_data_json();
            var customBoard = Asset.custom_board_data();
            var board = new Board();
            var defaultBoardWrapper = JsonUtility.FromJson<BoardWrapper>(defaultBoard);
            var customBoardWrapper = JsonUtility.FromJson<BoardWrapper>(customBoard);
            
            foreach (var space in defaultBoardWrapper.spaces)
            {
                foreach (var place in customBoardWrapper.spaces)
                {
                    if (space.position == place.position)
                    {
                        space.name = place.name;
                    }
                }
            }
            
            foreach (var space in defaultBoardWrapper.spaces)
            {
                SqType type = (SqType) System.Enum.Parse(typeof(SqType), space.type_action);
                switch (type)
                {
                    case SqType.PROPERTY:
                        board.spaces[space.position - 1] = new Space.Property(space.position, space.name, space.cost_amount,
                            (Group) System.Enum.Parse(typeof(Group), space.group), space.rents, space.house_cost, space.hotel_cost);
                        break;
                    case SqType.STATION:
                        board.spaces[space.position - 1] =
                            new Space.Station(space.position, space.name, space.cost_amount, space.rents);
                        break;
                    case SqType.UTILITY:
                        board.spaces[space.position - 1] =
                            new Space.Utility(space.position, space.name, space.cost_amount, space.rents);
                        break;
                    case SqType.TAX:
                        board.spaces[space.position - 1] = new Space.Tax(space.position, space.name, space.cost_amount);
                        break;
                    case SqType.POTLUCK:
                        board.spaces[space.position - 1] = new Space.PotLuck(space.position, space.name);
                        break;
                    case SqType.CHANCE:
                        board.spaces[space.position - 1] = new Space.Chance(space.position, space.name);
                        break;
                    case SqType.GO:
                        board.spaces[space.position - 1] = new Space.Go(space.position, space.name, space.cost_amount);
                        break;
                    case SqType.PARKING:
                        board.spaces[space.position - 1] = new Space.FreeParking(space.position, space.name);
                        break;
                    case SqType.GOTOJAIL:
                        board.spaces[space.position - 1] = new Space.GoToJail(space.position, space.name);
                        break;
                    case SqType.JAILVISIT:
                        board.spaces[space.position - 1] = new Space.VisitJail(space.position, space.name);
                        break;
                }
            }
            return board;
        }
    }
}