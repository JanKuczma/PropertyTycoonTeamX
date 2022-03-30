using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace View
{
    public class PopUp : MonoBehaviour
    {
        public Text message;
        public Button btn1;
        public Button btn2;
        public Button btn3;
        public Image optional_img;

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
            FindObjectOfType<View.HUD>().UpdatePlayersTabInfo();
        }

         public static PopUp OK(Transform parent, string message)
         {
            PopUp popUp = Instantiate(Asset.OkPopUpPrefab, parent).GetComponent<PopUp>();
            popUp.btn1.onClick.AddListener(popUp.closePopup);
            popUp.SetMessage(message);
            return popUp;
         }

         public static PopUp InJail(Transform parent, temp_contr controller)
         {
            PopUp popUp = Instantiate(Asset.InJailPopUpPrefab, parent).GetComponent<PopUp>();
            popUp.SetMessage("Stay in Jail or try to break out by rolling a double!");
            popUp.btn1.onClick.AddListener(() => popUp.stayInJailOption(controller));
            popUp.btn2.onClick.AddListener(() => popUp.rollInJailOption(controller));
            return popUp;
         }

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
            return popUp;
         }

        public static PopUp PayRent(Transform parent, Model.Player payer, Model.Space.Purchasable space, Model.Board board,temp_contr controller)
        {
            PopUp popUp = Instantiate(Asset.PayRentPopUpPrefab, parent).GetComponent<PopUp>();
            int rent_amount = space.rent_amount(board);
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
            }
            c.GetComponent<RectTransform>().anchoredPosition = new Vector2(220,0);
            c.gameObject.SetActive(true);
            return popUp;
        }
        public static PopUp PayRentUtility(Transform parent, Model.Player payer, Model.Space.Purchasable space, Model.Board board, int dice_result,temp_contr controller)
        {
            PopUp popUp = Instantiate(Asset.PayRentPopUpPrefab, parent).GetComponent<PopUp>();
            int rent_amount = space.rent_amount(board)*dice_result;
            popUp.SetMessage("This property is owned by " + space.owner.name+"! You have to pay "+ rent_amount+"!");
            popUp.btn1.onClick.AddListener(() => popUp.PayRentOption(payer.PayCash(rent_amount,space.owner),controller,payer));
            PurchasableCard c = UtilityCard.Create((Model.Space.Utility)space,popUp.transform);
            c.GetComponent<RectTransform>().anchoredPosition = new Vector2(220,0);
            c.gameObject.SetActive(true);
            return popUp;
        }

        public static PopUp BuyProperty(Transform parent, Model.Player player, Model.Space.Purchasable space, View.Square square, temp_contr controller)
        {
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
            return popUp;
        }

        public static PopUp GoToJail(Transform parent, Model.Player player, temp_contr controller, string msg = null)
        {
            PopUp popUp = Instantiate(Asset.GoToJailPopUpPrefab, parent).GetComponent<PopUp>();
            popUp.SetMessage(player.name + " broke the law! They must go straight to jail!");
            if(msg != null) { popUp.SetMessage(msg); }
            popUp.btn1.onClick.AddListener(() => popUp.goToJailOption(player, controller));
            popUp.btn2.onClick.AddListener(() => popUp.jailCardOption(player, controller));
            popUp.btn3.onClick.AddListener(() => popUp.jailPay50Option(player, controller));
            return popUp;
        }

        public static PopUp Card(Transform parent, Model.Player player, temp_contr controller, Model.Card card, SqType card_type)
        {
            PopUp popup = Instantiate(Asset.CardActionPopUp,parent).GetComponent<PopUp>();
            popup.SetMessage(card.description);
            switch(card_type)
            {
                case SqType.CHANCE:
                    popup.optional_img.sprite = Asset.chance_IMG;
                break;
                case SqType.POTLUCK:
                    popup.optional_img.sprite = Asset.opportunity_knocks_IMG;
                break;
            }
            return popup;
        }
        public static PopUp CardWithOption(Transform parent, Model.Player player, temp_contr controller, Model.Card card, SqType card_type)
        {
            PopUp popup = Instantiate(Asset.CardActionPopWithOptionsUp,parent).GetComponent<PopUp>();
            popup.SetMessage(card.description);
            switch(card_type)
            {
                case SqType.CHANCE:
                    popup.optional_img.sprite = Asset.chance_IMG;
                break;
                case SqType.POTLUCK:
                    popup.optional_img.sprite = Asset.opportunity_knocks_IMG;
                break;
            }
            return popup;
        }


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
                        (((View.PropertySquare)square)).showRibbon(player.color);
                    }
                    else if(square is UtilitySquare)
                    {
                        ((View.UtilitySquare)(square)).showRibbon(player.color);
                    }
                    MessagePopUp.Create(transform.parent,"Property purchased!",2);
                    closePopup();
                break;
            }
        }
        public void dontBuyPropertyOption(Model.Player player, Model.Space.Purchasable space, temp_contr controller)
        {
            controller.startAuction(player,space);
            closePopup();
        }

        public void PayRentOption(Model.Decision_outcome decision,temp_contr controller,Model.Player player)
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
    public void stayInJailOption(temp_contr controller)
    {
        MessagePopUp.Create(transform.parent, "You are staying in Jail!",3);
        controller.stayInJail();
        closePopup();
    }
    public void rollInJailOption(temp_contr controller)
    {
        controller.tryBreakOut();
        closePopup();
    }

/*

    // Land on GO TO JAIL options

*/
        public void goToJailOption(Model.Player player, temp_contr controller)
        {
            player.go_to_jail();
            controller.sendPieceToJail();
            FindObjectOfType<HUD>().jail_bars.gameObject.SetActive(true);
            MessagePopUp.Create(transform.parent, "You go to Jail!",3);
            closePopup();
        }
        public void jailCardOption(Model.Player player, temp_contr controller)
        {
            if(player.getOutOfJailCardsNo == 0)
            {
                MessagePopUp.Create(transform, "You have no \"Break out of Jail\" cards!",2);
            } else {
                player.getOutOfJailCardsNo -= 1;
                MessagePopUp.Create(transform.parent,"You go to visit the jail... outside!",3);
                controller.sendPieceToVisitJail();
                closePopup();
            }
        }
        public void jailPay50Option(Model.Player player, temp_contr controller)
        {
            if(player.cash < 50)
            {
                MessagePopUp.Create(transform, "You have not enough money! Sell or mortgage your properties to get some cash!",2);
            } else {
                player.PayCash(50,board:controller.board_model);
                MessagePopUp.Create(transform.parent,"You go to visit the jail... outside!",3);
                controller.sendPieceToVisitJail();
                closePopup();
            }
        }

/*

    // Land on TAKE CARD options

*/
    public void PayOption(Model.Decision_outcome decision,temp_contr controller,Model.Player player)
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
    }
}