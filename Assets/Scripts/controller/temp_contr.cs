using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// enum for keeping track of the turnstate state
public enum TurnState {DICEROLL, PIECEMOVE, ACTION, END}
public enum Token {CAT=0 ,BOOT=1,IRON=2,SHIP=3,HATSTAND=4,SMARTPHONE=5}
public class temp_contr : MonoBehaviour
{
    public GameObject   CatPrefab, BootPrefab, IronPrefab,
                        ShipPrefab, HatStandPrefab, SmartphonePrefab;
    public GameObject DicePrefab;
    public GameObject BoardPrefab;
    public Texture2D pointing_hand; // cursor
    private Board board;
    private DiceContainer dice;

    Dictionary<Token,PieceBehaviour> pieces;
    Dictionary<int,Token> players;
    Vector3 cam_pos_top;    // top cam position
    int current_player;
    Token current;
    TurnState state;

    void Awake()
    {
        players = new Dictionary<int, Token>();
        pieces = new Dictionary<Token, PieceBehaviour>();
    }
    void Start()
    {
        board = Instantiate(BoardPrefab,transform).GetComponent<Board>();
        dice = Instantiate(DicePrefab,transform).GetComponent<DiceContainer>();
        board.initGo();
        board.initProperty(2,"","","");
        board.initPotLuck(3);
        board.initProperty(4,"","","");
        board.initIncomeTax(5,"");
        board.initStation(6,"","");
        board.initProperty(7,"","","");
        board.initChance1(8);
        board.initProperty(9,"","","");
        board.initProperty(10,"","","");
        board.initJailVisit();
        board.initProperty(12,"","","");
        board.initBulb(13,"","");
        board.initProperty(14,"","","");
        board.initProperty(15,"","","");
        board.initStation(16,"","");
        board.initProperty(17,"","","");
        board.initPotLuck(18);
        board.initProperty(19,"","","");
        board.initProperty(20,"","","");
        board.initParking();
        board.initProperty(22,"","","");
        board.initChance2(23);
        board.initProperty(24,"","","");
        board.initProperty(25,"","","");
        board.initStation(26,"","");
        board.initProperty(27,"","","");
        board.initProperty(28,"","","");
        board.initWater(29,"","");
        board.initProperty(30,"","","");
        board.initGoToJail();
        board.initProperty(32,"","","");
        board.initProperty(33,"","","");
        board.initPotLuck(34);
        board.initProperty(35,"","","");
        board.initStation(36,"","");
        board.initChance3(37);
        board.initProperty(38,"","","");
        board.initSuperTax(39,"");
        board.initProperty(40,"","","");
        addPiece(Token.CAT);
        players.Add(0,Token.CAT);
        addPiece(Token.SHIP);
        players.Add(1,Token.SHIP);
        addPiece(Token.BOOT);
        players.Add(2,Token.BOOT);
        current_player = 0;
        current = players[current_player];

        Cursor.SetCursor(pointing_hand,Vector2.zero,CursorMode.Auto);
        cam_pos_top = Camera.main.transform.position;
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
        if(state == TurnState.DICEROLL)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(pieces[current].move(-3));
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

    public void addPiece(Token token)
    {
        GameObject tmp =    token == Token.CAT ? CatPrefab:
                            token == Token.BOOT ? BootPrefab :
                            token == Token.IRON ? IronPrefab :
                            token == Token.HATSTAND ? HatStandPrefab :
                            token == Token.SHIP ? ShipPrefab :
                            SmartphonePrefab ;
        pieces.Add(token,Instantiate(tmp,transform).GetComponent<PieceBehaviour>());
        pieces[token].assignBoard(board);
        pieces[token].transform.localPosition = new Vector3(0,0.1f,0);
        pieces[token].moveInstant(0);
    }
}
