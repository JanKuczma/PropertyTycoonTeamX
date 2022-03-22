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
        card.property = space;
        card.propertyName.text = space.name;
        card.oneLotTimesRent.text = "If one \"Utility\" is owned rent is "+space.rents[0].ToString()+" times the amount shown on dice.";
        card.twoLotTimesRent.text = "If both \"Utilities\" are owned rent is "+space.rents[1].ToString()+" times the amount shown on dice.";
        card.mortgage.text = (space.cost/2).ToString()+"Q";
        card.gameObject.SetActive(false);
        card.price.text = "PRICE "+space.cost.ToString() + "Q";
        return card;
    }
}   
}
