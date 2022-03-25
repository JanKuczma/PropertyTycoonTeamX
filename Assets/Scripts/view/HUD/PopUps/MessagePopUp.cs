using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MessagePopUp : MonoBehaviour, IPointerClickHandler
{
    public Text message;
    void Awake()
    {
        StartCoroutine(closeAfterTime());
    }

    public static MessagePopUp Create(string message, Transform parent)
    {
        MessagePopUp popUp = Instantiate(Asset.MessagePopUpPrefab,parent).GetComponent<MessagePopUp>();
        popUp.message.text = message;
        return popUp;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Destroy(gameObject);
    }

    public IEnumerator closeAfterTime()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
        yield return null;
    }
}
