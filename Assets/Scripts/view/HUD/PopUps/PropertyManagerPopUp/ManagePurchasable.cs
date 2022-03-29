using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace View
{
public class ManagePurchasable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDeselectHandler
{
    protected ManagePurchasable PopUp;
    bool isPointerOver = false;

    PurchasableCard card;
    Model.Board board;
    public View.Square square;
    public Button mortgageBtn;
    public Button sellBtn;
    public Button buyHouseBtn;
    public Button sellHouseBtn;

    void Start()
    {
        temp_contr controller = FindObjectOfType<temp_contr>(); 
        card = GetComponent<PurchasableCard>();
        board = controller.board_model;
        square = controller.board_view.squares[card.property.position-1];
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
        if(PopUp == null)
        {
            if(card is PropertyCard)
            {
                PopUp = Instantiate(Asset.ManagePropertyPopUpPrefab,card.transform).GetComponent<ManagePurchasable>();
                PopUp.mortgageBtn.onClick.AddListener(() => mortgagePropertyOption(card.property.mortgage()));
                PopUp.sellBtn.onClick.AddListener(() => sellPropertyOption(card.property.owner.SellProperty(card.property,board),(View.PropertySquare)square));
                PopUp.buyHouseBtn.onClick.AddListener(() => buyHouseOption(card.property.owner.BuyProperty(card.property),(Model.Space.Property)card.property,(View.PropertySquare)square));
                PopUp.sellHouseBtn.onClick.AddListener(() => sellHouseOption(card.property.owner.BuyProperty(card.property),(Model.Space.Property)card.property,(View.PropertySquare)square));
            } else {
                PopUp = Instantiate(Asset.ManageUtilityPopUpPrefab,card.transform).GetComponent<ManagePurchasable>();
                PopUp.mortgageBtn.onClick.AddListener(() => mortgagePropertyOption(card.property.mortgage()));
                PopUp.sellBtn.onClick.AddListener(() => sellPropertyOption(card.property.owner.SellProperty(card.property,board),(View.PropertySquare)square));
            }
        }
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
            Destroy(PopUp.gameObject);
            PopUp = null;
        }
    }

        public void buyHouseOption(Model.Decision_outcome decision, Model.Space.Property space, View.PropertySquare square)
    {
        switch(decision) //((Model.Space.Property)(card.property)).buyHouse(FindObjectOfType<temp_contr>().board_model)
        {
            case Model.Decision_outcome.NOT_ALL_PROPERTIES_IN_GROUP:
                MessagePopUp.Create(transform, "You need to own all the properties of this colour first!");
            break;
            case Model.Decision_outcome.DIFFERENCE_IN_HOUSES:
                MessagePopUp.Create(transform, "The difference in number of houses on properties of the same colour cannot be bigger than one! Develop other properties of this colour!");
            break;
            case Model.Decision_outcome.MAX_HOUSES:
                MessagePopUp.Create(transform, "Maximum number of houses reached!");
            break;
            case Model.Decision_outcome.NOT_ENOUGH_MONEY:
                MessagePopUp.Create(transform, "You have not enough money! Sell or mortgage your properties to get some cash!");
            break;
            case Model.Decision_outcome.SUCCESSFUL:
                transform.parent.GetComponent<View.PropertyCard>().showHouse(space.noOfHouses);
                square.addHouse();
                MessagePopUp.Create(transform.parent, "House bought!");
                Destroy(gameObject);
            break;
        }
    }
    public void sellHouseOption(Model.Decision_outcome decision, Model.Space.Property space, View.PropertySquare square)
    {
        switch(decision)
        {
            case Model.Decision_outcome.DIFFERENCE_IN_HOUSES:
                MessagePopUp.Create(transform, "The difference in number of houses on properties of the same colour cannot be bigger than one! Sell houses on other properties of this colour first!");
            break;
            case Model.Decision_outcome.NO_HOUSES:
                MessagePopUp.Create(transform, "There are no more houses to sell!");
            break;
            case Model.Decision_outcome.SUCCESSFUL:
                transform.parent.GetComponent<View.PropertyCard>().showHouse(space.noOfHouses);
                square.removeHouse();
                MessagePopUp.Create(transform.parent, "House sold!");
                Destroy(gameObject);
            break;
        }
    }
    public void sellPropertyOption(Model.Decision_outcome desicision, View.Square square)
    {
        switch(desicision)
        {
            case Model.Decision_outcome.DIFFERENCE_IN_HOUSES:
                MessagePopUp.Create(transform, "First sell all the houses on the properties of this colour!");
            break;
            case Model.Decision_outcome.SUCCESSFUL:
                if(square is View.PropertySquare) { ((View.PropertySquare)(square)).removeRibbon(); } else { ((View.UtilitySquare)(square)).removeRibbon(); }
                transform.parent.gameObject.SetActive(false);
                MessagePopUp.Create(transform.parent.parent, "Property sold!");
                Destroy(gameObject);
            break;
        }
    }
    public void mortgagePropertyOption(Model.Decision_outcome desicision)
    {
        if(card.property.isMortgaged)
        {
            switch (desicision)
            {
                case Model.Decision_outcome.NOT_ENOUGH_MONEY:
                    MessagePopUp.Create(transform.parent, "You have not enough money! Sell or mortgage your properties to get some cash!");
                break;
                case Model.Decision_outcome.SUCCESSFUL:
                    MessagePopUp.Create(transform.parent, "Property paid off!");
                break;
            }
        } else {
            MessagePopUp.Create(transform.parent, "Property mortgaged!");
        }
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        FindObjectOfType<View.HUD>().UpdatePlayersTabInfo();
    }
}
}
