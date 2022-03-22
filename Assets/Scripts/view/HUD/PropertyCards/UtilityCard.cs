using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
public class UtilityCard : PurchasableCard
{
    public Text oneLotTimesRent;
    public Text twoLotTimesRent;

    public static UtilityCard Create(Model.Space.Utility space,Transform parent)
    {
        UtilityCard card = Instantiate(Asset.UtilityCard(space.name),parent).GetComponent<UtilityCard>();
        card.setUpCard(space);
        card.gameObject.SetActive(false);
        return card;
    }

    public void setUpCard(Model.Space.Utility space)
    {
        this.property = space;
        this.propertyName.text = space.name;
        this.oneLotTimesRent.text = "If one \"Utility\" is owned rent is "+space.rents[0].ToString()+" times the amount shown on dice.";
        this.twoLotTimesRent.text = "If both \"Utilities\" are owned rent is "+space.rents[1].ToString()+" times the amount shown on dice.";
        this.mortgage.text = (space.cost/2).ToString()+"Q";
        this.price.text = "PRICE "+space.cost.ToString() + "Q";
    }

    public void setUpCard(UtilityCard card)
    {
        this.property = card.property;
        this.propertyName.text = card.propertyName.text;
        this.oneLotTimesRent.text = card.oneLotTimesRent.text;
        this.twoLotTimesRent.text = card.twoLotTimesRent.text;
        this.mortgage.text = card.mortgage.text;
        this.price.text = card.price.text;
    }
}   
}
