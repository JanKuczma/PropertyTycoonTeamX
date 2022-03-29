using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageUtilityPropertyPopUpController : MonoBehaviour
{
    Model.Space.Purchasable property;
    public Text mortgageBtnText;
    void Awake()
    {
        property = transform.parent.GetComponent<View.PurchasableCard>().property;
        if(property.isMortgaged)
        {
            mortgageBtnText.text = "Pay Off Mortgage";
        }
    }
    public void buyHouseOption()
    {
        switch(((Model.Space.Property)(property)).buyHouse(FindObjectOfType<temp_contr>().board_model))
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
                transform.parent.GetComponent<View.PropertyCard>().showHouse(((Model.Space.Property)(property)).noOfHouses);
                ((View.PropertySquare)(FindObjectOfType<temp_contr>().board_view.squares[property.position-1])).addHouse();
                MessagePopUp.Create(transform.parent, "House bought!");
                Destroy(gameObject);
            break;
        }
    }
    public void sellHouseOption()
    {
        switch(((Model.Space.Property)(property)).sellHouse(FindObjectOfType<temp_contr>().board_model))
        {
            case Model.Decision_outcome.DIFFERENCE_IN_HOUSES:
                MessagePopUp.Create(transform, "The difference in number of houses on properties of the same colour cannot be bigger than one! Sell houses on other properties of this colour first!");
            break;
            case Model.Decision_outcome.NO_HOUSES:
                MessagePopUp.Create(transform, "There are no more houses to sell!");
            break;
            case Model.Decision_outcome.SUCCESSFUL:
                transform.parent.GetComponent<View.PropertyCard>().showHouse(((Model.Space.Property)(property)).noOfHouses);
                ((View.PropertySquare)(FindObjectOfType<temp_contr>().board_view.squares[property.position-1])).removeHouse();
                MessagePopUp.Create(transform.parent, "House sold!");
                Destroy(gameObject);
            break;
        }
    }
    public void sellPropertyOption()
    {
        switch(property.owner.SellProperty(property,FindObjectOfType<temp_contr>().board_model))
        {
            case Model.Decision_outcome.DIFFERENCE_IN_HOUSES:
                MessagePopUp.Create(transform, "First sell all the houses on the properties of this colour!");
            break;
            case Model.Decision_outcome.SUCCESSFUL:
                ((View.PropertySquare)(FindObjectOfType<temp_contr>().board_view.squares[property.position-1])).removeRibbon();
                transform.parent.gameObject.SetActive(false);
                MessagePopUp.Create(transform.parent.parent, "Property sold!");
                Destroy(gameObject);
            break;
        }
    }
    public void mortgagePropertyOption()
    {
        if(property.isMortgaged)
        {
            switch (property.pay_off_mortgage())
            {
                case Model.Decision_outcome.NOT_ENOUGH_MONEY:
                    MessagePopUp.Create(transform.parent, "You have not enough money! Sell or mortgage your properties to get some cash!");
                break;
                case Model.Decision_outcome.SUCCESSFUL:
                    MessagePopUp.Create(transform.parent, "Property paid off!");
                break;
            }
        } else {
            property.mortgage();
            MessagePopUp.Create(transform.parent, "Property mortgaged!");
        }
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        FindObjectOfType<View.HUD>().UpdatePlayersTabInfo();
    }
}
