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

        public void buyPropertyOption()
        {
            if(player.cash < space.cost)
            {
                MessagePopUp.Create("You have not enough money! Sell or mortgage your properties to get some cash!",transform);
            } else {
                player.PayCash(space.cost);
                player.owned_spaces.Add(space);
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
            Debug.Log("Property not bought");
            closePopup();
        }

        void OnDestroy()
        {
            FindObjectOfType<View.HUD>().UpdatePlayersTabInfo();
        }
    }
}