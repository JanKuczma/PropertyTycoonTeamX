using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class OkPopUp : MonoBehaviour
    {
        public Text message;

        public static OkPopUp Create(Transform parent, string msg)
        {
            OkPopUp popUp = Instantiate(Asset.okPopup, parent).GetComponent<OkPopUp>();
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
            Debug.Log("hey");
        }
    }
}