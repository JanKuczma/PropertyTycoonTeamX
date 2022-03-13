using UnityEngine;
namespace Model2{
public static class BoardData
{
    public static Board loadBoard(string json_format)
    {
        BoardD boardData = JsonUtility.FromJson<BoardD>(json_format);
        Board board = new Board();
        foreach(SpaceData sD in boardData.spaces)
        {
            switch(sD.type_action)
            {
                case SqType.PROPERTY:
                board.spaces[sD.position-1] = new Space.Property(sD.position,sD.name,sD.cost_amount,sD.group,sD.rents,sD.house_cost,sD.hotel_cost);
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
                board.spaces[sD.position-1] = new Space.FreeParking(sD.position,sD.name);
                break;
                case SqType.JAILVISIT:
                board.spaces[sD.position-1] = new Space.FreeParking(sD.position,sD.name);
                break;
            }
        }
        return board;
    }

    public static void saveBoard(Board board,string filename = "custom_board")
    {
        BoardD boardData = new BoardD();
        foreach(Space sp in board.spaces)
        {
            boardData.spaces[sp.position-1] = spaceToData(sp);
        }
        System.IO.File.WriteAllText(Application.persistentDataPath + filename+".json",JsonUtility.ToJson(boardData,true));
    }

    public static Board loadBoardFromFile(string filename = "custom_board")
    {
        string json_format = System.IO.File.ReadAllText(Application.persistentDataPath + filename+".json");
        return BoardData.loadBoard(json_format);
    }
    [System.Serializable]
    private class BoardD
    {
        [SerializeField] public SpaceData[] spaces = new SpaceData[40];
    }

    [System.Serializable]
    private class SpaceData
    {
        public int position;
        public string name;
        public SqType type_action;
        public int cost_amount;
        public Group group;
        public int[] rents;
        public int house_cost;
        public int hotel_cost;
    }

    private static SpaceData spaceToData(Space space)
    {
        SpaceData spData = new SpaceData();
        spData.position = space.position;
        spData.name = space.name;
        spData.type_action = space.type;
        switch(space.type)
        {
            case SqType.PROPERTY:
            spData.cost_amount = ((Space.Property)space).cost;
            spData.group = ((Space.Property)space).group;
            spData.rents = ((Space.Property)space).rents;
            spData.house_cost = ((Space.Property)space).house_cost;
            spData.hotel_cost = ((Space.Property)space).hotel_cost;
            break;
            case SqType.STATION:
            spData.cost_amount = ((Space.Station)space).cost;
            spData.rents = ((Space.Station)space).rents;
            break;
            case SqType.UTILITY:
            spData.cost_amount = ((Space.Utility)space).cost;
            spData.rents = ((Space.Utility)space).rents;
            break;
            case SqType.TAX:
            spData.cost_amount = ((Space.Tax)space).amount;
            break;
            case SqType.GO:
            spData.cost_amount = ((Space.Go)space).amount;
            break;
        }
        return spData;
    }
}
}