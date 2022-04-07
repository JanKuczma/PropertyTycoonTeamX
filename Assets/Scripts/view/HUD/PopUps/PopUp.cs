using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

namespace View
{
    /// <summary>
    /// Popups that require Player's interaction.
    /// </summary>
    public class PopUp : MonoBehaviour
    {
        public Text message;
        /// <summary>
        /// The button most to the left.
        /// </summary>
        public Button btn1;
        /// <summary>
        /// The button most to the right.
        /// </summary>
        public Button btn2;
        /// <summary>
        /// Button most to the botttom.
        /// </summary>
        public Button btn3;
        public Image optional_img;
        public int rules_controls_index = 0;
        public TextMeshProUGUI buttonText;

        public void SetMessage(string msg)
        {
            this.message.text = msg;
        }

        public void closePopup()
        {
            Destroy(this.gameObject);
        }
        void OnDestroy()
        {
            if(FindObjectOfType<View.HUD>()) {FindObjectOfType<View.HUD>().UpdateInfo(FindObjectOfType<game_controller>());}
        }
/// <summary>
/// Popup with one button. Simple popup that does not require decision.
/// </summary>
         public static PopUp OK(Transform parent, string message)
         {
            PopUp popUp = Instantiate(Asset.OkPopUpPrefab, parent).GetComponent<PopUp>();
            popUp.btn1.onClick.AddListener(popUp.closePopup);
            popUp.SetMessage(message);
            popUp.transform.SetSiblingIndex(2);
            return popUp;
         }

