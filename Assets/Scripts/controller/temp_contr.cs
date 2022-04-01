using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using System.Threading;
using System.Timers;
using View;
using Object = System.Object;
using Space = Model.Space;
using Timer = System.Timers.Timer;
using UnityEngine.SceneManagement;

// enum for keeping track of the turnstate state
// just chucking a comment in here, testing git stuff :) (RD)
public enum TurnState {BEGIN,PRE_DICE_ROLL,DICEROLL,DICE_ROLL_EXTRA, CHECK_DOUBLE_ROLL,MOVE_THE_PIECE,PIECEMOVE, PERFORM_ACTION, MANAGE_PROPERTIES,END, NONE}
public enum GameState {PLAYERTURN,PAUSE,ORDERINGPHASE,WINNERCELEBRATION}
/*
    it's just temporary script to test all MonoBehaviour Scripts together
*/
public class temp_contr : MonoBehaviour
{
    //game elements
    public View.Board board_view;
    public Model.Board board_model;
    public Model.CardStack opportunity_knocks;
    public Model.CardStack potluck;
    public View.DiceContainer dice;
    public Dictionary<Model.Player,View.Piece> pieces; // dict for Piece objects where the keys values are references to Model.Player obj
    //players
    List<Model.Player> players;     // players list in some random order, it'll be ordered in GameState.ORDERINGPHASE
    Dictionary<Model.Player, int> player_throws; //holds throw values when deciding player order
    int current_player;         // incremented every turn, holds the index of the current player (ordered in List players)
    // bits needed to manage game and turns
    TurnState turnState;
    GameState gameState;
    public Timer rollTimer = new Timer(5000);
    public bool double_rolled = false; // use this to keep track of whether player just rolled a double
    public int double_count = 0;           // incremented when player rolls a double, reset back to zero when current player is updated 
    bool passed_go = false; // use this to keep track if the current player can get money for passing GO
    int steps; // to pass dice result between states
    //HUD
    public View.HUD hud; 
    //other
    Vector3 cam_pos_top;    // top cam position
    public GameObject invisibleWall;
    bool tabs_set;
    //SFX
    public BGMusicSelector music_player;
    void Awake()
    {
        players = GameObject.Find("PersistentObject").GetComponent<PermObject>().players;
        player_throws = new Dictionary<Model.Player, int>();    
        pieces = new Dictionary<Model.Player, View.Piece>();
        tabs_set = false;
        invisibleWall.SetActive(false);
        music_player = GameObject.Find("Background Music").GetComponent<BGMusicSelector>();
    }
    void Start()
    {
        //load data (to be changed for XLSX in near future)
        board_model = Model.JSONData.loadBoard(Asset.board_data_json());
        opportunity_knocks = Model.JSONData.loadCardStack(Asset.opportunity_knocks_data_json());
        opportunity_knocks.ShuffleStack();
        potluck = Model.JSONData.loadCardStack(Asset.potluck_data_json());
        potluck.ShuffleStack();
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
        //assign timer function
        rollTimer.Elapsed += TimedEventHandler;
        rollTimer.AutoReset = true;
        rollTimer.Enabled = true;
    }

