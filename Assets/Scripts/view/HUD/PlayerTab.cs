using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 using UnityEngine.EventSystems;

public class PlayerTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    Color color;
    public Text player_name;
    public Image token;
    bool active;
    bool pointer_over;
     
     public void OnPointerEnter(PointerEventData eventData)
     {
         //do stuff
         pointer_over = true;
         if(pointer_over)
         {
            popUp(FindObjectOfType<Canvas>().GetComponent<RectTransform>().sizeDelta.y);
         }
     }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("not over");
        pointer_over = false;
        if(active)
        {
            halfPopUp(FindObjectOfType<Canvas>().GetComponent<RectTransform>().sizeDelta.y);
        } else {
            hide(FindObjectOfType<Canvas>().GetComponent<RectTransform>().sizeDelta.y);
        }
    }

    public static PlayerTab Create(Transform parent,string name,Token token,Color color)
    {
        PlayerTab tab = Instantiate(Asset.playerTab(),parent).GetComponent<PlayerTab>();
        tab.setName(name);
        tab.setColor(color);
        tab.setToken(token);
        tab.active = false;
        tab.pointer_over = false;
        return tab;
    }
    public void setName(string name)
    {
        this.player_name.text = name;
    }

    public void setToken(Token token)
    {
        this.token.sprite = Asset.TokenIMG(token);
    }

    public void setColor(Color color,float opacity = 100f)
    {
        color.a = opacity/255f;
        this.color = color;
        GetComponent<Image>().color = color;
    }

    public void popUp(float height)
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x,-400*height/1080);
        Debug.Log(GetComponentInParent<RectTransform>().sizeDelta.y);
    }

    public void hide(float height)
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x,-630*height/1080);
        setColor(this.color,100);
        active = false;
    }

    public void halfPopUp(float height)
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x,-540*height/1080);
        setColor(this.color,200);
        active = true;
    }
}
