using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View
{
public class ManagePropertyController : ManageUtilityController
{
    override public void OnPointerClick(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
        if(PopUp == null)
        {
            PopUp = Instantiate(Asset.ManagePropertyPopUpPrefab,transform);
        }
    }
}
}
