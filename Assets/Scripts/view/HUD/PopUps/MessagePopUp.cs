using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MessagePopUp : MonoBehaviour, IPointerClickHandler
{
    public Text message;
    int display_time;

    void Start()
    {
        StartCoroutine(closeAfterTime(display_time));
    }
    public static MessagePopUp Create(string message, Transform parent,int display_time = 5)
    {
        MessagePopUp popUp = Instantiate(Asset.MessagePopUpPrefab,parent).GetComponent<MessagePopUp>();
        popUp.message.text = message;
        popUp.display_time = display_time;
        return popUp;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Destroy(gameObject);
    }

    public IEnumerator closeAfterTime(int display_time)
    {
        yield return new WaitForSeconds(display_time);
        Destroy(gameObject);
        yield return null;
    }
}
