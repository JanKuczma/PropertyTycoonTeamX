using System;
using UnityEngine;
using View;

namespace view.Menus
{
    public class HelpPopUp : View.PopUp
    {

        public int text_indicator = 0; // 0 = controls, 1 = rules
        public static string controls;
        public static string rules;
        
        public static HelpPopUp Create(Transform parent)
        {
            Debug.Log("do we get here?");
            HelpPopUp popUp = Instantiate(Asset.HelpPopUpPreFab, parent).GetComponent<HelpPopUp>();
            controls = "Click, drag and release dice to roll\nPress SPACEBAR while tokens are moving to speed them up\nHover over and click player tabs to access and manage properties";
            rules = "Make money yo";
            //popUp.btn1.onClick.AddListener(() => SwitchText());
            popUp.btn2.onClick.AddListener(() => popUp.closePopup());
            popUp.SetMessage(controls); 
            return popUp;
        }
        public void SwitchText()
        {
            if (text_indicator == 0)
            {
                
            }
        }
    }
}