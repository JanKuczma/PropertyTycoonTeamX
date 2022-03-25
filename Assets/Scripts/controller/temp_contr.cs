using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using View;
using Space = Model.Space;

// enum for keeping track of the turnstate state
// just chucking a comment in here, testing git stuff :) (RD)
public enum TurnState {BEGIN,DICEROLL, PIECEMOVE, PERFORMACTION,MANAGEPROPERTIES,END}
public enum GameState {PLAYERTURN,PAUSE,ORDERINGPHASE,WINNERCELEBRATION}
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
    Dictionary<Model.Player,View.Piece> pieces; // dict for Piece objects where the keys values are references to Model.Player obj
    //players
    List<Model.Player> players;     // players list in some random order, it'll be ordered in GameState.ORDERINGPHASE
    Dictionary<Model.Player, int> player_throws; //holds throw values when deciding player order *JK: also for other stuff (for Utility Sqpace - get as much money as u throw X 4)
    int current_player;         // incremented every turn, holds the index of the current player (ordered in List players)
    int double_count = 0;           // incremented when player rolls a double, reset back to zero when current player is updated 
    // bits needed to manage game and turns
    TurnState turnState;
    GameState gameState;
    bool double_rolled = false; // use this to keep track of whether player just rolled a double
    //HUD
    public View.HUD hud; 
    //other
    Vector3 cam_pos_top;    // top cam position
    bool tabs_set;
    public GameObject invisibleWall;

    void Awake()
    {
        players = GameObject.Find("PersistentObject").GetComponent<PermObject>().players;
        player_throws = new Dictionary<Model.Player, int>();    
        pieces = new Dictionary<Model.Player, View.Piece>();
        tabs_set = false;
        invisibleWall.SetActive(false);
    }
    void Start()
    {
        //load data (to be changed for XLSX in near future)
        board_model = Model.JSONData.loadBoard(Asset.board_data_json());
        opportunity_knocks = Model.JSONData.loadCardStack(Asset.opportunity_knocks_data_json());
        potluck = Model.JSONData.loadCardStack(Asset.potluck_data_json());
        //create board with card stacks and dice
        board_view = View.Board.Create(transform,board_model);
        dice = View.DiceContainer.Create(transform);
        //create playerTabs
        hud.Create_player_tabs(players,board_model);
        //craete pieces
        foreach(Model.Player player in players)
        {
            pieces.Add(player,View.Piece.Create(player.token,transform,board_view));
        }
        //setup finger cursor and get init cemara pos (top pos)
        Cursor.SetCursor(Asset.Cursor(CursorType.FINGER),Vector2.zero,CursorMode.Auto);
        cam_pos_top = Camera.main.transform.position;
        //set current turn state to DICEROLL and gameState to ORDERINGPHASE (subject to change if we want to continue game from a saved game)
        gameState = GameState.ORDERINGPHASE;
        turnState = TurnState.BEGIN;
        current_player = 0;
    }

    void Update()
    {
        // temp code for speeding up piece movement
        if(turnState == TurnState.PIECEMOVE)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                pieces[players[current_player]].speedUp();
            }
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            board_view.squares[1].GetComponent<View.PropertySquare>().showRibbon(Color.blue);
            board_view.squares[1].GetComponent<View.PropertySquare>().showRibbon(Color.red);
            board_view.squares[5].GetComponent<View.UtilitySquare>().showRibbon(Color.magenta);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            View.OkPopUp.Create(hud.transform, Asset.okPopup,"testing");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ((View.PropertySquare)board_view.squares[1]).addHouse();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ((View.PropertySquare)board_view.squares[1]).removeHouse();
        }
    }

    void FixedUpdate()
    {
        if(gameState == GameState.ORDERINGPHASE)    //if game state
        {
            decidePlayerOrder();
        }
        if(gameState == GameState.PLAYERTURN)
        {
            if(turnState == TurnState.BEGIN)
            {
                if(!tabs_set)
                {
                    hud.set_current_player_tab(players[current_player]);
                    tabs_set = true;
                }
                if(dice.start_roll) 
                {
                    turnState = TurnState.DICEROLL;
                    invisibleWall.SetActive(true);
                }
            }
            if(turnState == TurnState.DICEROLL) // turn begins
            {
                if(!dice.areRolling())  // if dice are not rolling anymore
                {
                    invisibleWall.SetActive(false);
                    int steps = dice.get_result();  // get the result
                    double_rolled = dice.is_double(); // return whether double was rolled
                    if(steps < 0)                   // if result is negative (dice are stuck)
                    {                               // reset the dice
                        dice.reset();
                        turnState = TurnState.BEGIN;
                    } else     // if not in the ordering phase of the game, move Token and continue with game
                    {
                        // else start moving piece and change the turn state
                        StartCoroutine(pieces[players[current_player]].move(steps));
                        turnState = TurnState.PIECEMOVE;
                    }
                }
            }
            else if(turnState == TurnState.PIECEMOVE)
            {
                if(!pieces[players[current_player]].isMoving)   //if piece is not moving anymore
                {
                    turnState = TurnState.PERFORMACTION;   // change turn state to action
                    PerformAction();
                }
            }
            else if(turnState == TurnState.PERFORMACTION)  // ACTION state (buy property, pay rent etc...)
            {
                // when PopUp is closed the `trunState` is changed to MANAGEPROPERTIES
                if(hud.currentPopUp == null)
                {
                    turnState = TurnState.MANAGEPROPERTIES;
                }
            }
            else if(turnState == TurnState.MANAGEPROPERTIES)  // (manage your properties, check other players' properties)
            {
                hud.FinishTurnButton.gameObject.SetActive(true);
                // when player presses FINISH TURN button the `turnState` is changed to END
            }
            else if(turnState == TurnState.END)     // END state, when player finished his turn
            {
                hud.FinishTurnButton.gameObject.SetActive(false);
                dice.reset();                   // reset dice
                if (double_rolled)              // if double has been rolled, increase double count by 1 and maintain current player
                {
                    View.OkPopUp.Create(hud.transform, Asset.okPopup, players[current_player].name + "rolled a double, have another turn!");
                    double_count++;
                    double_rolled = false;      // reset double check
                    if (double_count == 3)      // if 3 doubles in a row, send player to jail and update current player
                    {
                        View.OkPopUp.Create(hud.transform, Asset.okPopup,"Three doubles in a row? You must be cheating… go to jail!");
                        // send player to jail
                        nextPlayer();
                    }
                }
                else
                {
                    nextPlayer();
                }
                tabs_set = false;
                turnState = TurnState.BEGIN;     // change state to initial state
            }
        }
        
    }

    //temp code for camera movement
     void LateUpdate()
    {
        if(turnState == TurnState.PIECEMOVE)
        {
            moveCameraTowardsPiece(pieces[players[current_player]]);
        }
        else if(turnState == TurnState.DICEROLL)
        {
            moveCameraTowardsDice(dice);
        } else {
            moveCameraTopView();
        }
    }

     public void decidePlayerOrder()
     {
         if(!tabs_set)
            {
                hud.set_current_player_tab(players[current_player]);
                tabs_set = true;
            }
            if(dice.start_roll)     // this bit is so camera knows when to follow dice
            {
                invisibleWall.SetActive(true);
                turnState = TurnState.DICEROLL;
            }
            if(!dice.areRolling())  //when dice stopped rolling
            {
                int steps = dice.get_result();  // get the result
                if(steps < 0)                   // if result is negative (dice are stuck)
                {                                // reset the dice
                    Debug.Log("Dice stuck. Please roll again!");
                    dice.reset();
                } else {    // if not in the ordering phase of the game, move Token and continue with game
                    invisibleWall.SetActive(false);
                    Debug.Log("Player " + current_player + " rolled a " + steps);
                    if (player_throws.ContainsValue(steps))             // force re-roll if player has already rolled the same number
                    {
                        Debug.Log("Someone has already rolled "+ steps +". Please roll again!");
                        dice.reset();
                    } else {
                        player_throws.Add(players[current_player],steps);        // log value that player rolled
                        dice.reset();                   // reset dice
                        current_player = current_player+1;       // update turn state so that it becomes next player's turn
                        tabs_set = false;
                        if (current_player == players.Count)            // check whether every player has rolled the dice
                        {
                            var player_throws_sorted =
                                from entry in player_throws orderby entry.Value descending select entry;    // this line sorts the dictionary in descending order by each pair's Value
                            Dictionary<Model.Player,int> sorted_dict = player_throws_sorted.ToDictionary(pair => pair.Key, pair => pair.Value); // casts output from previous line as a Dictionary
                            Debug.Log("**BEFORE ORDERING**"); 
                            for (int i = 0; i < players.Count; i++)
                            {
                                Debug.Log("Key: " + i + " || Token: " + players[i].token.ToString()); // prints new list of players and their selected Token
                            }

                            players = sorted_dict.Keys.ToList<Model.Player>();

                            Debug.Log("**AFTER ORDERING**"); 
                            for (int i = 0; i < players.Count; i++)
                            {
                                Debug.Log("after Key: " + i + " || Token: " + players[i].token.ToString()); // prints new list of players and their selected Token
                            }

                            current_player = 0;             // game starts with player first on ordered list
                            hud.sort_tabs(players);
                            turnState = TurnState.BEGIN;
                            gameState = GameState.PLAYERTURN;
                        }
                    }
                }
                turnState = TurnState.BEGIN;    // this bit is so camera comes back to top position
            }
     }

     void nextPlayer()
     {
         double_rolled = false;  // reset double check
         double_count = 0;       // reset double count
         current_player = (current_player + 1) % players.Count;
         View.OkPopUp.Create(hud.transform, Asset.okPopup, players[current_player].name + ", it's your turn!");
     }
     
    void PerformAction()
    {
        Debug.Log("here");
        int current_square = pieces[players[current_player]].GetCurrentSquare();    // get location of current player piece on board
        Debug.Log("current square integer: " + current_square);
        Space current_space = board_model.spaces[current_square];                   // get Space from location on board
        Debug.Log("current space: " + current_space.name);
        SqType current_space_type = current_space.type;                             // get SqType from Space
        Debug.Log("current space type: " + current_space_type.GetType().ToString());

        if (current_space_type == SqType.CHANCE)                                    // first two if statements check whether square is a "take a card" square
        {
            Model.Card card_taken = opportunity_knocks.cards[0];
            performCardAction(card_taken, players[current_player]);                 // if so, call performCardAction()
        }

        if (current_space_type == SqType.POTLUCK)
        {
            Model.Card card_taken = potluck.cards[0];
            performCardAction(card_taken, players[current_player]);                 // NOTE: currently when a card is taken, it is not placed at the bottom nor is the deck reshuffled
        }

        switch (current_space_type)
        {
            case SqType.GO:
            {
                hud.currentPopUp = OkPopUp.Create(hud.transform, Asset.okPopup, players[current_player].name + "passed GO, collect £200!");
                //give player £200
                //update in player info that this player has passed GO
                break;
            }
            case SqType.JAILVISIT:
            {
                
                break;
            }
            case SqType.PARKING:
            {
                hud.currentPopUp = OkPopUp.Create(hud.transform, Asset.okPopup, players[current_player].name + "landed on Free Parking. Collect all those juicy fines!");
                //reset FREE PARKING balance to zero
                //give player whatever the balance in FREE PARKING
                break;
            }
            case SqType.GOTOJAIL:
            {
                hud.currentPopUp = OkPopUp.Create(hud.transform, Asset.okPopup, players[current_player].name + "broke the law! They must go straight to jail!");
                //player token is moved to JAIL square
                //player hud icon is updated
                //jail cell animation on board
                break;
            }
            case SqType.PROPERTY:
            {
                hud.currentPopUp = OkPopUp.Create(hud.transform, Asset.BuyPropertyPopup, players[current_player].name + " do you wish to purchase this property?");    // new pop up prefabs needed for this popup
                PurchasableCard c = PropertyCard.Create((Model.Space.Property)current_space,hud.currentPopUp.transform);
                c.GetComponent<RectTransform>().anchoredPosition = new Vector2(220,0);
                c.gameObject.SetActive(true);
                break;
            }
            case SqType.STATION:
            {
                hud.currentPopUp = OkPopUp.Create(hud.transform, Asset.BuyPropertyPopup, players[current_player].name + "do you wish to purchase this station?");      // new pop up prefabs needed for this popup
                PurchasableCard c = StationCard.Create((Model.Space.Station)current_space,hud.currentPopUp.transform);
                c.GetComponent<RectTransform>().anchoredPosition = new Vector2(220,0);
                c.gameObject.SetActive(true);
                break;
            }
            case SqType.UTILITY:
            {
                hud.currentPopUp = OkPopUp.Create(hud.transform, Asset.BuyPropertyPopup, players[current_player].name + "do you wish to purchase this utility company?");      // new pop up prefabs needed for this popup
                PurchasableCard c = UtilityCard.Create((Model.Space.Utility)current_space,hud.currentPopUp.transform);
                c.GetComponent<RectTransform>().anchoredPosition = new Vector2(220,0);
                c.gameObject.SetActive(true);
                break;
            }
            case SqType.TAX:
            {
                hud.currentPopUp = OkPopUp.Create(hud.transform, Asset.okPopup, players[current_player].name + "misfiled their tax returns, pay HMRC a SUPER TAX!");
                break;
            }
        }
    }
    
    public void performCardAction(Model.Card card, Model.Player player)
    {
        OkPopUp.Create(hud.transform, Asset.okPopup,players[current_player].name + " take a card!");
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

    public void finishTurn()
    {
        turnState = TurnState.END;
    }
    //Camera movement
    public void moveCameraLeft()
    {
        float new_x = cam_pos_top.z;
        float new_z = cam_pos_top.x*(-1);
        cam_pos_top = new Vector3(new_x,cam_pos_top.y,new_z);
    }
    public void moveCameraRight()
    {
        float new_z = cam_pos_top.x;
        float new_x = cam_pos_top.z*(-1);
        cam_pos_top = new Vector3(new_x,cam_pos_top.y,new_z);
    }
    public void moveCameraTowardsPiece(View.Piece piece)
    {
        Vector3 target = piece.transform.position*1.5f;
        target[1] = board_view.transform.position.y+7.0f;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,target,Time.deltaTime*2f);
        Vector3 lookDirection = piece.transform.position - Camera.main.transform.position;
        lookDirection.Normalize();
        Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.LookRotation(lookDirection),6f*Time.deltaTime);
    }
    public void moveCameraTowardsDice(View.DiceContainer diceContainer)
    {
        Vector3 target = diceContainer.position();
        //Mathf.Tan(Mathf.Deg2Rad*(Camera.main.fieldOfView/2)) depends only on fieldview angle so it is pretty much constatnt, could be set as constant in terms of optimisation
        target[1] = (diceContainer.transform.localScale.x + diceContainer.av_distance())/Mathf.Tan(Mathf.Deg2Rad*(Camera.main.fieldOfView/2));
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,target,Time.deltaTime*2f);
        Vector3 lookDirection = diceContainer.position() - Camera.main.transform.position;
        lookDirection.Normalize();
        Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.LookRotation(lookDirection),6f*Time.deltaTime);
    }
    public void moveCameraTopView()
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,cam_pos_top,Time.deltaTime*3f);
        Vector3 lookDirection = -1.0f*Camera.main.transform.position + Vector3.down*6; // 6 is how much camera is rotated down direction xD
        lookDirection.Normalize();
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.LookRotation(lookDirection),6f*Time.deltaTime);
    }
        
}
