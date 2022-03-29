using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ManagableCard : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    View.ManagePurchasable PopUp;
    bool isPointerOver = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        PopUp = View.ManagePurchasable.Create(transform,GetComponent<View.PurchasableCard>().property);
        isPointerOver = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = transform.localScale*2;
        isPointerOver = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = transform.localScale*.5f;
        isPointerOver = false;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if(!isPointerOver)
        {
            Destroy(gameObject);
        }
    }

}
