using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 using UnityEngine.EventSystems;
namespace View
{
public class PlayerTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Start is called before the first frame update
    public Model.Player player;
    public Dictionary<int,PurchasableCard> propertyCards;
    Color color;
    public TMPro.TMP_Text player_name;
    public TMPro.TMP_Text player_money;
    public Image token;
    Token tokes_enum;
    public PropertyGrid propertyGrid;
    public bool currentPlayer;
    Coroutine popUpCoruotine = null;
    Coroutine tokenCoroutine = null;
    float _FrameRate = 25f;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(transform.parent.GetComponent<HUD>().currentManager != null) { Destroy(transform.parent.GetComponent<HUD>().currentManager.gameObject); }
        transform.parent.GetComponent<HUD>().currentManager = PropertyManager.Create(FindObjectOfType<Canvas>().transform,player,propertyCards,(currentPlayer && player.in_jail == 0 && player.isHuman)).GetComponent<PropertyManager>();
    }
     
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
        if(currentPlayer)
        {
            popUpCoruotine = StartCoroutine(halfPopUp(FindObjectOfType<Canvas>().GetComponent<RectTransform>().sizeDelta.y));
        } else {
            popUpCoruotine = StartCoroutine(hide(FindObjectOfType<Canvas>().GetComponent<RectTransform>().sizeDelta.y));
        }
    }

    public static PlayerTab Create(Transform parent,Model.Player player,Dictionary<int,PurchasableCard> propertyCards)
    {
        PlayerTab tab = Instantiate(Asset.PlayerTabPrefab,parent).GetComponent<PlayerTab>();
        tab.player = player;
        tab.propertyCards = propertyCards;
        tab.setName(player.name);
        tab.setColor(player.Color());
        tab.setToken(player.token);
        tab.setMoney(player.cash);
        tab.currentPlayer = false;
        tab.setUpPropertyGrid(propertyCards);
        return tab;
    }
    public void setName(string name)
    {
        this.player_name.SetText(name);
    }
    public void setMoney(int money)
    {
        this.player_money.SetText(money.ToString() + "Q");
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
        currentPlayer = false;
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
        currentPlayer = true;
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
        while(currentPlayer)
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
