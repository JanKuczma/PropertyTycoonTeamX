using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ManagableCard : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDeselectHandler
{
    View.ManagePurchasable PopUp;
    public bool canManage = false;
    bool isPointerOver = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(canManage && PopUp == null){
            PopUp = View.ManagePurchasable.Create(transform,GetComponent<View.PurchasableCard>().property);
        }
        GetComponent<RectTransform>().SetAsLastSibling();
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<RectTransform>().SetAsLastSibling();
        if(PopUp == null) { transform.localScale = transform.localScale*2; }
        isPointerOver = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(PopUp == null) { transform.localScale = transform.localScale*.5f; }
        isPointerOver = false;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if(!isPointerOver && PopUp.gameObject != null)
        {
            transform.localScale = transform.localScale*.5f;
            Destroy(PopUp.gameObject);
        }
    }

}
