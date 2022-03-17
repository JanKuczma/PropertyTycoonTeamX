using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTab : MonoBehaviour
{
    // Start is called before the first frame update
    Color color;
    public Text player_name;
    public Image token;

    public static PlayerTab Create(Transform parent,string name,Token token,Color color)
    {
        PlayerTab tab = Instantiate(Asset.playerTab(),parent).GetComponent<PlayerTab>();
        tab.setName(name);
        tab.setColor(color);
        tab.setToken(token);
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

    public void popUp()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x,-400);
        setColor(this.color,200);
    }

    public void hide()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x,-550);
        setColor(this.color,100);
    }
}
