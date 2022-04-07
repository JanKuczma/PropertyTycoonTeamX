using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace View
{
    /// <summary>
    /// Script used to make the property cards appear when one of 'cards' in Player Tab is hovered
    /// </summary>
public class PropertyToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image group;
    public PurchasableCard card;
    Coroutine cardShow;

    public void OnPointerEnter(PointerEventData eventData)
     {
        cardShow = StartCoroutine(showCard());
     }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(cardShow);
        card.gameObject.SetActive(false);
    }
    
    IEnumerator showCard()
    {
        yield return new WaitForSeconds(.6f);
        card.GetComponent<RectTransform>().SetParent(transform.parent.parent.parent.transform);
        card.gameObject.SetActive(true);
        card.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,transform.parent.parent.parent.GetComponent<RectTransform>().sizeDelta.y);
    }

}   
}
