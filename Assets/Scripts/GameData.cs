using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameData : MonoBehaviour
{
    //game settings
    public bool starWarsTheme = false;
    public bool turboGame = false;
    //board/card data
    public Model.Board board_model;
    public Model.CardStack opportunity_knocks;
    public Model.CardStack potluck;
    //players and player bits
    public List<Model.Player> players;
    public Dictionary<Model.Player, int> player_throws; //holds throw values when deciding player order
    public int current_player = 0;
    //turn/game state bits
    public TurnState turnState = TurnState.BEGIN;
    public GameState gameState = GameState.ORDERINGPHASE;
    public bool double_rolled = false; // use this to keep track of whether player just rolled a double
    public int double_count = 0;           // incremented when player rolls a double, reset back to zero when current player is updated 
    public bool passed_go = false; // use this to keep track if the current player can get money for passing GO
    public int steps = 0; // to pass dice result between states
    public bool tabs_set = false;
    
    void Awake()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("GameData");
        if(obj != null)
        {
            if (obj.Length > 1)
            {
                Destroy(obj[0]);
            }
        }

        if (File.Exists(Asset.custom_board_data()))
        {
            board_model = Model.BoardData.LoadCustom();
        }
        else
        {
            board_model = Model.BoardData.loadBoard(Asset.board_data_json());   
        }
        opportunity_knocks = Model.CardData.loadCardStack(Asset.opportunity_knocks_data_json());
        potluck = Model.CardData.loadCardStack(Asset.potluck_data_json());
        players = new List<Model.Player>();
        player_throws = new Dictionary<Model.Player, int>();
        DontDestroyOnLoad(this.gameObject);
    }

    public void loadData(GameDataWrapper data)
    {
        starWarsTheme = data.starWarsTheme;
        turboGame = data.turboGame;
        board_model = data.board_model;
        opportunity_knocks = data.opportunity_knocks;
        potluck = data.potluck;
        players = data.players;
        player_throws = data.player_throws;
        current_player = data.current_player;
        turnState = data.turnState;
        gameState = data.gameState;
        double_rolled = data.double_rolled;
        double_count = data.double_count;
        passed_go = data.passed_go;
        steps = data.steps;
        tabs_set = data.tabs_set;
    }

    [System.Serializable]
        public class GameDataWrapper {
        public string saveDate;
        public bool starWarsTheme;
        public bool turboGame;
        //board/card data
        public Model.Board board_model;
        public Model.CardStack opportunity_knocks;
        public Model.CardStack potluck;
        //players and player bits
        public List<Model.Player> players;
        public Dictionary<Model.Player, int> player_throws; //holds throw values when deciding player order
        public int current_player;
        //turn/game state bits
        public TurnState turnState;
        public GameState gameState;
        public bool double_rolled; // use this to keep track of whether player just rolled a double
        public int double_count;           // incremented when player rolls a double, reset back to zero when current player is updated 
        public bool passed_go; // use this to keep track if the current player can get money for passing GO
        public int steps; // to pass dice result between states
        public bool tabs_set;

        public static void saveGame(string slot_name, GameData gameData)
        {
            GameDataWrapper data = new GameDataWrapper();
            data.saveDate = System.DateTime.Today.Date.Day.ToString("D2")+"/"+System.DateTime.Today.Date.Month.ToString("D2")+"/"+System.DateTime.Today.Date.Year.ToString("D4");
            data.starWarsTheme = gameData.starWarsTheme;
            data.turboGame = gameData.turboGame;
            data.board_model = gameData.board_model;
            data.opportunity_knocks = gameData.opportunity_knocks;
            data.potluck = gameData.potluck;
            data.players = gameData.players;
            data.player_throws = gameData.player_throws;
            data.current_player = gameData.current_player;
            data.turnState = gameData.turnState;
            data.gameState = gameData.gameState;
            data.double_rolled = gameData.double_rolled;
            data.double_count = gameData.double_count;
            data.passed_go = gameData.passed_go;
            data.steps = gameData.steps;
            data.tabs_set = gameData.tabs_set;
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/" + slot_name +".save";
            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream,data);
            stream.Close();
        }

    public static GameDataWrapper loadGame(string slot_name)
    {
        string path = Application.persistentDataPath + "/"+slot_name + ".save";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameDataWrapper data = formatter.Deserialize(stream) as GameDataWrapper;

            stream.Close();
            return data;
        } else {
            return null;
        }
    }

    public static void deleteGame(string slot_name)
    {
        File.Delete(Application.persistentDataPath+"/"+slot_name+".save");
    }
    }
}
