using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
public class StationCard : PurchasableCard
{
    public Text oneStationRent;
    public Text twoStationsRent;
    public Text threeStationsRent;
    public Text fourStationsRent;
    public Image image;

    public static StationCard Create(Model.Space.Station space,Transform parent)
    {
        StationCard card = Instantiate(Asset.StationCard,parent).GetComponent<StationCard>();
        card.propertyName.text = space.name;
        card.oneStationRent.text = space.rents[0].ToString()+"Q";
        card.twoStationsRent.text = space.rents[1].ToString()+"Q";
        card.threeStationsRent.text = space.rents[2].ToString()+"Q";
        card.fourStationsRent.text = space.rents[3].ToString()+"Q";
        card.mortgage.text = (space.cost/2).ToString()+"Q";
        card.gameObject.SetActive(false);
        card.price.text = "PRICE "+space.cost.ToString() + "Q";
        return card;
    }
}   
}
