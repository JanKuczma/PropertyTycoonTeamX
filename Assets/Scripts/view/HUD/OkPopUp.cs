using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class OkPopUp : MonoBehaviour
    {
        public Text message;

        public static OkPopUp Create(Transform parent, GameObject popUpType,string msg)
        {
            OkPopUp popUp = Instantiate(popUpType, parent).GetComponent<OkPopUp>();
            popUp.SetMessage(msg);
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
            Debug.Log("Property bought");
            closePopup();
        }
        public void dontBuyPropertyOption()
        {
            Debug.Log("Property not bought");
            closePopup();
        }
    }
}