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

    public static PropertyCard Create(Model.Space.Property space,Transform parent)
    {
        PropertyCard card = Instantiate(Asset.PropertyCard,parent).GetComponent<PropertyCard>();
        card.propertyName.text = space.name;
        Color color;
        if ( ColorUtility.TryParseHtmlString("#"+((int)space.group).ToString("X")+"FF", out color))
        { card.group.color = color; }
        card.price.text = "PRICE "+space.cost.ToString() + "Q";
        card.rent.text = "Rent " + space.rents[0].ToString()+"Q";
        card.oneHouseRent.text = space.rents[1].ToString()+"Q";
        card.twoHousesRent.text = space.rents[2].ToString()+"Q";
        card.threeHousesRent.text = space.rents[3].ToString()+"Q";
        card.fourHousesRent.text = space.rents[4].ToString()+"Q";
        card.hotelRent.text = space.rents[5].ToString()+"Q";
        card.houseCost.text = "Housecost "+space.house_cost.ToString()+"Q each";
        card.hotelCost.text = "Hotel cost "+(space.hotel_cost-(4*space.house_cost)).ToString()+"Q each plus 4 houses";
        card.note.text = "If a player owns ALL the Lots of any Color-Group, the renst is Doubled on Unimproved Lots in that Group.";
        card.mortgage.text = "";
        card.mortgage.text = (space.cost/2).ToString()+"Q";
        card.gameObject.SetActive(false);
        return card;
    }
}   
}
