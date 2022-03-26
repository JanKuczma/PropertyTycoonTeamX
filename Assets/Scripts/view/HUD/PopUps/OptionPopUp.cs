using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class OptionPopUp : MonoBehaviour
    {
        public Text message;
        Model.Player player;
        Model.Space.Purchasable space;
        View.Square square;
        int amount;
        public Image optional_image;
        public Button OkBtn;

        public static OptionPopUp Create(Transform parent, GameObject popUpType,string msg, Model.Player player = null, Model.Space.Purchasable space = null, View.Square square = null, int amount = 0)
        {
            OptionPopUp popUp = Instantiate(popUpType, parent).GetComponent<OptionPopUp>();
            popUp.SetMessage(msg);
            popUp.player = player;
            popUp.space = space;
            popUp.square  = square;
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
            if(player.cash < space.cost)
            {
                MessagePopUp.Create("You have not enough money! Sell or mortgage your properties to get some cash!",transform);
            } else {
                player.PayCash(space.cost);
                player.owned_spaces.Add(space);
                space.owner = player;
                if(square is PropertySquare)
                {
                    ((PropertySquare)(square)).showRibbon(player.color);
                }
                else if(square is UtilitySquare)
                {
                    ((UtilitySquare)(square)).showRibbon(player.color);
                }
                MessagePopUp.Create("Property bought",transform.parent);
                closePopup();
            }
        }
        public void dontBuyPropertyOption()
        {
            MessagePopUp.Create("*Auction system to be developed*",transform.parent);
            closePopup();
        }

        public void okPayRent(Model.Player owner,Model.Player payer)
    {
        if(payer.totalValueOfAssets() < this.amount)
        {
            MessagePopUp.Create("You're broke. You're bankrupt",transform.parent);
            owner.ReceiveCash(amount);
            closePopup();
        }
        else if(payer.cash < this.amount)
        {
            MessagePopUp.Create("You have not enough money! Sell or mortgage your properties to get some cash!",transform);
        } else {
            payer.PayCash(amount,owner);
            closePopup();
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
            MessagePopUp.Create("You go to Jail!",transform.parent);
            closePopup();
        }
        public void jailCardOption()
        {
            if(player.getOutOfJailCardsNo == 0)
            {
                MessagePopUp.Create("You have no \"Break out of Jail\" cards!",transform);
            } else {
                player.getOutOfJailCardsNo -= 1;
                MessagePopUp.Create("You go free!",transform.parent);
                closePopup();
            }
        }

/*

    // Land on TAKE CARD options

*/

    public void takeCardOption()
    {
        MessagePopUp.Create("You took card",transform.parent);
        closePopup();
    }
    public void optionalPayOption()
    {
        if(player.cash < this.amount)
        {
            MessagePopUp.Create("You have not enough money! Sell or mortgage your properties to get some cash!",transform);
        } else {
            player.PayCash(amount);
            closePopup();
        }
    }
    public void okPayBank()
    {
        if(player.totalValueOfAssets() < this.amount)
        {
            MessagePopUp.Create("You're broke. You're bankrupt",transform.parent);
            closePopup();
        }
        else if(player.cash < this.amount)
        {
            MessagePopUp.Create("You have not enough money! Sell or mortgage your properties to get some cash!",transform);
        } else {
            player.PayCash(amount);
            closePopup();
        }
    }
    }
}