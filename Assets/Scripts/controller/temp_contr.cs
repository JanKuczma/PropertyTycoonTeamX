using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// enum for keeping track of the turnstate state
public enum TurnState {DICEROLL, PIECEMOVE, ACTION, END}
/*
    it's just temporary script to test all MonoBehaviour Scripts together
*/
public class temp_contr : MonoBehaviour
{
    //game elements
    Board board;
    DiceContainer dice;
    Dictionary<Token,Piece> pieces;
    Dictionary<int,Token> players;
    // bits needed to run the turns
    Vector3 cam_pos_top;    // top cam position
    int current_player;
    Token current;
    TurnState state;
    //init lists
    void Awake()
    {
        players = new Dictionary<int, Token>();
        pieces = new Dictionary<Token, Piece>();
    }
    void Start()
    {
        //create board and dice
        board = Board.Create(transform);
        dice = DiceContainer.Create(transform);
        board.initSquare(SqType.GO,1);
        board.initSquare(SqType.PROPERTY,2);
        board.initSquare(SqType.POTLUCK,3);
        board.initSquare(SqType.PROPERTY,4);
        board.initSquare(SqType.INCOMETAX,5);
        board.initSquare(SqType.STATION,6);
        board.initSquare(SqType.PROPERTY,7);
        board.initSquare(SqType.CHANCE1,8);
        board.initSquare(SqType.PROPERTY,9);
        board.initSquare(SqType.PROPERTY,10);
        board.initSquare(SqType.JAILVISIT,11);
        board.initSquare(SqType.PROPERTY,12);
        board.initSquare(SqType.BULB,13);
        board.initSquare(SqType.PROPERTY,14);
        board.initSquare(SqType.PROPERTY,15);
        board.initSquare(SqType.STATION,16);
        board.initSquare(SqType.PROPERTY,17);
        board.initSquare(SqType.POTLUCK,18);
        board.initSquare(SqType.PROPERTY,19);
        board.initSquare(SqType.PROPERTY,20);
        board.initSquare(SqType.PARKING,21);
        board.initSquare(SqType.PROPERTY,22);
        board.initSquare(SqType.CHANCE2,23);
        board.initSquare(SqType.PROPERTY,24);
        board.initSquare(SqType.PROPERTY,25);
        board.initSquare(SqType.STATION,26);
        board.initSquare(SqType.PROPERTY,27);
        board.initSquare(SqType.PROPERTY,28);
        board.initSquare(SqType.WATER,29);
        board.initSquare(SqType.PROPERTY,30);
        board.initSquare(SqType.GOTOJAIL,31);
        board.initSquare(SqType.PROPERTY,32);
        board.initSquare(SqType.PROPERTY,33);
        board.initSquare(SqType.POTLUCK,34);
        board.initSquare(SqType.PROPERTY,35);
        board.initSquare(SqType.STATION,36);
        board.initSquare(SqType.CHANCE3,37);
        board.initSquare(SqType.PROPERTY,38);
        board.initSquare(SqType.SUPERTAX,39);
        board.initSquare(SqType.PROPERTY,40);
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
        Cursor.SetCursor(Asset.Cursor(CursorType.FiNGER),Vector2.zero,CursorMode.Auto);
        cam_pos_top = Camera.main.transform.position;
        //set current turn state to DICEROLL
        state = TurnState.DICEROLL;
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
        //temp code to try if piece can move backwards
        if(state == TurnState.DICEROLL)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(pieces[current].goToJail());
                current_player = (current_player+1)%players.Count;
                current = players[current_player];
            }
        }
    }

    void FixedUpdate()
    {
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
            state = TurnState.DICEROLL;     // change state to initial state
        }
    }

    //temp code for camera movement
     void LateUpdate()
    {
        // simply if the current piece is moving move camera towards it, else move camera towards top position
        if(pieces[current].isMoving)
        {
            Vector3 target = pieces[current].transform.position*1.5f;
            target[1] = 7.0f;
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position,target,8.0f*Time.deltaTime);
            Vector3 lookDirection = pieces[current].transform.position - Camera.main.transform.position;
            lookDirection.Normalize();
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.LookRotation(lookDirection), 3.0f * Time.deltaTime);
        } else {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position,cam_pos_top,10.0f*Time.deltaTime);
            Vector3 lookDirection = -1.0f*Camera.main.transform.position;
            lookDirection.Normalize();
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.LookRotation(lookDirection), 4.0f * Time.deltaTime);
        }
    }
   
    public void addPlayer(Token token)
    {
        pieces.Add(token,Piece.Create(token, transform, board));
    }
}
