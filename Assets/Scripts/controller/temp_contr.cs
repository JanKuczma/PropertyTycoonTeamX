using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using View;
using Space = Model.Space;

// enum for keeping track of the turnstate state
// just chucking a comment in here, testing git stuff :) (RD)
public enum TurnState {BEGIN,DICEROLL,DICEROLL_UTILITY_RENT, PIECEMOVE, JAIL_PIECE_MOVE, PERFORM_ACTION,MANAGE_PROPERTIES,END}
public enum GameState {PLAYERTURN,PAUSE,ORDERINGPHASE,WINNERCELEBRATION}
/*
    it's just temporary script to test all MonoBehaviour Scripts together
*/
public class temp_contr : MonoBehaviour
{
    //game elements
    public View.Board board_view;
    public Model.Board board_model;
    Model.CardStack opportunity_knocks;
    Model.CardStack potluck;
    View.DiceContainer dice;
    public Dictionary<Model.Player,View.Piece> pieces; // dict for Piece objects where the keys values are references to Model.Player obj
    //players
    List<Model.Player> players;     // players list in some random order, it'll be ordered in GameState.ORDERINGPHASE
    Dictionary<Model.Player, int> player_throws; //holds throw values when deciding player order *JK: also for other stuff (for Utility Sqpace - get as much money as u throw X 4)
    int current_player;         // incremented every turn, holds the index of the current player (ordered in List players)
    // bits needed to manage game and turns
    TurnState turnState;
    GameState gameState;
    public bool double_rolled = false; // use this to keep track of whether player just rolled a double
    public int double_count = 0;           // incremented when player rolls a double, reset back to zero when current player is updated 
    bool passed_go = false; // use this to keep track if the current player can get money for passing GO
    public Model.Decision_outcome jail_decision = Model.Decision_outcome.NONE;
    //HUD
    public View.HUD hud; 
    //other
    Vector3 cam_pos_top;    // top cam position
    public GameObject invisibleWall;
    bool tabs_set;

