using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// enum for keeping track of the turnstate state
public enum TurnState {BEGIN,DICEROLL, PIECEMOVE, ACTION, END}
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
    Vector3 cam_pos_top;    // top cam position
    // bits needed to run the turns
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
        board.initSquare(SqType.PROPERTY,2,"THE OLD CREEK","60",((int)Group.BROWN));
        board.initSquare(SqType.POTLUCK,3,"POT LUCK");
        board.initSquare(SqType.PROPERTY,4,"GANGSTER PARADISE","60",((int)Group.BROWN));
        board.initSquare(SqType.INCOMETAX,5,"INCOME TAX","100");
        board.initSquare(SqType.STATION,6,"BRIGHTON STATION","200");
        board.initSquare(SqType.PROPERTY,7,"THE ANGELS DELIGHT","60",((int)Group.BLUE));
        board.initSquare(SqType.CHANCE1,8,"OPPORTUNITY KNOCKS");
        board.initSquare(SqType.PROPERTY,9,"POTTER AVENUE","100",((int)Group.BLUE));
        board.initSquare(SqType.PROPERTY,10,"GRANGER DRIVE","100",((int)Group.BLUE));
        board.initSquare(SqType.JAILVISIT,11);
        board.initSquare(SqType.PROPERTY,12,"SKYWALKER DRIVE","140",((int)Group.PURPLE));
        board.initSquare(SqType.BULB,13,"TESLA POWER CO","150");
        board.initSquare(SqType.PROPERTY,14,"WOOKIE HOLE","140",((int)Group.PURPLE));
        board.initSquare(SqType.PROPERTY,15,"REY LANE","160",((int)Group.PURPLE));
        board.initSquare(SqType.STATION,16,"HOVE STATION","200");
        board.initSquare(SqType.PROPERTY,17,"BISHOP DRIVE","180",((int)Group.ORANGE));
        board.initSquare(SqType.POTLUCK,18,"POT LUCK");
        board.initSquare(SqType.PROPERTY,19,"DUNHAM STREET","180",((int)Group.ORANGE));
        board.initSquare(SqType.PROPERTY,20,"BROYLES LANE","200",((int)Group.ORANGE));
        board.initSquare(SqType.PARKING,21);
        board.initSquare(SqType.PROPERTY,22,"YUE FEI SQUARE","220",((int)Group.RED));
        board.initSquare(SqType.CHANCE2,23,"OPPORTUNITY KNOCKS");
        board.initSquare(SqType.PROPERTY,24,"MILAN ROGUE","220",((int)Group.RED));
        board.initSquare(SqType.PROPERTY,25,"HAN XIN GARDENS","240",((int)Group.RED));
        board.initSquare(SqType.STATION,26,"FALMER STATION","200");
        board.initSquare(SqType.PROPERTY,27,"SHATNER CLOSE","260",((int)Group.YELLOW));
        board.initSquare(SqType.PROPERTY,28,"PICARD AVENUE","260",((int)Group.YELLOW));
        board.initSquare(SqType.WATER,29,"EDISON WATER","150");
        board.initSquare(SqType.PROPERTY,30,"CRUSHER CREEK","280",((int)Group.YELLOW));
        board.initSquare(SqType.GOTOJAIL,31);
        board.initSquare(SqType.PROPERTY,32,"SIRAT MEWS","300",((int)Group.GREEN));
        board.initSquare(SqType.PROPERTY,33,"GENGHIS CRESCENT","300",((int)Group.GREEN));
        board.initSquare(SqType.POTLUCK,34,"POT LUCK");
        board.initSquare(SqType.PROPERTY,35,"IBIS CLOSE","320",((int)Group.GREEN));
        board.initSquare(SqType.STATION,36,"PORTSLADE STATION","200");
        board.initSquare(SqType.CHANCE3,37,"OPPORTUNITY KNOCKS");
        board.initSquare(SqType.PROPERTY,38,"JAMES WEBB WAY","350",((int)Group.DEEPBLUE));
        board.initSquare(SqType.SUPERTAX,39,"SUPER TAX","100");
        board.initSquare(SqType.PROPERTY,40,"TURING HEIGHTS","400",((int)Group.DEEPBLUE));
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
                ((PropertySquare)board.squares[1]).addHouse();
            }
        if(Input.GetKeyDown("r"))
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
                    state = TurnState.BEGIN;
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
            target[1] = board.transform.position.y+7.0f;
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