    private void TimedEventHandler(object obj, ElapsedEventArgs e)
    {
        Debug.Log(obj);
        
        PopUp resetPopUp = PopUp.ResetDice(hud.transform, dice, "The dice aren't ever going to stop rolling on their own. Let's reset them!");
        
        dice.reset();
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
            ((View.PropertySquare)(board_view.squares[1])).addHouse();
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            ((View.PropertySquare)(board_view.squares[1])).removeHouse();
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            foreach(Model.Player player in players)
            {
                player.cash = 0;
            }
            players[0].cash = 10000;
        }
    }

    void FixedUpdate()
    {
        music_player.UpdateGameState(turnState);
        if(gameState == GameState.ORDERINGPHASE)    //if game state
        {
            decidePlayerOrder();
        }
        else if(gameState == GameState.PLAYERTURN)
        {
            if(turnState == TurnState.BEGIN)
            {
                if(players.Count == 1) { gameState = GameState.WINNERCELEBRATION; return; }
                if(!tabs_set)
                {
                    MessagePopUp tmp_popUp = MessagePopUp.Create(hud.transform, players[current_player].name + ", it's your turn!",2,true);
                    hud.set_current_player_tab(players[current_player]);
                    tabs_set = true;
                }
                if(players[current_player].in_jail > 1) // check if a bad boy
                {
                    if(hud.current_main_PopUp == null)
                    {
                        hud.jail_bars.gameObject.SetActive(true);
                        dice.gameObject.SetActive(false);
                        hud.current_main_PopUp = PopUp.InJail(hud.transform,this);
                    }
                }
                else if(players[current_player].in_jail == 1)   // check if resocialized
                {
                    dice.gameObject.SetActive(false);
                    if(hud.current_main_PopUp == null)
                    {
                        hud.current_main_PopUp = PopUp.OK(hud.transform,"You are leaving the jail! You can roll dice in the next round!");
                        
                        StartCoroutine(pieces[players[current_player]].leaveJail());
                        hud.jail_bars.gameObject.SetActive(false);
                        players[current_player].in_jail = 0;
                        turnState = TurnState.MANAGE_PROPERTIES;
                    }
                }
                else  
                {
                    turnState = TurnState.PRE_DICE_ROLL;
                }
            }
            else if(turnState == TurnState.PRE_DICE_ROLL)
            {
                if(dice.start_roll)
                {
                    turnState = TurnState.DICEROLL;
                    invisibleWall.SetActive(true);
                    rollTimer.Start();
                }
            }
            if(turnState == TurnState.DICEROLL) // turn begins
            {
                if(!dice.areRolling())  // if dice are not rolling anymore
                {
                    rollTimer.Stop();
                    invisibleWall.SetActive(false);
                    steps = dice.get_result();  // get the result
                    steps = 30; //DELTE WHEN DONE TESTING
                    double_rolled = dice.is_double(); // return whether double was rolled
                    if(steps < 0)                   // if result is negative (dice are stuck)
                    {                               // reset the dice
                        dice.reset();
                        MessagePopUp.Create(hud.transform, "Dice stuck. Please roll again!",2);
                        turnState = TurnState.PRE_DICE_ROLL;
                        return;
                    }
                    if (double_rolled)              // if double has been rolled, increase double count by 1 and maintain current player
                    {
                        double_count++;
                        turnState = TurnState.CHECK_DOUBLE_ROLL;
                    } else {
                        turnState = TurnState.MOVE_THE_PIECE;
                    }
                }
                else if(dice.belowBoard()) {
                    dice.reset();
                    MessagePopUp.Create(hud.transform, "Dice stuck. Please roll again!",2);
                    turnState = TurnState.PRE_DICE_ROLL;
                    return;
                }
            }
            else if(turnState == TurnState.CHECK_DOUBLE_ROLL)
            {
                if (double_count >= 3)      // if 3 doubles in a row, send player straight to jail or let them use the card
                {
                    hud.current_main_PopUp = View.PopUp.GoToJail(hud.transform, players[current_player], this, double_count+" doubles in a row? You must be cheating… go to jail!");
                } else {
                    MessagePopUp.Create(hud.transform,players[current_player].name + " rolled a double, they will have another turn!",2,true);
                }
                turnState = TurnState.MOVE_THE_PIECE;
                
            }
            else if(turnState == TurnState.MOVE_THE_PIECE)
            {
                if(hud.current_main_PopUp == null)
                {
                    if(players[current_player].in_jail == 0) // if a good boy (not in jail in general and not rolled 3 if used card)
                    {
                        // start moving piece and change the turn state
                        passed_go = (steps + pieces[players[current_player]].GetCurrentSquare())>=40; // if current position plus steps is greater than 40 then it means passed_go true
                        StartCoroutine(pieces[players[current_player]].move(steps));
                        turnState = TurnState.PIECEMOVE;
                    } else {
                        double_count = 0;
                        double_rolled = false;
                        turnState = TurnState.MANAGE_PROPERTIES;
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
                        hud.UpdateInfo(this);
                        MessagePopUp.Create(hud.transform, "You passed GO! You receive "+((Model.Space.Go)(board_model.spaces[0])).amount+ "Q in cash!",3,true);
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
                turnState = TurnState.BEGIN;     // change state to initial state
            }
        }
        else if(gameState == GameState.WINNERCELEBRATION)
        {
            if(hud.current_main_PopUp == null) {
                Debug.Log("I'm here");
                hud.current_main_PopUp = PopUp.OK(hud.transform,"Player " + players[current_player].name + " won the game.");
                hud.current_main_PopUp.btn1.onClick.AddListener(delegate {
                    Destroy(GameObject.Find("PersistentObject"));
                    SceneManager.LoadScene(0);
                });
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
        else if(turnState == TurnState.DICEROLL || turnState == TurnState.DICE_ROLL_EXTRA)
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
                rollTimer.Start();
                invisibleWall.SetActive(true);
                turnState = TurnState.DICEROLL;

            }
            if(!dice.areRolling())  //when dice stopped rolling
            {
                rollTimer.Stop();
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
            else if(dice.belowBoard())
            {
                dice.reset();
                MessagePopUp.Create(hud.transform, "Dice stuck. Please roll again!",2);
            }
     }

     void nextPlayer()
     {
         double_rolled = false;  // reset double check
         double_count = 0;       // reset double count
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
            Model.Card card_taken = opportunity_knocks.PopCard();
            performCardAction(card_taken, players[current_player],SqType.CHANCE);                 // if so, call performCardAction()
        }

        if (current_space_type == SqType.POTLUCK)
        {
            Model.Card card_taken = potluck.PopCard();
            performCardAction(card_taken, players[current_player],SqType.POTLUCK);                 
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
                hud.current_main_PopUp.btn1.onClick.AddListener(delegate {
                    players[current_player].ReceiveCash(board_model.parkingFees);
                    board_model.parkingFees = 0;
                });
                //give player whatever the balance in FREE PARKING
                //reset FREE PARKING balance to zero
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
            case SqType.UTILITY:
            {
                if(((Space.Purchasable)(current_space)).owner == null && players[current_player].allowed_to_buy)
                {
                    hud.current_main_PopUp = PopUp.BuyProperty(hud.transform, players[current_player],(Space.Purchasable)current_space, board_view.squares[current_square],this);
                }
                else if(((Space.Purchasable)(current_space)).owner == players[current_player])
                {
                    
                }
                else if(((Space.Purchasable)(current_space)).owner != null)
                {
                    if(((Space.Purchasable)(current_space)).isMortgaged)
                    {
                        MessagePopUp.Create(hud.transform, "This property is under mortgage, you don't have to pay the rent.",3);
                    }
                    else if(((Space.Purchasable)(current_space)).owner.in_jail > 0)
                    {
                        MessagePopUp.Create(hud.transform, "The owner of this property is in jail, you don't have to pay the rent.",3);
                    } else {
                        hud.current_main_PopUp = PopUp.PayRent(hud.transform,players[current_player],(Model.Space.Purchasable)current_space,board_model,this);
                    }
                } else {
                    MessagePopUp.Create(hud.transform, "You have to complete one circuit of the board by passing the GO to buy a property!",4);
                }
                break;
            }
            case SqType.TAX:
            {
                hud.current_main_PopUp = PopUp.OK(hud.transform, players[current_player].name + " misfiled their tax returns, pay HMRC "+((Model.Space.Tax)(current_space)).amount  +"Q of "+current_space.name);
                hud.current_main_PopUp.btn1.onClick.AddListener(() => hud.current_main_PopUp.PayOption(players[current_player].PayCash(((Model.Space.Tax)(current_space)).amount),this,players[current_player]));
                break;
            }
        }
    }
    
    public void performCardAction(Model.Card card, Model.Player player,SqType card_type)
    {
        switch(card.action)
        {
            case CardAction.PAYTOBANK:
                hud.current_main_PopUp = PopUp.Card(hud.transform,player,this,card,card_type);
                hud.current_main_PopUp.btn1.onClick.AddListener(() => hud.current_main_PopUp.PayOption(player.PayCash(card.kwargs["amount"]),this,player));
                hud.current_main_PopUp.btn1.onClick.AddListener(hud.current_main_PopUp.closePopup);
            break;
            case CardAction.PAYTOPLAYER:
                hud.current_main_PopUp = PopUp.Card(hud.transform,player,this,card,card_type);
                hud.current_main_PopUp.btn1.onClick.AddListener(() => player.ReceiveCash(card.kwargs["amount"]));
                hud.current_main_PopUp.btn1.onClick.AddListener(hud.current_main_PopUp.closePopup);
            break;
            case CardAction.MOVEFORWARDTO:
                steps = ((39+card.kwargs["position"]) - pieces[player].GetCurrentSquare())%40;
                passed_go = (steps + pieces[players[current_player]].GetCurrentSquare())>=40; // if current position plus steps is greater than 40 then it means passed_go true
                hud.current_main_PopUp = PopUp.Card(hud.transform,player,this,card,card_type);
                hud.current_main_PopUp.btn1.onClick.AddListener(() => StartCoroutine(pieces[player].move(steps)));
                hud.current_main_PopUp.btn1.onClick.AddListener(delegate { hud.current_main_PopUp.closePopup(); turnState = TurnState.PIECEMOVE; });
            break;
            case CardAction.MOVEBACKTO:
                steps = -1 * ((pieces[player].GetCurrentSquare()+41 - card.kwargs["position"])%40); 
                hud.current_main_PopUp = PopUp.Card(hud.transform,player,this,card,card_type);
                hud.current_main_PopUp.btn1.onClick.AddListener(() => StartCoroutine(pieces[player].move(steps)));
                hud.current_main_PopUp.btn1.onClick.AddListener(delegate { hud.current_main_PopUp.closePopup(); turnState = TurnState.PIECEMOVE; });
            break;
            case CardAction.MOVEBACK:
                steps = -1*card.kwargs["steps"];
                hud.current_main_PopUp = PopUp.Card(hud.transform,player,this,card,card_type);
                hud.current_main_PopUp.btn1.onClick.AddListener(() => StartCoroutine(pieces[player].move(steps)));
                hud.current_main_PopUp.btn1.onClick.AddListener(delegate { hud.current_main_PopUp.closePopup(); turnState = TurnState.PIECEMOVE; });
            break;
            case CardAction.GOTOJAIL:
                hud.current_main_PopUp = PopUp.Card(hud.transform,player,this,card,card_type);
                hud.current_main_PopUp.btn1.onClick.AddListener(delegate { hud.current_main_PopUp.closePopup(); hud.current_main_PopUp = View.PopUp.GoToJail(hud.transform, players[current_player], this); });      
            break;
            case CardAction.BIRTHDAY:
                string absent_names = "";
                hud.current_main_PopUp = PopUp.Card(hud.transform,player,this,card,card_type);
                hud.current_main_PopUp.btn1.onClick.AddListener(delegate {
                    hud.current_main_PopUp.closePopup();
                    foreach(Model.Player p in players)
                    {
                        if(p != player)
                        {
                            Model.Decision_outcome outcome = p.PayCash(card.kwargs["amount"],player);
                            if(outcome == Model.Decision_outcome.NOT_ENOUGH_MONEY || outcome == Model.Decision_outcome.NOT_ENOUGH_ASSETS)
                            {
                                absent_names += p.name + " ";
                            }
                        }
                    }
                    if(absent_names != "")
                    {
                        PopUp.OK(hud.transform,absent_names + "didn't come to you birthday party!");
                    }
                });
            break;
            case CardAction.OUTOFJAIL:
                hud.current_main_PopUp = PopUp.Card(hud.transform,player,this,card,card_type);
                hud.current_main_PopUp.btn1.onClick.AddListener(delegate {hud.current_main_PopUp.closePopup(); player.getOutOfJailCardsNo+=1;});
            break;
            case CardAction.PAYORCHANCE:
                hud.current_main_PopUp = PopUp.CardWithOption(hud.transform,player,this,card,card_type);
                hud.current_main_PopUp.btn1.onClick.AddListener(() => hud.current_main_PopUp.PayOption(player.PayCash(card.kwargs["amount"],board:board_model),this,player));
                hud.current_main_PopUp.btn2.onClick.AddListener(delegate {
                    hud.current_main_PopUp.closePopup(); 
                    Model.Card new_card = opportunity_knocks.PopCard();
                    performCardAction(new_card, player,SqType.CHANCE);
                    });
            break;
            case CardAction.PAYTOPARKING:
                hud.current_main_PopUp = PopUp.Card(hud.transform,player,this,card,card_type);
                hud.current_main_PopUp.btn1.onClick.AddListener(() => player.PayCash(card.kwargs["amount"],board:board_model));
                hud.current_main_PopUp.btn1.onClick.AddListener(hud.current_main_PopUp.closePopup);
            break;
            case CardAction.REPAIRS:
                int total = 0;
                foreach(Model.Space.Purchasable space in player.owned_spaces)
                {
                    if(space.type == SqType.PROPERTY)
                    {
                        if(((Model.Space.Property)space).noOfHouses == 5)
                        {
                            total += card.kwargs["hotel"];
                        } else {
                            total += ((Model.Space.Property)space).noOfHouses * card.kwargs["house"];
                        }
                    }
                }
                hud.current_main_PopUp = PopUp.Card(hud.transform,player,this,card,card_type);
                hud.current_main_PopUp.btn1.onClick.AddListener(() => hud.current_main_PopUp.PayOption(player.PayCash(total),this,player));
            break;
        }

    }

    IEnumerator rollInJailCoroutine(Model.Player player)
    {
        dice.gameObject.SetActive(true);
        bool successful = false;
        hud.current_main_PopUp = PopUp.OK(hud.transform,"");
        hud.current_main_PopUp.gameObject.SetActive(false);
        dice.reset();
        while(!successful)
        {
            if(dice.start_roll) 
            {
                rollTimer.Start();
                turnState = TurnState.DICE_ROLL_EXTRA;
                invisibleWall.SetActive(true);
            } else {
                yield return null;
            }
            if(!dice.areRolling())  // if dice are not rolling anymore
            {
                rollTimer.Stop();
                invisibleWall.SetActive(false);
                int dice_result = dice.get_result();  // get the result
                if(dice_result < 0)                   // if result is negative (dice are stuck)
                {                               // reset the dice
                    dice.reset();
                    MessagePopUp.Create(hud.transform, "Dice stuck. Please roll again!",2);
                    turnState = TurnState.PERFORM_ACTION;
                } else if (dice.is_double())
                {
                    successful = true;
                    Destroy(hud.current_main_PopUp.gameObject);
                    MessagePopUp.Create(hud.transform,"You go free!",4);
                    players[current_player].in_jail = 0;
                    StartCoroutine(pieces[players[current_player]].leaveJail());
                    hud.jail_bars.gameObject.SetActive(false);
                    turnState = TurnState.MANAGE_PROPERTIES;
                } else {
                    successful = true;
                    Destroy(hud.current_main_PopUp.gameObject);
                    MessagePopUp.Create(hud.transform,"You stay in Jail!",4);
                    players[current_player].in_jail -= 1;
                    turnState = TurnState.MANAGE_PROPERTIES;
                }
            }
            else if(dice.belowBoard())                   // if result is negative (dice are stuck)
            {                               // reset the dice
                dice.reset();
                MessagePopUp.Create(hud.transform, "Dice stuck. Please roll again!",2);
                turnState = TurnState.PERFORM_ACTION;
            }
            yield return null;
        }
    }

    IEnumerator auctionCoroutine(Model.Player player,Space.Purchasable current_space)
    {
        int highest_bid = current_space.cost-10;
        Model.Player highest_bidder = null;
        List<Model.Player> bidders = new List<Model.Player>();
        int current_bidder = 0;
        foreach(Model.Player p in players) { if(p != player && p.allowed_to_buy && p.in_jail == 0) { bidders.Add(p);  } }
        if(bidders.Count == 0)
        {
            hud.current_main_PopUp.closePopup();
            MessagePopUp.Create(hud.transform,"Nobody bought this property!",3);
            yield break;
        }
        hud.current_main_PopUp = PopUp.Auction(hud.transform,current_space);
        hud.current_main_PopUp.SetMessage(bidders[current_bidder].name + ", do you wish to bid for "+(highest_bid+10)+"?");
        hud.current_main_PopUp.btn1.onClick.AddListener(delegate {
            if(bidders[current_bidder].cash < highest_bid+10)
            {
                MessagePopUp.Create(hud.current_main_PopUp.transform,"You have not enough money!",2);
            } else {
                highest_bid = highest_bid+10;
                highest_bidder = bidders[current_bidder];
                current_bidder = (current_bidder+1)%bidders.Count;
                hud.current_main_PopUp.SetMessage(bidders[current_bidder].name + ", do you wish to bid for "+(highest_bid+10)+"?");
            }

        });
        hud.current_main_PopUp.btn2.onClick.AddListener(delegate {
            bidders.RemoveAt(current_bidder);
            if(bidders.Count > 0)
            {
                current_bidder = (current_bidder)%bidders.Count;
                hud.current_main_PopUp.SetMessage(bidders[current_bidder].name + ", do you wish to bid for "+(highest_bid+10)+"?");
            }
        });
        while((highest_bidder == null || !(highest_bidder != null && bidders.Count == 1)) && bidders.Count > 0)
        {
            Debug.Log("highest: "+ highest_bid);
            yield return null;
        }
        if(highest_bidder == null)
        {
            hud.current_main_PopUp.closePopup();
            MessagePopUp.Create(hud.transform,"Nobody bought this property!",3);
        } else {
            highest_bidder.BuyProperty(current_space,highest_bid);
            View.Square square = board_view.squares[current_space.position-1];
            if(square is PropertySquare)
            {
                (((View.PropertySquare)square)).showRibbon(highest_bidder.color);
            }
            else if(square is UtilitySquare)
            {
                ((View.UtilitySquare)(square)).showRibbon(highest_bidder.color);
            }
            hud.current_main_PopUp.closePopup();
            MessagePopUp.Create(hud.transform,highest_bidder.name+" purchased this property for "+highest_bid+"Q!",3);
        }
    }
    public void sendPieceToJail()
    {
        StartCoroutine(pieces[players[current_player]].goToJail());
        turnState = TurnState.MANAGE_PROPERTIES;
        double_rolled = false;      // reset double check so it won't have another turn
    }

    public void sendPieceToVisitJail()
    {
        StartCoroutine(pieces[players[current_player]].goToVisitJail());
        turnState = TurnState.MANAGE_PROPERTIES;
        double_rolled = false;      // reset double check so it won't have another turn
    }

    public void startAuction(Model.Player player, Model.Space.Purchasable space)
    {
        StartCoroutine(auctionCoroutine(player,space));
    }

    public void tryBreakOut()
    {
        StartCoroutine(rollInJailCoroutine(players[current_player]));
    }

    public void stayInJail()
    {
        players[current_player].in_jail -= 1;
        turnState = TurnState.MANAGE_PROPERTIES;
    }

    public void finishTurn()
    {
        turnState = TurnState.END;
    }
    public void RemovePLayer(Model.Player player)
    {
        foreach(Model.Space.Purchasable space in player.owned_spaces)
        {
            if(space is Model.Space.Property)
            {
                for(int i = 0; i > ((Model.Space.Property)(space)).noOfHouses; i++) { ((View.PropertySquare)(board_view.squares[space.position-1])).removeHouse(); }
                ((View.PropertySquare)(board_view.squares[space.position-1])).removeRibbon();
                ((Model.Space.Property)(space)).noOfHouses = 0;
            } else {
                ((View.UtilitySquare)(board_view.squares[space.position-1])).removeRibbon();
            }
            space.owner = null;
        }
        hud.RemovePlayerTab(player);
        Destroy(pieces[player].gameObject);
        players.Remove(player);
        hud.sort_tabs(players);
        double_rolled = false;  // reset double check
        double_count = 0;       // reset double count
        dice.gameObject.SetActive(true);
        hud.jail_bars.gameObject.SetActive(false); // reset jail bars to not active
        current_player = (current_player) % players.Count;
        dice.reset();
        tabs_set = false;
        turnState = TurnState.BEGIN;
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
