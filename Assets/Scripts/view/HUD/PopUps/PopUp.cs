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

        public static PopUp PayRent(Transform parent, Model.Player payer, Model.Space.Purchasable space, Model.Board board)
        {
            PopUp popUp = Instantiate(Asset.PayRentPopUpPrefab, parent).GetComponent<PopUp>();
            int rent_amount = space.rent_amount(board);
            popUp.SetMessage("This property is owned by " + space.owner.name+"! You have to pay "+ rent_amount+"!");
            popUp.btn1.onClick.AddListener(() => popUp.PayRentOption(payer.PayCash(rent_amount,space.owner)));
            PurchasableCard c = PropertyCard.Create((Model.Space.Property)space,popUp.transform);
            c.GetComponent<RectTransform>().anchoredPosition = new Vector2(220,0);
            c.gameObject.SetActive(true);
            return popUp;
        }
        public static PopUp PayRentUtility(Transform parent, Model.Player payer, Model.Space.Purchasable space, Model.Board board, int dice_result)
        {
            PopUp popUp = Instantiate(Asset.PayRentPopUpPrefab, parent).GetComponent<PopUp>();
            int rent_amount = space.rent_amount(board)*dice_result;
            popUp.SetMessage("This property is owned by " + space.owner.name+"! You have to pay "+ rent_amount+"!");
            popUp.btn1.onClick.AddListener(() => popUp.PayRentOption(payer.PayCash(rent_amount,space.owner)));
            PurchasableCard c = PropertyCard.Create((Model.Space.Property)space,popUp.transform);
            c.GetComponent<RectTransform>().anchoredPosition = new Vector2(220,0);
            c.gameObject.SetActive(true);
            return popUp;
        }

        public static PopUp BuyProperty(Transform parent, Model.Player player, Model.Space.Purchasable space, View.Square square)
        {
            PopUp popUp = Instantiate(Asset.PayRentPopUpPrefab, parent).GetComponent<PopUp>();
            popUp.SetMessage(player.name + ", do you wish to purchase this property?");
            popUp.btn1.onClick.AddListener(() => popUp.buyPropertyOption(player.BuyProperty(space), player, square));
            popUp.btn2.onClick.AddListener(popUp.dontBuyPropertyOption);
            PurchasableCard c = PropertyCard.Create((Model.Space.Property)space,popUp.transform);
            c.GetComponent<RectTransform>().anchoredPosition = new Vector2(220,0);
            c.gameObject.SetActive(true);
            return popUp;
        }

        public static PopUp GoToJail(Transform parent, Model.Player player, temp_contr controller, string msg = null)
        {
            PopUp popUp = Instantiate(Asset.PayRentPopUpPrefab, parent).GetComponent<PopUp>();
            popUp.SetMessage(player.name + " broke the law! They must go straight to jail!");
            if(msg != null) { popUp.SendMessage(msg); }
            popUp.btn1.onClick.AddListener(() => popUp.goToJailOption(player, controller));
            popUp.btn2.onClick.AddListener(() => popUp.jailCardOption(player, controller));
            popUp.btn3.onClick.AddListener(() => popUp.jailPay50Option(player, controller));
            return popUp;
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
        public void dontBuyPropertyOption()
        {
            MessagePopUp.Create(transform.parent, "*Auction system to be developed*");
            closePopup();
        }

        public void PayRentOption(Model.Decision_outcome decision)
        {
            switch(decision)
            {
                case Model.Decision_outcome.NOT_ENOUGH_ASSETS:
                    MessagePopUp.Create(transform,"You're broke. You're bankrupt\n*bankrupt mechanism to be dveloped*",3);
                    closePopup();
                break;
                case Model.Decision_outcome.NOT_ENOUGH_MONEY:
                    MessagePopUp.Create(transform, "You have not enough money! Sell or mortgage your properties to get some cash!",2);
                break;
            }
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
                player.PayCash(50);
                MessagePopUp.Create(transform.parent, "You go free!",3);
                controller.sendPieceFree();
                closePopup();
            }
        }

/*

    // Land on TAKE CARD options

*/

    public void takeCardOption()
    {
        MessagePopUp.Create(transform.parent, "You took card!",2);
        closePopup();
    }
    public void optionalPayOption(Model.Decision_outcome decision)
    {
        switch(decision)
        {
            case Model.Decision_outcome.NOT_ENOUGH_MONEY:
                MessagePopUp.Create(transform, "You have not enough money! Sell or mortgage your properties to get some cash!",2);
            break;
            case Model.Decision_outcome.SUCCESSFUL:
                closePopup();
            break;
        }
    }
    public void okPayBank(Model.Decision_outcome decision)
    {
        switch(decision)
        {
            case Model.Decision_outcome.NOT_ENOUGH_MONEY:
                MessagePopUp.Create(transform, "You have not enough money! Sell or mortgage your properties to get some cash!",2);
            break;
            case Model.Decision_outcome.NOT_ENOUGH_ASSETS:
                MessagePopUp.Create(transform.parent, "You're broke. You're bankrupt\n*bankrupt mechanism to be dveloped*",3);
                closePopup();
            break;
            case Model.Decision_outcome.SUCCESSFUL:
                closePopup();
            break;
        }
    }
    }
}