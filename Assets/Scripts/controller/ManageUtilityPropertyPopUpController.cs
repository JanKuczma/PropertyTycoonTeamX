using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageUtilityPropertyPopUpController : MonoBehaviour
{
    Model.Player owner;
    Model.Space.Purchasable property;
    public Text mortgageBtnText;
    void Awake()
    {
        owner = transform.parent.GetComponent<View.PurchasableCard>().property.owner;
        property = transform.parent.GetComponent<View.PurchasableCard>().property;
        if(property.isMortgaged)
        {
            mortgageBtnText.text = "Pay Off Mortgage";
        }
    }
    public void buyHouseOption()
    {
        if(allPropertiesInGroup(((Model.Space.Property)(this.property)).group).Count != ownedPropertiesInGroup(((Model.Space.Property)(this.property)).group).Count)
        {
            MessagePopUp.Create("You need to own all the properties of this colour first!",transform);
        }
        else if(!differenceInHousesLevelBuyingOK(((Model.Space.Property)(this.property)).group,((Model.Space.Property)(this.property)).noOfHouses))
        {
            MessagePopUp.Create("The difference in number of houses on properties of the same colour cannot be bigger than one! Develop other properties of this colour!",transform);
        } 
        else if(((Model.Space.Property)(this.property)).noOfHouses == 5)
        {
            MessagePopUp.Create("Maximum number of houses reached!",transform);
        }
        else if(((Model.Space.Property)(this.property)).house_cost > owner.cash)
        {
            MessagePopUp.Create("You have not enough money! Sell or mortgage your properties to get some cash!",transform);
        }
        else if(((Model.Space.Property)(this.property)).noOfHouses == 4 && ((Model.Space.Property)(this.property)).hotel_cost > owner.cash)
        {
            MessagePopUp.Create("You have not enough money! Sell or mortgage your properties to get some cash!",transform);
        } else {
            if(((Model.Space.Property)(this.property)).noOfHouses == 4)
            {
                owner.PayCash(((Model.Space.Property)(property)).hotel_cost);
            } else {
                owner.PayCash(((Model.Space.Property)(property)).house_cost);
            }
            ((Model.Space.Property)(property)).noOfHouses += 1;
            transform.parent.GetComponent<View.PropertyCard>().showHouse(((Model.Space.Property)(property)).noOfHouses);
            MessagePopUp.Create("House bought!",transform.parent);
            Destroy(gameObject);
        }
    }
    public void sellHouseOption()
    {
        if(!differenceInHousesLevelSellingOK(((Model.Space.Property)(this.property)).group,((Model.Space.Property)(this.property)).noOfHouses))
        {
            MessagePopUp.Create("The difference in number of houses on properties of the same colour cannot be bigger than one! Sell houses on other properties of this colour first!",transform);
        } 
        else if(((Model.Space.Property)(this.property)).noOfHouses == 0)
        {
            MessagePopUp.Create("There are no more houses to sell!",transform);
        } else {
            if(((Model.Space.Property)(property)).noOfHouses == 5)
            {
                owner.ReceiveCash(((Model.Space.Property)(property)).hotel_cost);
            } else {
                owner.ReceiveCash(((Model.Space.Property)(property)).house_cost);
            }
            ((Model.Space.Property)(property)).noOfHouses -= 1;
            transform.parent.GetComponent<View.PropertyCard>().showHouse(((Model.Space.Property)(property)).noOfHouses);
            MessagePopUp.Create("House sold!",transform.parent);
            Destroy(gameObject);
        }
    }
    public void sellPropertyOption()
    {
        if(property is Model.Space.Property)
        {
            if(!differenceInHousesLevelSellingPropertyOK(((Model.Space.Property)(this.property)).group))
            {
                MessagePopUp.Create("First sell all the houses on the properties of this colour!",transform);
            } else {
                if(property.isMortgaged)
                {
                    owner.ReceiveCash(property.cost/2);
                    property.isMortgaged = false;
                } else { owner.ReceiveCash(property.cost); }
                owner.owned_spaces.Remove(property);
                property.owner = null;
                ((View.PropertySquare)(FindObjectOfType<temp_contr>().board_view.squares[property.position-1])).removeRibbon();
                transform.parent.gameObject.SetActive(false);
                MessagePopUp.Create("Property sold!",transform.parent.parent);
                Destroy(gameObject);
            }
        }
        else {
            if(property.isMortgaged)
            {
                owner.ReceiveCash(property.cost/2);
                property.isMortgaged = false;
            } else { owner.ReceiveCash(property.cost); }
            owner.owned_spaces.Remove(property);
            property.owner = null;
            ((View.UtilitySquare)(FindObjectOfType<temp_contr>().board_view.squares[property.position-1])).removeRibbon();
            transform.parent.gameObject.SetActive(false);
            MessagePopUp.Create("Property sold!",transform.parent.parent);
            Destroy(gameObject);
        }
    }
    public void mortgagePropertyOption()
    {
        Debug.Log(property.name);
        if(property.isMortgaged)
        {
            if(owner.cash < property.cost/2)
            {
                MessagePopUp.Create("You have not enough money! Sell or mortgage your properties to get some cash!",transform.parent);
            } else {
                MessagePopUp.Create("Property paid off!",transform.parent);
                owner.PayCash(property.cost/2);
                property.isMortgaged = false;
            }
        } else {
            MessagePopUp.Create("Property mortgaged!",transform.parent);
            owner.ReceiveCash(property.cost/2);
            property.isMortgaged = true;
        }
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        FindObjectOfType<View.HUD>().UpdatePlayersTabInfo();
    }




    // private methods
    List<Model.Space.Property> allPropertiesInGroup(Group group)
    {
        List<Model.Space.Property> spacesInGroup = new List<Model.Space.Property>();
        foreach(Model.Space space in FindObjectOfType<temp_contr>().board_model.spaces)
        {
            if(space is Model.Space.Property)
            {
                if(((Model.Space.Property)(space)).group == group)
                {
                    spacesInGroup.Add(((Model.Space.Property)(space)));
                }
            }
        }
        return spacesInGroup;
    }
    List<Model.Space.Property> ownedPropertiesInGroup(Group group)
    {
        List<Model.Space.Property> spacesInGroup = new List<Model.Space.Property>();
        foreach(Model.Space.Purchasable space in owner.owned_spaces)
        {
            if(space is Model.Space.Property)
            {
                if(((Model.Space.Property)(space)).group == group)
                {
                    spacesInGroup.Add(((Model.Space.Property)(space)));
                }
            }
        }
        return spacesInGroup;
    }

    bool differenceInHousesLevelBuyingOK(Group group, int houses)
    {
        foreach(Model.Space.Property prop in ownedPropertiesInGroup(group))
        {
            if(prop.noOfHouses < houses) { return false; }
        }
        return true;
    }
    bool differenceInHousesLevelSellingOK(Group group, int houses)
    {
        foreach(Model.Space.Property prop in ownedPropertiesInGroup(group))
        {
            if(prop.noOfHouses > houses) { return false; }
        }
        return true;
    }
    bool differenceInHousesLevelSellingPropertyOK(Group group)
    {
        foreach(Model.Space.Property prop in ownedPropertiesInGroup(group))
        {
            if(prop.noOfHouses != 0) { return false; }
        }
        return true;
    }
}
