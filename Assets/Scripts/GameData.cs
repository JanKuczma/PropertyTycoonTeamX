using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
/// <summary>
/// Extends <c>MonoBehaviour</c>.<br/>
/// Used to store data about the game and pass it between scenes. See <see cref="Object.DontDestroyOnLoad()"/>.<br/>
/// Used to load/save game data from/to a file <br/>
/// </summary>
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
    public float timer=-1;
    
    void Awake()
    {
        //checks if there's already object with the same tag
        GameObject[] obj = GameObject.FindGameObjectsWithTag("GameData");
        if(obj != null)
        {
            //if there's one, destroy current object
            if (obj.Length > 1)
            {
                Destroy(obj[0]);
            }
        }
        //initial values
        board_model = Model.BoardData.LoadBoard();
        opportunity_knocks = Model.CardData.loadCardStack(Asset.opportunity_knocks_data_json());
        potluck = Model.CardData.loadCardStack(Asset.potluck_data_json());
        players = new List<Model.Player>();
        player_throws = new Dictionary<Model.Player, int>();
        //prevent's the object from being destroyed when on Scene change
        DontDestroyOnLoad(this.gameObject);
    }
    /// <summary>
    /// Loads data from <c>GameDataWrapper</c> object
    /// </summary>
    /// <param name="data">Wrapped game data</param>
    public void loadData(Wrapper data)
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
        timer = data.timer;
    }
/// <summary>
/// Class used to wrap the game data into serializable class.
/// </summary>
    [System.Serializable]
        public class Wrapper {
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
        public float timer;
/// <summary>
/// Saves game data to a file
/// </summary>
/// <param name="slot_name">file name</param>
/// <param name="gameData">Wrapped game data</param>
        public static void saveGame(string slot_name, GameData gameData)
        {
            Wrapper data = new Wrapper();
            data.saveDate = System.DateTime.Now.ToString("dd MMM yyyy\nhh:mm tt");
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
            data.timer = gameData.timer;
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/" + slot_name +".save";
            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream,data);
            stream.Close();
        }
/// <summary>
/// Loads game data from a file
/// </summary>
/// <param name="slot_name">File name</param>
/// <returns>Wrapped game data</returns>
    public static Wrapper loadGame(string slot_name)
    {
        string path = Application.persistentDataPath + "/"+slot_name + ".save";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Wrapper data = formatter.Deserialize(stream) as Wrapper;

            stream.Close();
            return data;
        } else {
            return null;
        }
    }
/// <summary>
/// Deletes a file
/// </summary>
/// <param name="slot_name">File name</param>
    public static void deleteGame(string slot_name)
    {
        File.Delete(Application.persistentDataPath+"/"+slot_name+".save");
    }
    }
}