         public static PopUp Help(Transform parent)
         {
             PopUp popUp = Instantiate(Asset.HelpPopUpPreFab, parent).GetComponent<PopUp>();
             popUp.SetMessage("Click, drag and release dice to roll\n\nPress SPACEBAR while tokens are moving to speed them up\n\nHover over and click player tabs to access and manage properties");
             popUp.btn1.onClick.AddListener(() => popUp.ControlsRulesSwitcher());
             popUp.btn2.onClick.AddListener(popUp.closePopup);
             return popUp;
         }
/// <summary>
/// Popup displayed when Player is in Jail. Asks if Player either wants to stay in Jail or try to roll double to break out.
/// </summary>
         public static PopUp InJail(Transform parent, game_controller controller)
         {
            controller.AI_trigger = Model.Decision_trigger.INJAIL;
            PopUp popUp = Instantiate(Asset.InJailPopUpPrefab, parent).GetComponent<PopUp>();
            popUp.SetMessage("Stay in Jail or try to break out by rolling a double!");
            popUp.btn1.onClick.AddListener(() => popUp.stayInJailOption(controller));
            popUp.btn2.onClick.AddListener(() => popUp.rollInJailOption(controller));
            popUp.transform.SetSiblingIndex(2);
            return popUp;
         }
/// <summary>
/// Auction popup. Two buttons: Bid and Pass.
/// </summary>
         public static PopUp Auction(Transform parent,Model.Space.Purchasable space)
         {
            PopUp popUp = Instantiate(Asset.AuctionPopUpPrefab, parent).GetComponent<PopUp>();
            PurchasableCard c = null;
            switch(space.type)
            {
                case SqType.PROPERTY:
                c = PropertyCard.Create((Model.Space.Property)space,popUp.transform);
                break;
                case SqType.STATION:
                c = StationCard.Create((Model.Space.Station)space,popUp.transform);
                break;
                case SqType.UTILITY:
                c = UtilityCard.Create((Model.Space.Utility)space,popUp.transform);
                break;
            }
            c.GetComponent<RectTransform>().anchoredPosition = new Vector2(220,0);
            c.gameObject.SetActive(true);
            popUp.transform.SetSiblingIndex(2);
            return popUp;
         }
/// <summary>
/// 'Pay rent' popup. It won't disapear until Player can pay the rent OR player has not enough assets to pay the debt.
/// </summary>
        public static PopUp PayRent(Transform parent, Model.Player payer, Model.Space.Purchasable space, Model.Board board,game_controller controller,int rent=0)
        {
            controller.AI_trigger = Model.Decision_trigger.PAYMONEY;
            PopUp popUp = Instantiate(Asset.PayRentPopUpPrefab, parent).GetComponent<PopUp>();
            int rent_amount = space.rent_amount(board);
            if(space is Model.Space.Utility) { rent_amount *= controller.dice.get_result(); }
            controller.AI_moneyToPay = rent_amount;
            popUp.SetMessage("This property is owned by " + space.owner.name+"! You have to pay "+ rent_amount+"!");
            popUp.btn1.onClick.AddListener(() => popUp.PayRentOption(payer.PayCash(rent_amount,space.owner),controller,payer));
            PurchasableCard c = null;
            switch(space.type)
            {
                case SqType.PROPERTY:
                c = PropertyCard.Create((Model.Space.Property)space,popUp.transform);
                break;
                case SqType.STATION:
                c = StationCard.Create((Model.Space.Station)space,popUp.transform);
                break;
                case SqType.UTILITY:
                c = UtilityCard.Create((Model.Space.Utility)space,popUp.transform);
                break;
            }
            c.GetComponent<RectTransform>().anchoredPosition = new Vector2(220,0);
            c.gameObject.SetActive(true);
            popUp.transform.SetSiblingIndex(2);
            return popUp;
        }
/// <summary>
/// Two options popup: Buy OR Not To Buy, this the question...
/// </summary>
        public static PopUp BuyProperty(Transform parent, Model.Player player, Model.Space.Purchasable space, View.Square square, game_controller controller)
        {
            controller.AI_trigger = Model.Decision_trigger.BUYPROPERTY;
            PopUp popUp = Instantiate(Asset.BuyPropertyPopup, parent).GetComponent<PopUp>();
            popUp.SetMessage(player.name + ", do you wish to purchase this property?");
            popUp.btn1.onClick.AddListener(() => popUp.buyPropertyOption(player.BuyProperty(space), player, square));
            popUp.btn2.onClick.AddListener(() => popUp.dontBuyPropertyOption(player,space,controller));
            PurchasableCard c = null;
            switch(space.type)
            {
                case SqType.PROPERTY:
                c = PropertyCard.Create((Model.Space.Property)space,popUp.transform);
                break;
                case SqType.STATION:
                c = StationCard.Create((Model.Space.Station)space,popUp.transform);
                break;
                case SqType.UTILITY:
                c = UtilityCard.Create((Model.Space.Utility)space,popUp.transform);
                break;
            }
            c.GetComponent<RectTransform>().anchoredPosition = new Vector2(220,0);
            c.gameObject.SetActive(true);
            popUp.transform.SetSiblingIndex(2);
            return popUp;
        }
/// <summary>
/// PopUp with 3 options: Pay 50, Use Card OR Go To Jail
/// </summary>
        public static PopUp GoToJail(Transform parent, Model.Player player, game_controller controller, string msg = null)
        {
            controller.AI_trigger = Model.Decision_trigger.GOTOJAIL;
            PopUp popUp = Instantiate(Asset.GoToJailPopUpPrefab, parent).GetComponent<PopUp>();
            popUp.SetMessage(player.name + " broke the law! They must go straight to jail!");
            if(msg != null) { popUp.SetMessage(msg); }
            popUp.btn1.onClick.AddListener(() => popUp.goToJailOption(player, controller));
            popUp.btn2.onClick.AddListener(() => popUp.jailCardOption(player, controller));
            popUp.btn3.onClick.AddListener(() => popUp.jailPay50Option(player, controller));
            popUp.transform.SetSiblingIndex(2);
            
            controller.soundManagerClassic.PlayAndStopOthers("Jail");
            
            return popUp;
        }
/// <summary>
/// PopUp with one OK button used to display the action card description.
/// </summary>
        public static PopUp Card(Transform parent, Model.Player player, game_controller controller, Model.Card card, SqType card_type)
        {
            PopUp popup = Instantiate(Asset.CardActionPopUp,parent).GetComponent<PopUp>();
            popup.SetMessage(card.description);
            switch(card_type)
            {
                case SqType.CHANCE:
                    popup.optional_img.sprite = Asset.ClassicChangeIMG;
                    if(GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>().starWarsTheme) { popup.optional_img.sprite = Asset.StarWarsChangeIMG; }
                break;
                case SqType.POTLUCK:
                    popup.optional_img.sprite = Asset.ClassicOppKnocksIMG;
                    if(GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>().starWarsTheme) { popup.optional_img.sprite = Asset.StarWarsOppKnocksIMG; }
                break;
            }
            popup.transform.SetSiblingIndex(2);
            return popup;
        }
/// <summary>
/// PopUp with TWO buttons. Used to display the action card description.
/// </summary>
        public static PopUp CardWithOption(Transform parent, Model.Player player, game_controller controller, Model.Card card, SqType card_type)
        {
            PopUp popup = Instantiate(Asset.CardActionPopWithOptionsUp,parent).GetComponent<PopUp>();
            popup.SetMessage(card.description);
            switch(card_type)
            {
                case SqType.CHANCE:
                    popup.optional_img.sprite = Asset.ClassicChangeIMG;
                    if(GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>().starWarsTheme) { popup.optional_img.sprite = Asset.StarWarsChangeIMG; }
                break;
                case SqType.POTLUCK:
                    popup.optional_img.sprite = Asset.ClassicOppKnocksIMG;
                    if(GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>().starWarsTheme) { popup.optional_img.sprite = Asset.StarWarsOppKnocksIMG; }
                break;
            }
            popup.transform.SetSiblingIndex(2);
            return popup;
        }

// The "what happens if I click this button" functions. In other words OnClick Events.


