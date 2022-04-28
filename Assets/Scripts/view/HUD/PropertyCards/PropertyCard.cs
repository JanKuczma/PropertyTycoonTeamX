using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
public class PropertyCard : PurchasableCard
{
    public Image group;
    public Text rent;
    public Text oneHouseRent;
    public Text twoHousesRent;
    public Text threeHousesRent;
    public Text fourHousesRent;
    public Text hotelRent;
    public Text houseCost;
    public Text hotelCost;
    public Text note;
    [SerializeField] public Image[] housesIMGs;

    public static PropertyCard Create(Model.Space.Property space,Transform parent)
    {
        PropertyCard card = Instantiate(Asset.PropertyCard,parent).GetComponent<PropertyCard>();
        card.setUpCard(space);
        card.gameObject.SetActive(false);
        return card;
    }

    public void showHouse(int noOfHouses)
    {
        for(int i = 0; i < housesIMGs.Length; i++) { housesIMGs[i].gameObject.SetActive(false); }
        if(noOfHouses == 5) { housesIMGs[4].gameObject.SetActive(true); return; }
        for(int i = 0; i < noOfHouses; i++) { housesIMGs[i].gameObject.SetActive(true); }
    }

    public void setUpCard(Model.Space.Property space)
    {
        this.property = space;
        this.propertyName.text = space.name;
        Color color;
        if ( ColorUtility.TryParseHtmlString("#"+((int)space.group).ToString("X")+"FF", out color))
        { this.group.color = color; }
        this.price.text = "PRICE "+space.cost.ToString() + "Q";
        this.rent.text = "Rent " + space.rents[0].ToString()+"Q";
        this.oneHouseRent.text = space.rents[1].ToString()+"Q";
        this.twoHousesRent.text = space.rents[2].ToString()+"Q";
        this.threeHousesRent.text = space.rents[3].ToString()+"Q";
        this.fourHousesRent.text = space.rents[4].ToString()+"Q";
        this.hotelRent.text = space.rents[5].ToString()+"Q";
        this.houseCost.text = "Housecost "+space.house_cost.ToString()+"Q each";
        this.hotelCost.text = "Hotel cost "+space.house_cost.ToString()+"Q plus 4 houses";
        this.note.text = "If a player owns ALL the Lots of any Color-Group, the renst is Doubled on Unimproved Lots in that Group.";
        this.mortgage.text = (space.cost/2).ToString()+"Q";
    }
    public void setUpCard(PropertyCard card)
    {
        this.property = card.property;
        this.propertyName.text = card.propertyName.text;
        this.group.color = card.group.color;
        this.price.text = card.price.text;
        this.rent.text = card.rent.text;
        this.oneHouseRent.text = card.oneHouseRent.text;
        this.twoHousesRent.text = card.twoHousesRent.text;
        this.threeHousesRent.text = card.threeHousesRent.text;
        this.fourHousesRent.text = card.fourHousesRent.text;
        this.hotelRent.text = card.hotelRent.text;
        this.houseCost.text = card.houseCost.text;
        this.hotelCost.text = card.hotelCost.text;
        this.note.text = card.note.text;
        this.mortgage.text = card.mortgage.text;
    }
}   
}
