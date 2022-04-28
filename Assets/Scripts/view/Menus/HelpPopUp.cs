using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View;

namespace view.Menus
{
    public class HelpPopUp : View.PopUp
    {

        public static int text_indicator; // 0 = controls, 1 = rules
        public static TextMeshProUGUI rulesControlsText;
        public static string controls = "Click, drag and release dice to roll\n\nClick the arrows to the left and right of the screen to change camera angle\n\nHover over and click player tabs to access and manage properties";
        public static string rules = "Make money yo";
        
        public static HelpPopUp Create(Transform parent)
        {
            HelpPopUp popUp = Instantiate(Asset.HelpPopUpPreFab, parent).GetComponent<HelpPopUp>();
            text_indicator = 0;
            rulesControlsText.text = "Rules";
            popUp.btn1.onClick.AddListener(() => popUp.SetMessage(SwitchText()));
            popUp.btn2.onClick.AddListener(() => popUp.closePopup());
            popUp.SetMessage(controls);
            return popUp;
        }
        public static string SwitchText()
        {
            if (text_indicator == 0)
            {
                text_indicator = 1;
                rulesControlsText.text = "Controls";
                return rules;
            }
            if(text_indicator == 1)
            {
                text_indicator = 0;
                rulesControlsText.text = "Rules";
                return controls;
            }
            return "YOU SHOULD NOT GET HERE";
        }
    }
}