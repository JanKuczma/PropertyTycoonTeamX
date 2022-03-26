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
        public Image optional_image;

        public static OptionPopUp Create(Transform parent, GameObject popUpType,string msg, Model.Player player = null, Model.Space.Purchasable space = null, View.Square square = null)
        {
            OptionPopUp popUp = Instantiate(popUpType, parent).GetComponent<OptionPopUp>();
            popUp.SetMessage(msg);
            popUp.player = player;
            popUp.space = space;
            popUp.square  = square;
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
    public void payOption()
    {
        MessagePopUp.Create("You decided to pay",transform.parent);
        closePopup();
    }
    }
}