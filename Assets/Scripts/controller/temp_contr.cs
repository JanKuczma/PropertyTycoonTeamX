using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// enum for keeping track of the turnstate state
// just chucking a comment in here, testing git stuff :) (RD)
public enum TurnState {BEGIN,DICEROLL, PIECEMOVE, ACTION, END}
/*
    it's just temporary script to test all MonoBehaviour Scripts together
*/
public class temp_contr : MonoBehaviour
{
    //game elements
    View.Board board_view;
    Model.Board board_model;
    Model.CardStack opportunity_knocks;
    Model.CardStack potluck;
    View.DiceContainer dice;
    Dictionary<Token,View.Piece> pieces;
    Dictionary<int,Token> players; //main list of players (order of dictionary relates to player order, not the Key)
    Dictionary<int, Token> players_ordered; //uses players and player_throws to create ordered dict of <player_index, Token>
    Dictionary<int, int> player_throws; //holds throw values when deciding player order
    Vector3 cam_pos_top;    // top cam position
    // bits needed to run the turns
    int current_player;
    Token current;
    TurnState state;
    bool ordering_phase = true; //set to false after player order has been decided
    List<int> no = new List<int> {0,1,2,3,4,5};
    //init lists
    void Awake()
    {
        players = new Dictionary<int, Token>();
        if (ordering_phase)
        {
            player_throws = new Dictionary<int, int>();    
        }
        pieces = new Dictionary<Token, View.Piece>();
    }
    void Start()
    {
        //load data
        board_model = Model.JSONData.loadBoard(Asset.board_data_json());
        opportunity_knocks = Model.JSONData.loadCardStack(Asset.opportunity_knocks_data_json());
        potluck = Model.JSONData.loadCardStack(Asset.potluck_data_json());
        //create board with card stacks and dice
        board_view = View.Board.Create(transform,board_model);
        dice = View.DiceContainer.Create(transform);
        //add players: player<int,token> dict, pieces<token,piece> dict
        addPlayer(Token.CAT);
        players.Add(0,Token.CAT);
        addPlayer(Token.SHIP);
        players.Add(1,Token.SHIP);
        addPlayer(Token.BOOT);
        players.Add(2,Token.BOOT);
        addPlayer(Token.IRON);
        players.Add(3,Token.IRON);
        addPlayer(Token.HATSTAND);
        players.Add(4,Token.HATSTAND);
        addPlayer(Token.SMARTPHONE);
        players.Add(5,Token.SMARTPHONE);
        current_player = 0;
        current = players[current_player];
        //setup finger cursor and get init cemara pos (top pos)
        Cursor.SetCursor(Asset.Cursor(CursorType.FINGER),Vector2.zero,CursorMode.Auto);
        cam_pos_top = Camera.main.transform.position;
        //set current turn state to DICEROLL
        state = TurnState.BEGIN;
    }

