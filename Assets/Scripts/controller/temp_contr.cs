using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    Model2.Board board_model;
    Model2.CardStack opportunity_knocks;
    Model2.CardStack potluck;
    View.DiceContainer dice;
    Dictionary<Token,View.Piece> pieces;
    Dictionary<int,Token> players;
    Vector3 cam_pos_top;    // top cam position
    // bits needed to run the turns
    int current_player;
    Token current;
    TurnState state;
    List<int> no = new List<int> {0,1,2,3,4,5};
    //init lists
    void Awake()
    {
        players = new Dictionary<int, Token>();
        pieces = new Dictionary<Token, View.Piece>();
    }
    void Start()
    {
        //load data
        board_model = Model2.JSONData.loadBoard(Asset.board_data_json());
        opportunity_knocks = Model2.JSONData.loadCardStack(Asset.opportunity_knocks_data_json());
        potluck = Model2.JSONData.loadCardStack(Asset.potluck_data_json());
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
            if(dice.start_roll) {
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
                } else {                        // else start moving piece and change the turn state
                    StartCoroutine(pieces[current].move(steps));
                    state = TurnState.PIECEMOVE;
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
            target[1] = 1.7f*dice.av_distance()/Mathf.Tan(Mathf.Deg2Rad*(Camera.main.fieldOfView/2));//Mathf.Tan(Mathf.Deg2Rad*(Camera.main.fieldOfView/2)) depends only on fieldview angle so it is pretty much constatnt, could be set as constract in terms of optimisation
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
}
