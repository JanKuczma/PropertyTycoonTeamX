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
    public Vector3 v = Vector3.one;
    
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
        board_model = Model.JSONData.loadBoard(Asset.board_data_json());
        opportunity_knocks = Model.JSONData.loadCardStack(Asset.opportunity_knocks_data_json());
        potluck = Model.JSONData.loadCardStack(Asset.potluck_data_json());
        players = new List<Model.Player>();
        player_throws = new Dictionary<Model.Player, int>();
        DontDestroyOnLoad(this.gameObject);
    }
    
    public void saveGame(int current_player, TurnState turnState, GameState gameState, bool double_rolled, int double_count, bool passed_go, int steps, bool tabs_set)
    {
        GameDataWrapper data = new GameDataWrapper();
        data.starWarsTheme = starWarsTheme;
        data.turboGame = turboGame;
        data.board_model = board_model;
        data.opportunity_knocks = opportunity_knocks;
        data.potluck = potluck;
        data.players = players;
        data.player_throws = player_throws;
        data.current_player = current_player;
        data.turnState = turnState;
        data.gameState = gameState;
        data.double_rolled = double_rolled;
        data.double_count = double_count;
        data.passed_go = passed_go;
        data.steps = steps;
        data.tabs_set = tabs_set;
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/slot1.save";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream,data);
        stream.Close();
    }

    public void loadGame()
    {
        string path = Application.persistentDataPath + "/slot1.save";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameDataWrapper data = formatter.Deserialize(stream) as GameDataWrapper;

            stream.Close();
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
        } else {
            Debug.Log("file not found");
        }
    }
    [System.Serializable]
    class GameDataWrapper {
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
    }
}
