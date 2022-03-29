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
                MessagePopUp.Create("You need to own all the properties of this colour first!",transform);
            break;
            case Model.Decision_outcome.DIFFERENCE_IN_HOUSES:
                MessagePopUp.Create("The difference in number of houses on properties of the same colour cannot be bigger than one! Develop other properties of this colour!",transform);
            break;
            case Model.Decision_outcome.MAX_HOUSES:
                MessagePopUp.Create("Maximum number of houses reached!",transform);
            break;
            case Model.Decision_outcome.NOT_ENOUGH_MONEY:
                MessagePopUp.Create("You have not enough money! Sell or mortgage your properties to get some cash!",transform);
            break;
            case Model.Decision_outcome.SUCCESSFUL:
                transform.parent.GetComponent<View.PropertyCard>().showHouse(((Model.Space.Property)(property)).noOfHouses);
                ((View.PropertySquare)(FindObjectOfType<temp_contr>().board_view.squares[property.position-1])).addHouse();
                MessagePopUp.Create("House bought!",transform.parent);
                Destroy(gameObject);
            break;
        }
    }
    public void sellHouseOption()
    {
        switch(((Model.Space.Property)(property)).sellHouse(FindObjectOfType<temp_contr>().board_model))
        {
            case Model.Decision_outcome.DIFFERENCE_IN_HOUSES:
                MessagePopUp.Create("The difference in number of houses on properties of the same colour cannot be bigger than one! Sell houses on other properties of this colour first!",transform);
            break;
            case Model.Decision_outcome.NO_HOUSES:
                MessagePopUp.Create("There are no more houses to sell!",transform);
            break;
            case Model.Decision_outcome.SUCCESSFUL:
                transform.parent.GetComponent<View.PropertyCard>().showHouse(((Model.Space.Property)(property)).noOfHouses);
                ((View.PropertySquare)(FindObjectOfType<temp_contr>().board_view.squares[property.position-1])).removeHouse();
                MessagePopUp.Create("House sold!",transform.parent);
                Destroy(gameObject);
            break;
        }
    }
    public void sellPropertyOption()
    {
        switch(property.owner.SellProperty(property,FindObjectOfType<temp_contr>().board_model))
        {
            case Model.Decision_outcome.DIFFERENCE_IN_HOUSES:
                MessagePopUp.Create("First sell all the houses on the properties of this colour!",transform);
            break;
            case Model.Decision_outcome.SUCCESSFUL:
                ((View.PropertySquare)(FindObjectOfType<temp_contr>().board_view.squares[property.position-1])).removeRibbon();
                transform.parent.gameObject.SetActive(false);
                MessagePopUp.Create("Property sold!",transform.parent.parent);
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
                    MessagePopUp.Create("You have not enough money! Sell or mortgage your properties to get some cash!",transform.parent);
                break;
                case Model.Decision_outcome.SUCCESSFUL:
                    MessagePopUp.Create("Property paid off!",transform.parent);
                break;
            }
        } else {
            property.mortgage();
            MessagePopUp.Create("Property mortgaged!",transform.parent);
        }
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        FindObjectOfType<View.HUD>().UpdatePlayersTabInfo();
    }
}
