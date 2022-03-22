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
        card.setUpCard(space);
        card.gameObject.SetActive(false);
        return card;
    }

    public void setUpCard(Model.Space.Station space)
    {
        this.propertyName.text = space.name;
        this.property = space;
        this.price.text = "PRICE "+space.cost.ToString() + "Q";
        this.oneStationRent.text = space.rents[0].ToString()+"Q";
        this.twoStationsRent.text = space.rents[1].ToString()+"Q";
        this.threeStationsRent.text = space.rents[2].ToString()+"Q";
        this.fourStationsRent.text = space.rents[3].ToString()+"Q";
        this.mortgage.text = (space.cost/2).ToString()+"Q";
    }

    public void setUpCard(StationCard card)
    {
        this.propertyName.text = card.propertyName.text;
        this.property = card.property;
        this.price.text = card.price.text;
        this.oneStationRent.text = card.oneStationRent.text;
        this.twoStationsRent.text = card.twoStationsRent.text;
        this.threeStationsRent.text = card.threeStationsRent.text;
        this.fourStationsRent.text = card.fourStationsRent.text;
        this.mortgage.text = card.mortgage.text;
    }
}   
}
