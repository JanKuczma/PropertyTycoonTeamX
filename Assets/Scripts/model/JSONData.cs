using UnityEngine;
using System.Collections.Generic;
using System.Linq;
namespace Model{
public static class JSONData
{
    [System.Serializable]
    private class BoardWrapper
    {
    [SerializeField] public SpaceWrapper[] spaces = new SpaceWrapper[40];
    }

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

    [System.Serializable]
    private class CardStackWrapper
    {
        public List<CardWrapper> cards = new List<CardWrapper>();
    }

    [System.Serializable]
    private class CardWrapper
    {
        public string description;
        public string action;

        public List<string> keys;
        public List<int> values;
    }

    public static void saveCardStack(CardStack cards,string filename = "custom_cardstack.json")
    {
        string cards_json = JsonUtility.ToJson(stackToWrapper(cards),true);
        //System.IO.File.WriteAllText(Application.persistentDataPath + filename+".json",cards_json);
        System.IO.File.WriteAllText(filename,cards_json);
    }
    public static CardStack loadCardStack(string json_format)
    {

        CardStackWrapper cards = JsonUtility.FromJson<CardStackWrapper>(json_format);
        return wrapperToStack(cards);
    }
    public static CardStack loadCardStackFromFile(string file_name)
    {
        //CardStack cards = JsonUtility.FromJson<CardStack>(System.IO.File.ReadAllText(Application.persistentDataPath + file_path +".json"));
        CardStackWrapper cards = JsonUtility.FromJson<CardStackWrapper>(System.IO.File.ReadAllText(file_name));
        return wrapperToStack(cards);
    }
    public static Board loadBoard(string json_format)
    {
        BoardWrapper boardData = JsonUtility.FromJson<BoardWrapper>(json_format);
        Board board = new Board();
        foreach(SpaceWrapper sD in boardData.spaces)
        {
            SqType type = (SqType)System.Enum.Parse(typeof(SqType),sD.type_action);
            switch(type)
            {
                case SqType.PROPERTY:
                board.spaces[sD.position-1] = new Space.Property(sD.position,sD.name,sD.cost_amount,(Group)System.Enum.Parse(typeof(Group),sD.group),sD.rents,sD.house_cost,sD.hotel_cost);
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

    public static void saveBoard(Board board,string filename = "custom_board")
    {
        BoardWrapper boardData = new BoardWrapper();
        foreach(Space sp in board.spaces)
        {
            boardData.spaces[sp.position-1] = spaceToWrapper(sp);
        }
        //System.IO.File.WriteAllText(Application.persistentDataPath + filename+".json",JsonUtility.ToJson(boardData,true));
        System.IO.File.WriteAllText(filename+".json",JsonUtility.ToJson(boardData,true));
    }

    public static Board loadBoardFromFile(string filename = "custom_board")
    {
        string json_format = System.IO.File.ReadAllText(Application.persistentDataPath + filename+".json");
        return JSONData.loadBoard(json_format);
    }

    private static SpaceWrapper spaceToWrapper(Space space)
    {
        SpaceWrapper spData = new SpaceWrapper();
        spData.position = space.position;
        spData.name = space.name;
        spData.type_action = space.type.ToString();
        switch(space.type)
        {
            case SqType.PROPERTY:
            spData.cost_amount = ((Space.Property)space).cost;
            spData.group = ((Space.Property)space).group.ToString();
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

    private static CardWrapper cardToWrapper(Card card)
    {
        CardWrapper cardData = new CardWrapper();
        cardData.description = card.description;
        cardData.action = card.action.ToString();
        if(card.kwargs != null)
        {
            cardData.keys = card.kwargs.Keys.ToList();
            cardData.values = card.kwargs.Values.ToList();
        }
        return cardData;
    }

    private static CardStackWrapper stackToWrapper(CardStack stack)
    {
        CardStackWrapper stackData = new CardStackWrapper();
        foreach(Card card in stack.cards)
        {
            stackData.cards.Add(cardToWrapper(card));
        }
        return stackData;
    }
    private static Card wrapperToCard(CardWrapper data)
    {
        Dictionary<string,int> dict = new Dictionary<string, int>();
        for(int i = 0 ; i < data.keys.Count ;i++)
        {
            dict.Add(data.keys[i],data.values[i]);
        }
        Card card = new Card(data.description,(CardAction)System.Enum.Parse(typeof(CardAction),data.action),dict);
        return card;
    }

    private static CardStack wrapperToStack(CardStackWrapper data)
    {
        CardStack stack = new CardStack();
        foreach(CardWrapper cdata in data.cards)
        {
            stack.cards.Add(wrapperToCard(cdata));
        }
        return stack;
    }
}
}