        public void buyPropertyOption(Model.Decision_outcome decision, Model.Player player, View.Square square)
        {
            switch(decision)
            {
                case Model.Decision_outcome.NOT_ENOUGH_MONEY:
                    MessagePopUp.Create(transform,"You have not enough money! Sell or mortgage your properties to get some cash!",2);
                break;
                case Model.Decision_outcome.SUCCESSFUL:
                    if(square is PropertySquare)
                    {
                        (((View.PropertySquare)square)).showRibbon(player.Color());
                    }
                    else if(square is UtilitySquare)
                    {
                        ((View.UtilitySquare)(square)).showRibbon(player.Color());
                    }

                    SoundManager soundManager = GameObject.FindWithTag("GameMusic").GetComponent<SoundManager>();
                    MessagePopUp.Create(transform.parent,"Property purchased!",2);
                    soundManager.PlayPurchaseSound();
                    closePopup();
                break;
            }
        }
        public void dontBuyPropertyOption(Model.Player player, Model.Space.Purchasable space, game_controller controller)
        {
            controller.startAuction(player,space);
            closePopup();
        }

        public void PayRentOption(Model.Decision_outcome decision,game_controller controller,Model.Player player)
        {
            switch(decision)
            {
                case Model.Decision_outcome.NOT_ENOUGH_ASSETS:
                controller.hud.current_main_PopUp = PopUp.OK(controller.hud.transform,"You have no enough assets to pay it! You lost the game!");
                controller.hud.current_main_PopUp.btn1.onClick.AddListener(() => controller.RemovePLayer(player));
                closePopup();
            break;
                case Model.Decision_outcome.NOT_ENOUGH_MONEY:
                    MessagePopUp.Create(transform, "You have not enough money! Sell or mortgage your properties to get some cash!",2);
                break;
                case Model.Decision_outcome.SUCCESSFUL:
                    MessagePopUp.Create(transform.parent, "Rent paid!",2);
                    closePopup();
                break;
            }
        }
/*

    //while in jail

*/
    public void stayInJailOption(game_controller controller)
    {
        MessagePopUp.Create(transform.parent, "You are staying in Jail!",3);
        controller.stayInJail();
        closePopup();
    }
    public void rollInJailOption(game_controller controller)
    {
        controller.tryBreakOut();
        closePopup();
    }

/*

    // Land on GO TO JAIL options

*/
        public void goToJailOption(Model.Player player, game_controller controller)
        {
            player.go_to_jail();
            controller.sendPieceToJail();
            FindObjectOfType<HUD>().jail_bars.gameObject.SetActive(true);
            MessagePopUp.Create(transform.parent, "You go to Jail!",3);
            closePopup();
        }
        public void jailCardOption(Model.Player player, game_controller controller)
        {
            if(player.getOutOfJailCardsNo == 0)
            {
                MessagePopUp.Create(transform, "You have no \"Break out of Jail\" cards!",2);
            } else {
                player.position = 11;
                player.getOutOfJailCardsNo -= 1;
                MessagePopUp.Create(transform.parent,"You go to visit the jail... outside!",3);
                controller.sendPieceToVisitJail();
                closePopup();
            }
        }
        public void jailPay50Option(Model.Player player, game_controller controller)
        {
            if(player.cash < 50)
            {
                MessagePopUp.Create(transform, "You have not enough money! Sell or mortgage your properties to get some cash!",2);
            } else {
                player.position = 11;
                player.PayCash(50,board:controller.board_model);
                MessagePopUp.Create(transform.parent,"You go to visit the jail... outside!",3);
                controller.sendPieceToVisitJail();
                closePopup();
            }
        }

/*

    // Land on TAKE CARD options

*/
    public void PayOption(Model.Decision_outcome decision,game_controller controller,Model.Player player)
    {
        switch(decision)
        {
            case Model.Decision_outcome.NOT_ENOUGH_MONEY:
                MessagePopUp.Create(transform, "You have not enough money! Sell or mortgage your properties to get some cash!",2);
            break;
            case Model.Decision_outcome.NOT_ENOUGH_ASSETS:
                controller.hud.current_main_PopUp = PopUp.OK(controller.hud.transform,"You have no enough assets to pay it! You lost the game!");
                controller.hud.current_main_PopUp.btn1.onClick.AddListener(() => controller.RemovePLayer(player));
                closePopup();
            break;
            case Model.Decision_outcome.SUCCESSFUL:
                closePopup();
            break;
        }
    }

    public void ControlsRulesSwitcher()
    {
        if (rules_controls_index == 0)
        {
            rules_controls_index = 1;
            SetMessage("Make money yo");
            buttonText.text = "Rules";
        }
        else
        {
            rules_controls_index = 0;
            SetMessage("Click, drag and release dice to roll\n\nPress SPACEBAR while tokens are moving to speed them up\n\nHover over and click player tabs to access and manage properties");
            buttonText.text = "Controls";
        }
    }
    
    }
}