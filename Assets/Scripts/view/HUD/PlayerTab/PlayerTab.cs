using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 using UnityEngine.EventSystems;
namespace View
{
public class PlayerTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    Color color;
    public Text player_name;
    public Image token;
    Token tokes_enum;
    public PropertyGrid propertyGrid;
    bool active;
    Coroutine popUpCoruotine = null;
    Coroutine tokenCoroutine = null;
    float _FrameRate = 25f;
     
     public void OnPointerEnter(PointerEventData eventData)
     {
        if(popUpCoruotine != null)
        {
            StopCoroutine(popUpCoruotine);
        }
        popUpCoruotine = StartCoroutine(popUp(FindObjectOfType<Canvas>().GetComponent<RectTransform>().sizeDelta.y));
     }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(popUpCoruotine != null)
        {
            StopCoroutine(popUpCoruotine);
        }
        if(active)
        {
            popUpCoruotine = StartCoroutine(halfPopUp(FindObjectOfType<Canvas>().GetComponent<RectTransform>().sizeDelta.y));
        } else {
            popUpCoruotine = StartCoroutine(hide(FindObjectOfType<Canvas>().GetComponent<RectTransform>().sizeDelta.y));
        }
    }

    public static PlayerTab Create(Transform parent,Color color,string name,Token token,string money,Dictionary<int,PurchasableCard> propertyCards)
    {
        PlayerTab tab = Instantiate(Asset.playerTab(),parent).GetComponent<PlayerTab>();
        tab.setName(name);
        tab.setColor(color);
        tab.setToken(token);
        tab.active = false;
        tab.setUpPropertyGrid(propertyCards);
        return tab;
    }
    public void setName(string name)
    {
        this.player_name.text = name;
    }

    public void setToken(Token token)
    {
        this.tokes_enum = token;
        this.token.sprite = Asset.TokenIMG(token);
    }

    public void setColor(Color color,float opacity = 100f)
    {
        color.a = opacity/255f;
        this.color = color;
        GetComponent<Image>().color = color;
    }

    public IEnumerator popUp(float height)
    {
        float timeOfTravel = 1f;
        float currentTime = 0f;
        while (currentTime <= timeOfTravel) { 
            currentTime += Time.deltaTime; 
            GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(GetComponent<RectTransform>().anchoredPosition,new Vector2(GetComponent<RectTransform>().anchoredPosition.x,-360*height/1080), currentTime/timeOfTravel); 
            yield return null; }
    }

    public IEnumerator hide(float height)
    {
        if(tokenCoroutine != null){ StopCoroutine(tokenCoroutine); tokenCoroutine = null;}
        token.sprite = Asset.TokenIMG(tokes_enum);
        setColor(this.color,100);
        active = false;
        float timeOfTravel = 1f;
        float currentTime = 0f;
        while (currentTime <= timeOfTravel) { 
            currentTime += Time.deltaTime; 
            GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(GetComponent<RectTransform>().anchoredPosition,new Vector2(GetComponent<RectTransform>().anchoredPosition.x,-650*height/1080), currentTime/timeOfTravel); 
            yield return null; }
    }

    public IEnumerator halfPopUp(float height)
    {
        setColor(this.color,200);
        active = true;
        float timeOfTravel = 1f;
        float currentTime = 0f;
        if(tokenCoroutine == null)
        { tokenCoroutine = StartCoroutine(playAnim(Asset.TokenAnim(tokes_enum))); }
        while (currentTime <= timeOfTravel) { 
            currentTime += Time.deltaTime; 
            GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(GetComponent<RectTransform>().anchoredPosition,new Vector2(GetComponent<RectTransform>().anchoredPosition.x,-580*height/1080), currentTime/timeOfTravel); 
            yield return null; }
    }
    void setUpPropertyGrid(Dictionary<int,PurchasableCard> propertyCards)
    {
        foreach(KeyValuePair<int, PurchasableCard> entry in propertyCards)
        {
            this.propertyGrid.propertyToggles.getValue(entry.Key).card = entry.Value;
            if(entry.Value.GetType() == typeof(PropertyCard))
            {
                this.propertyGrid.propertyToggles.getValue(entry.Key).group.color = ((PropertyCard)entry.Value).group.color;
            }
        }
    }

    IEnumerator playAnim(Sprite[] tokenSheet)
    {
        int index = 0;
        float frame_time = Time.deltaTime;
        while(active)
        {
            if(frame_time >= 1/_FrameRate)
            {
                token.sprite = tokenSheet[index];
                index = (index+1)%tokenSheet.Length;
                frame_time = 0;
            }
            frame_time += Time.deltaTime;
            yield return null;
        }
    }
}
}