    void Awake()
    {
        players = GameObject.Find("PersistentObject").GetComponent<PermObject>().players;
        foreach(Model.Player player in players) { player.allowed_to_buy = true; }
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
        if (Input.GetKeyDown(KeyCode.X))
        {
            ((View.PropertySquare)board_view.squares[1]).addHouse();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ((View.PropertySquare)board_view.squares[1]).removeHouse();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("totval " + players[current_player].totalValueOfAssets());
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
                    MessagePopUp tmp_popUp = MessagePopUp.Create(hud.transform, players[current_player].name + ", it's your turn!",2,true);
                    hud.set_current_player_tab(players[current_player]);
                    tabs_set = true;
                }
                if(players[current_player].in_jail > 1) // check if a bad boy
                {
                    hud.jail_bars.gameObject.SetActive(true);
                    dice.gameObject.SetActive(false);
                    if(hud.current_main_PopUp == null)
                    {
                        hud.current_main_PopUp = PopUp.OK(hud.transform,"You have to stay in jail for "+players[current_player].in_jail +" more rounds");
                        players[current_player].in_jail -= 1;
                        turnState = TurnState.PERFORM_ACTION;
                    }
                }
                else if(players[current_player].in_jail == 1)   // check if resocialized
                {
                    dice.gameObject.SetActive(false);
                    if(hud.current_main_PopUp == null)
                    {
                        hud.current_main_PopUp = PopUp.OK(hud.transform,"You are leaving the jail! You can roll dice in the next round!");
                        StartCoroutine(pieces[players[current_player]].leaveJail());
                        players[current_player].in_jail -= 1;
                        turnState = TurnState.PERFORM_ACTION;
                    }
                }
                else if(dice.start_roll) 
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
                    passed_go = (steps + pieces[players[current_player]].GetCurrentSquare())>=40; // if current position plus steps is greater than 40 then it means passed_go true
                    double_rolled = dice.is_double(); // return whether double was rolled
                    if (double_rolled)              // if double has been rolled, increase double count by 1 and maintain current player
                    {
                        double_count++;
                        if (double_count >= 3)      // if 3 doubles in a row, send player straight to jail or let them use the card
                        {
                            if(hud.current_main_PopUp != null)
                            {
                                hud.current_main_PopUp = View.PopUp.GoToJail(hud.transform, players[current_player], this, double_count+" doubles in a row? You must be cheating… go to jail!");
                            }
                            switch(jail_decision)
                            {
                                case Model.Decision_outcome.GO_TO_JAIL:
                                    turnState = TurnState.MANAGE_PROPERTIES;
                                    jail_decision = Model.Decision_outcome.NONE;
                                    double_rolled = false;      // reset double check so it won't have another turn
                                break;
                                case Model.Decision_outcome.SUCCESSFUL:
                                    double_count = 2;
                                    jail_decision = Model.Decision_outcome.NONE;
                                break;
                                case Model.Decision_outcome.NONE:
                                break;
                            }
                        } else {
                            MessagePopUp tmp_popUp = MessagePopUp.Create(hud.transform,players[current_player].name + " rolled a double, they will have another turn!",2,true);
                        }
                    }
                    if(players[current_player].in_jail == 0 && double_count < 3) // if a good boy (not in jail in general and not rolled 3 if used card)
                    {
                        if(steps < 0)                   // if result is negative (dice are stuck)
                        {                               // reset the dice
                            dice.reset();
                            MessagePopUp.Create(hud.transform, "Dice stuck. Please roll again!",2);
                            turnState = TurnState.BEGIN;
                        } else
                        {
                            // else start moving piece and change the turn state
                            StartCoroutine(pieces[players[current_player]].move(steps));
                            turnState = TurnState.PIECEMOVE;
                        }
                    }
                }
            }
            else if(turnState == TurnState.PIECEMOVE)
            {
                if(!pieces[players[current_player]].isMoving)   //if piece is not moving anymore
                {
                    turnState = TurnState.PERFORM_ACTION;   // change turn state to action
                    if(passed_go)
                    {
                        players[current_player].allowed_to_buy = true;
                        players[current_player].ReceiveCash(((Model.Space.Go)(board_model.spaces[0])).amount);
                        hud.UpdatePlayersTabInfo();
                        MessagePopUp temp_popUp = MessagePopUp.Create(hud.transform, "You passed GO! You receive "+((Model.Space.Go)(board_model.spaces[0])).amount+ "Q in cash!",3,true);
                        passed_go = false;
                    }
                    PerformAction();
                }
            }
            else if(turnState == TurnState.PERFORM_ACTION)  // ACTION state (buy property, pay rent etc...)
            {
                // when PopUp is closed the `trunState` is changed to MANAGEPROPERTIES
                if(hud.current_main_PopUp == null)
                {
                    turnState = TurnState.MANAGE_PROPERTIES;
                }
            }
            else if(turnState == TurnState.MANAGE_PROPERTIES)  // (manage your properties, check other players' properties)
            {
                hud.FinishTurnButton.gameObject.SetActive(true);
                // when player presses FINISH TURN button the `turnState` is changed to END
            }
            else if(turnState == TurnState.END)     // END state, when player finished his turn
            {
                hud.FinishTurnButton.gameObject.SetActive(false);
                if(double_rolled)              // if double has been rolled, increase double count by 1 and maintain current player
                {
                    MessagePopUp.Create(hud.transform, players[current_player].name + " rolled a double, they have another turn!",3);
                    double_rolled = false;      // reset double check
                    dice.reset();
                }
                else
                {
                    nextPlayer();
                    dice.reset();
                }
                tabs_set = false;
                jail_decision = Model.Decision_outcome.NONE; // reset jail decision
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
        else if(turnState == TurnState.DICEROLL || turnState == TurnState.DICEROLL_UTILITY_RENT)
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
                MessagePopUp tmp_popUp = MessagePopUp.Create(hud.transform, players[current_player].name + ", it's your turn!",2,true);
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
                invisibleWall.SetActive(false);
                int steps = dice.get_result();  // get the result
                if(steps < 0)                   // if result is negative (dice are stuck)
                {                                // reset the dice
                    MessagePopUp.Create(hud.transform, "Dice stuck. Please roll again!",2);
                    dice.reset();
                } else {    // if not in the ordering phase of the game, move Token and continue with game
                    invisibleWall.SetActive(false);
                    Debug.Log("Player " + current_player + " rolled a " + steps);
                    if (player_throws.ContainsValue(steps))             // force re-roll if player has already rolled the same number
                    {
                        MessagePopUp.Create(hud.transform, "Someone has already rolled "+ steps +". Please roll again!",2);
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
                            players = sorted_dict.Keys.ToList<Model.Player>();
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
         dice.gameObject.SetActive(true);
         hud.jail_bars.gameObject.SetActive(false); // reset jail bars to not active
         current_player = (current_player + 1) % players.Count;
         dice.gameObject.SetActive(true);
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
            performCardAction(card_taken, players[current_player],SqType.CHANCE);                 // if so, call performCardAction()
        }

        if (current_space_type == SqType.POTLUCK)
        {
            Model.Card card_taken = potluck.cards[0];
            performCardAction(card_taken, players[current_player],SqType.POTLUCK);                 // NOTE: currently when a card is taken, it is not placed at the bottom nor is the deck reshuffled
        }

        switch (current_space_type)
        {
            case SqType.GO:
            {
                break;
            }
            case SqType.JAILVISIT:
            {
                
                break;
            }
            case SqType.PARKING:
            {
                hud.current_main_PopUp = PopUp.OK(hud.transform, players[current_player].name + " landed on Free Parking. Collect all those juicy fines!");
                //reset FREE PARKING balance to zero
                //give player whatever the balance in FREE PARKING
                break;
            }
            case SqType.GOTOJAIL:
            {
                hud.current_main_PopUp = View.PopUp.GoToJail(hud.transform, players[current_player], this);
                //player token is moved to JAIL square
                //player hud icon is updated
                //jail cell animation on board
                break;
            }
            case SqType.PROPERTY:
            case SqType.STATION:
            {
                if(((Space.Purchasable)(current_space)).owner == null && players[current_player].allowed_to_buy)
                {
                    hud.current_main_PopUp = PopUp.BuyProperty(hud.transform, players[current_player],(Space.Purchasable)current_space, board_view.squares[current_square]);
                }
                else if(((Space.Purchasable)(current_space)).owner == players[current_player])
                {
                    
                }
                else if(((Space.Purchasable)(current_space)).owner != null)
                {
                    if(((Space.Purchasable)(current_space)).isMortgaged)
                    {

                    } else {
                        hud.current_main_PopUp = PopUp.PayRent(hud.transform,players[current_player],(Model.Space.Purchasable)current_space,board_model);
                    }
                } else {
                    MessagePopUp.Create(hud.transform, "You have to complete one circuit of the board by passing the GO to buy a property!",4);
                }
                break;
            }
            case SqType.UTILITY:
            {
                if(((Space.Purchasable)(current_space)).owner == null && players[current_player].allowed_to_buy)
                {
                    hud.current_main_PopUp = PopUp.BuyProperty(hud.transform, players[current_player],(Space.Purchasable)current_space, board_view.squares[current_square]);
                }
                else if(((Space.Purchasable)(current_space)).owner == players[current_player])
                {
                    
                }
                else if(((Space.Purchasable)(current_space)).owner != null)
                {
                    if(((Space.Utility)(current_space)).isMortgaged)
                    {

                    } else {
                        int rent_times = ((Space.Utility)(current_space)).rent_amount(board_model);
                        MessagePopUp.Create(hud.transform, "This company is owned by " + ((Space.Utility)(current_space)).owner.name+"! You have to pay "+ rent_times+" times the value shown on the dice!");
                        MessagePopUp temp_popUp = MessagePopUp.Create(hud.transform, "Roll the dice! ", 3);
                        StartCoroutine(payUtilityRentCoroutine(players[current_player],(Space.Purchasable)current_space,board_model));
                    }
                } else {
                    MessagePopUp.Create(hud.transform, "You have to complete one circuit of the board by passing the GO to buy a property!",4);
                }
                break;
            }
            case SqType.TAX:
            {
                hud.current_main_PopUp = PopUp.OK(hud.transform, players[current_player].name + " misfiled their tax returns, pay HMRC a SUPER TAX!");
                
                //if(players[current_player].totalValueOfAssets < *whatever is the tax amount*) { *GoSquare bankrupt* }
                break;
            }
        }
    }
    
    public void performCardAction(Model.Card card, Model.Player player,SqType card_type)
    {
        hud.current_main_PopUp = PopUp.OK(hud.transform, card.description);
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
            //  } else {
            //      p.payCash(card.kwargs["amont"],player)  
            //  }
            //}
            break;
            case CardAction.OUTOFJAIL:
            //player.outOfJailCards+=1;
            break;
            case CardAction.PAYORCHANCE:                        //this has to be implemented in method in OptionPopUp.cs (two different options)
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
            */
            //player.payCash(total);
            break;
        }

    }

    IEnumerator payUtilityRentCoroutine(Model.Player payer, Space.Purchasable space, Model.Board board)
    {
        bool successful = false;
        hud.current_main_PopUp = PopUp.OK(hud.transform,"");
        hud.current_main_PopUp.gameObject.SetActive(false);
        dice.reset();
        while(!successful)
        {
            if(dice.start_roll) 
            {
                turnState = TurnState.DICEROLL_UTILITY_RENT;
                invisibleWall.SetActive(true);
            } else {
                yield return null;
            }
            if(!dice.areRolling())  // if dice are not rolling anymore
            {
                invisibleWall.SetActive(false);
                int dice_result = dice.get_result();  // get the result
                if(dice_result < 0)                   // if result is negative (dice are stuck)
                {                               // reset the dice
                    dice.reset();
                    MessagePopUp.Create(hud.transform, "Dice stuck. Please roll again!",2);
                    turnState = TurnState.PERFORM_ACTION;
                } else {
                    Destroy(hud.current_main_PopUp.gameObject);
                    hud.current_main_PopUp = PopUp.PayRentUtility(hud.transform,payer,space,board,dice_result);
                    turnState = TurnState.PERFORM_ACTION;
                    successful = true;
                }
            }
            yield return null;
        }

    }

    public void sendPieceToJail()
    {
        StartCoroutine(pieces[players[current_player]].goToJail());
        jail_decision = Model.Decision_outcome.GO_TO_JAIL;
    }

    public void sendPieceToVisitJail()
    {
        StartCoroutine(pieces[players[current_player]].goToVisitJail());
        jail_decision = Model.Decision_outcome.GO_TO_JAIL;
    }

    public void sendPieceFree()
    {
        jail_decision = Model.Decision_outcome.SUCCESSFUL;
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