    void Update()
    {
        // temp code for speeding up piece movement
        if(state == TurnState.PIECEMOVE)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                pieces[current].speedUp();
            }
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(pieces[current].goToJail());
            current_player = (current_player+1)%players.Count;
            current = players[current_player];
        }
    }

    void FixedUpdate()
    {
        if(state == TurnState.BEGIN)
        {
            if(dice.start_roll) 
            {
                state = TurnState.DICEROLL;
            }
        }
        if(state == TurnState.DICEROLL) // turn begins
        {
            if(!dice.areRolling())  // if dice are not rolling anymore
            {
                int steps = dice.get_result();  // get the result
                if(steps < 0)                   // if result is negative (dice are stuck)
                {                               // reset the dice
                    dice.reset();
                    state = TurnState.BEGIN;
                } else if (!ordering_phase)     // if not in the ordering phase of the game, move Token and continue with game
                {
                    // else start moving piece and change the turn state
                    StartCoroutine(pieces[current].move(steps));
                    state = TurnState.PIECEMOVE;
                }
                else                            // if still in ordering phase, continue with ordering logic
                {
                    Debug.Log("Player " + current_player + " rolled a " + steps);
                    if (player_throws.ContainsValue(steps))             // force re-roll if player has already rolled the same number
                    {
                        Debug.Log("Someone has already rolled this number. Please roll again!");
                        dice.reset();
                        state = TurnState.BEGIN;
                    } else {
                        player_throws.Add(current_player,steps);        // log value that player rolled
                        state = TurnState.END;                          // update turn state so that it becomes next player's turn
                    if (current_player == players.Count - 1)            // check whether every player has rolled the dice
                    {
                        players_ordered = new Dictionary<int, Token>(); // initialise dictionary for players ordered by their roll values
                        var player_throws_sorted =
                            from entry in player_throws orderby entry.Value descending select entry;    // this line sorts the dictionary in descending order by each pair's Value
                        Dictionary<int,int> sorted_dict = player_throws_sorted.ToDictionary(pair => pair.Key, pair => pair.Value); // casts output from previous line as a Dictionary
                        
                        int i = 0;                                      // counter for next for each loop
                        foreach (var player_index in sorted_dict.Keys)
                        {
                            players_ordered.Add(i,players[player_index]); // adds each player to a dictionary in order of roll value in format: <int new_player_index, Token player_token> (NOTE: player is given new index number which informs controller of player order)
                            i++;
                        }

                        foreach (var entry in players)
                        {
                            Debug.Log("Key: " + entry.Key + " || Token: " + entry.Value); // prints original list of players and their selected token
                        }
                        
                        players.Clear();                // clears original Dict of <player_index, Token>
                        players = players_ordered;      // replace original Dict, same format but just reordered
                        
                        foreach (var entry in players)
                        {
                            Debug.Log("Key: " + entry.Key + " || Token: " + entry.Value); // prints new list of players and their selected Token
                        }
                        ordering_phase = false;         // player order has now been initialised so the ordering_phase is over
                        current_player = -1;             // game starts with player first on ordered list
                    }
                    }
                }
            }
        }
        else if(state == TurnState.PIECEMOVE)
        {
            if(!pieces[current].isMoving)   //if piece is not moving anymore
            {
                state = TurnState.ACTION;   // change turn state to action
            }
        }
        else if(state == TurnState.ACTION)  // ACTION state (buy property, pay rent etc...)
        {
            state = TurnState.END;
        }
        else if(state == TurnState.END)     // END state, when player finished his turn
        {
            dice.reset();                   // reset dice
            current_player = (current_player+1)%players.Count;
            current = players[current_player];
            state = TurnState.BEGIN;     // change state to initial state
        }
    }

    //temp code for camera movement
     void LateUpdate()
    {
        // simply if the current piece is moving move camera towards it, else move camera towards top position
        if(state == TurnState.PIECEMOVE)
        {
            Vector3 target = pieces[current].transform.position*1.5f;
            target[1] = board_view.transform.position.y+7.0f;
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position,target,8.0f*Time.deltaTime);
            Vector3 lookDirection = pieces[current].transform.position - Camera.main.transform.position;
            lookDirection.Normalize();
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.LookRotation(lookDirection), 3.0f * Time.deltaTime);
        }
        else if(state == TurnState.DICEROLL)
        {
            Vector3 target = dice.position();
            target[1] = (dice.transform.localScale.x + dice.av_distance())/Mathf.Tan(Mathf.Deg2Rad*(Camera.main.fieldOfView/2));//Mathf.Tan(Mathf.Deg2Rad*(Camera.main.fieldOfView/2)) depends only on fieldview angle so it is pretty much constatnt, could be set as constract in terms of optimisation
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position,target,8.0f*Time.deltaTime);
            Vector3 lookDirection = dice.position() - Camera.main.transform.position;
            lookDirection.Normalize();
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.LookRotation(lookDirection), 3.0f * Time.deltaTime);
            if (target.x < -50.0)
            {
                dice.reset();
            }
        } else {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position,cam_pos_top,10.0f*Time.deltaTime);
            Vector3 lookDirection = -1.0f*Camera.main.transform.position;
            lookDirection.Normalize();
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.LookRotation(lookDirection), 4.0f * Time.deltaTime);
        }
    }
   
    public void addPlayer(Token token)
    {
        pieces.Add(token,View.Piece.Create(token, transform, board_view));
    }

    public static void performCardAction(Model.Card card, Model.Player player)
    {
        switch(card.action)
        {
            case CardAction.PAYTOBANK:
            //player.payCash(card.kwargs["amount"]);
            break;
            case CardAction.PAYTOPLAYER:
            //player.getCash(card.kwargs["amount"]);
            break;
            case CardAction.MOVEFORWARDTO:
            // int steps = (40+card.kwargs["position"]) - player.position)%40; 
            //player.move(card.kwargs["position"]);
            //StartCoroutine(pieces[player].move(steps));
            break;
            case CardAction.MOVEBACKTO:
            // int steps = -1 * ((player.position+40 - card.kwargs["position"])%40); 
            //player.move(card.kwargs["position"]);
            //StartCoroutine(pieces[player].move(steps));
            break;
            case CardAction.MOVEBACK:
            //player.move(card.kwargs["steps"]);
            //StartCoroutine(pieces[player].move(-1*steps));
            break;
            case CardAction.GOTOJAIL:
            //player.goToJail();
            //StartCoroutine(pieces[player].goToJail());
            break;
            case CardAction.BIRTHDAY:
            //foreach(Player p in players)
            //{
            //  if(p.cash < card.kwargs["amont"])
            //  {
            //      *** p player is unable to pay ***
            //  } else {
            //      p.payCash(card.kwargs["amont"],player)  
            //  }
            //}
            break;
            case CardAction.OUTOFJAIL:
            //player.outOfJailCards+=1;
            break;
            case CardAction.PAYORCHANCE:
            // bool choice = *** some choice popup window ***
            // if(choice)
            //{
            //  player.payCash(card.kwargs["amount"]);
            //  board_model.parkingFees += card.kwargs["amount"];
            //} else {
            //  player.takeCard(opportunity_knocks.pop());
            //}
            break;
            case CardAction.PAYTOPARKING:
            //  player.payCash(card.kwargs["amount"]);
            //  board_model.parkingFees += card.kwargs["amount"];
            break;
            case CardAction.REPAIRS:
            /*
                int total = 0;
                foreach(Model2.Space.Purchasable space in player.owned_spaces)
                {
                    if(space.type == SqType.PROPERTY)
                    {
                        if(((Model2.Space.Property)space).noOfHouses == 5)
                        {
                            total += card.kwargs["hotel"];
                        } else {
                            total += ((Model2.Space.Property)space).noOfHouses * card.kwargs["house"];
                        }
                    }
                }
                player.payCash(total);
            */
            break;
        }
    }
}
