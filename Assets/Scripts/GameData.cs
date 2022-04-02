using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        board_model = Model.JSONData.loadBoard(Asset.board_data_json());
        opportunity_knocks = Model.JSONData.loadCardStack(Asset.opportunity_knocks_data_json());
        potluck = Model.JSONData.loadCardStack(Asset.potluck_data_json());
        players = new List<Model.Player>();
        player_throws = new Dictionary<Model.Player, int>();
        DontDestroyOnLoad(this.gameObject);
    }
    //any stuff that we want to be continuous (not affected by scene changes)
}
