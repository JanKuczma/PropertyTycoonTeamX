using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace View
{
public class ManageUtilityController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDeselectHandler
{
    protected GameObject PopUp = null;
    bool isPointerOver = false;

    virtual public void OnPointerClick(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
        if(PopUp == null)
        {
            PopUp = Instantiate(Asset.ManageUtilityPopUpPrefab,transform);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if(!isPointerOver)
        {
            Destroy(PopUp.gameObject);
            PopUp = null;
        }
    }
}
}
