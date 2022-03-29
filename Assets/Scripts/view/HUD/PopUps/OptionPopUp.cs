using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace View
{
    public class OptionPopUp : MonoBehaviour
    {
        public Text message;
        Model.Player player;
        Model.Space.Purchasable space;
        Model.Board board_model;
        View.Board board_view;
        public Image optional_image;
        public Button OkBtn;
        int amount;

        public static OptionPopUp Create(Transform parent, GameObject popUpType,string msg, Model.Player player = null, Model.Space.Purchasable space = null, int amount = 0)
        {
            OptionPopUp popUp = Instantiate(popUpType, parent).GetComponent<OptionPopUp>();
            popUp.SetMessage(msg);
            popUp.player = player;
            popUp.space = space;
            popUp.board_model = FindObjectOfType<temp_contr>().board_model;
            popUp.board_view = FindObjectOfType<temp_contr>().board_view;
            popUp.amount = amount;
            return popUp;
        }
        
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

/*

    // Land on property options

*/

        public void buyPropertyOption()
        {
            switch(player.BuyProperty(space))
            {
                case Model.Decision_outcome.NOT_ENOUGH_MONEY:
                    MessagePopUp.Create("You have not enough money! Sell or mortgage your properties to get some cash!",transform,2);
                break;
                case Model.Decision_outcome.SUCCESSFUL:
                    if(board_view.squares[space.position-1] is PropertySquare)
                    {
                        ((PropertySquare)(board_view.squares[space.position-1])).showRibbon(player.color);
                    }
                    else if(board_view.squares[space.position-1] is UtilitySquare)
                    {
                        ((UtilitySquare)(board_view.squares[space.position-1])).showRibbon(player.color);
                    }
                    MessagePopUp.Create("Property purchased!",transform.parent,2);
                    closePopup();
                break;
            }
        }
        public void dontBuyPropertyOption()
        {
            MessagePopUp.Create("*Auction system to be developed*",transform.parent);
            closePopup();
        }

        public void PayRentOption()
        {
            switch(player.PayCash(space.rent_amount(board_model),space.owner))
            {
                case Model.Decision_outcome.NOT_ENOUGH_ASSETS:
                    MessagePopUp.Create("You're broke. You're bankrupt\n*bankrupt mechanism to be dveloped*",transform.parent,3);
                    closePopup();
                break;
                case Model.Decision_outcome.NOT_ENOUGH_MONEY:
                    MessagePopUp.Create("You have not enough money! Sell or mortgage your properties to get some cash!",transform,2);
                break;
            }
        }

/*

    // Land on GO TO JAIL options

*/
        public void goToJailOption()
        {
            player.in_jail += 2;
            FindObjectOfType<HUD>().jail_bars.gameObject.SetActive(true);
            FindObjectOfType<temp_contr>().sendPieceToJail();
            MessagePopUp.Create("You go to Jail!",transform.parent,2);
            closePopup();
        }
        public void jailCardOption()
        {
            if(player.getOutOfJailCardsNo == 0)
            {
                MessagePopUp.Create("You have no \"Break out of Jail\" cards!",transform,2);
            } else {
                player.getOutOfJailCardsNo -= 1;
                MessagePopUp.Create("You go to visit the jail... outside!",transform.parent,3);
                FindObjectOfType<temp_contr>().sendPieceToVisitJail();
                closePopup();
            }
        }
        public void jailPay50Option()
        {
            if(player.cash < 50)
            {
                MessagePopUp.Create("You have not enough money! Sell or mortgage your properties to get some cash!",transform,2);
            } else {
                player.PayCash(50);
                MessagePopUp.Create("You go free!",transform.parent,2);
                closePopup();
            }
        }

/*

    // Land on TAKE CARD options

*/

    public void takeCardOption()
    {
        MessagePopUp.Create("You took card!",transform.parent,2);
        closePopup();
    }
    public void optionalPayOption()
    {
        switch(player.PayCash(amount))
        {
            case Model.Decision_outcome.NOT_ENOUGH_MONEY:
                MessagePopUp.Create("You have not enough money! Sell or mortgage your properties to get some cash!",transform,2);
            break;
            case Model.Decision_outcome.SUCCESSFUL:
                closePopup();
            break;
        }
    }
    public void okPayBank()
    {
        switch(player.PayCash(amount))
        {
            case Model.Decision_outcome.NOT_ENOUGH_MONEY:
                MessagePopUp.Create("You have not enough money! Sell or mortgage your properties to get some cash!",transform,2);
            break;
            case Model.Decision_outcome.NOT_ENOUGH_ASSETS:
                MessagePopUp.Create("You're broke. You're bankrupt\n*bankrupt mechanism to be dveloped*",transform.parent,3);
                closePopup();
            break;
            case Model.Decision_outcome.SUCCESSFUL:
                closePopup();
            break;
        }
    }
    }
}