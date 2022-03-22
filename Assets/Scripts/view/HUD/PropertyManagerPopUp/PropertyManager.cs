using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View 
{
    public class PropertyManager : MonoBehaviour
    {
    // Start is called before the first frame update
    [SerializeField] public DictionaryWrapper<int,PurchasableCard> cards;

    public static PropertyManager Create(Transform parent, Model.Player player, Dictionary<int,PurchasableCard> propertyCards)
    {
        PropertyManager manager = Instantiate(Asset.propertyManager(),parent).GetComponent<PropertyManager>();
        manager.setUpCards(player,propertyCards);
        return manager;
    }

    void setUpCards(Model.Player player, Dictionary<int,PurchasableCard> propertyCards)
    {
        foreach(KeyValuePair<int, PurchasableCard> entry in propertyCards)
        {
            if(player.owned_spaces.Contains(entry.Value.property))
            //if(!player.owned_spaces.Contains(entry.Value.property))
            {
                cards.getValue(entry.Key).gameObject.SetActive(false);
            } else {
                cards.getValue(entry.Key).gameObject.SetActive(true);
                cards.getValue(entry.Key).property = entry.Value.property;
                cards.getValue(entry.Key).propertyName.text = entry.Value.propertyName.text;
                cards.getValue(entry.Key).price.text = entry.Value.price.text;
                cards.getValue(entry.Key).mortgage.text = entry.Value.mortgage.text;
                //change color or whatever
                switch(entry.Value.property.type)
                {
                    case SqType.PROPERTY:
                    ((PropertyCard)(cards.getValue(entry.Key))).group.color = ((PropertyCard)(entry.Value)).group.color;
                    ((PropertyCard)(cards.getValue(entry.Key))).rent.text = ((PropertyCard)(entry.Value)).rent.text;
                    ((PropertyCard)(cards.getValue(entry.Key))).oneHouseRent.text = ((PropertyCard)(entry.Value)).oneHouseRent.text;
                    ((PropertyCard)(cards.getValue(entry.Key))).twoHousesRent.text = ((PropertyCard)(entry.Value)).twoHousesRent.text;
                    ((PropertyCard)(cards.getValue(entry.Key))).threeHousesRent.text = ((PropertyCard)(entry.Value)).threeHousesRent.text;
                    ((PropertyCard)(cards.getValue(entry.Key))).fourHousesRent.text = ((PropertyCard)(entry.Value)).rent.text;
                    ((PropertyCard)(cards.getValue(entry.Key))).hotelRent.text = ((PropertyCard)(entry.Value)).hotelRent.text;
                    ((PropertyCard)(cards.getValue(entry.Key))).houseCost.text = ((PropertyCard)(entry.Value)).houseCost.text;
                    ((PropertyCard)(cards.getValue(entry.Key))).hotelCost.text = ((PropertyCard)(entry.Value)).hotelCost.text;
                    ((PropertyCard)(cards.getValue(entry.Key))).note.text = ((PropertyCard)(entry.Value)).note.text;
                    ((PropertyCard)(cards.getValue(entry.Key))).showHouse(((Model.Space.Property)(cards.getValue(entry.Key).property)).noOfHouses);
                    break;
                    case SqType.STATION:
                    ((StationCard)(cards.getValue(entry.Key))).oneStationRent.text = ((StationCard)(entry.Value)).oneStationRent.text;
                    ((StationCard)(cards.getValue(entry.Key))).twoStationsRent.text = ((StationCard)(entry.Value)).twoStationsRent.text;
                    ((StationCard)(cards.getValue(entry.Key))).threeStationsRent.text = ((StationCard)(entry.Value)).threeStationsRent.text;
                    ((StationCard)(cards.getValue(entry.Key))).fourStationsRent.text = ((StationCard)(entry.Value)).fourStationsRent.text;
                    break;
                    case SqType.UTILITY:
                    ((UtilityCard)(cards.getValue(entry.Key))).oneLotTimesRent.text = ((UtilityCard)(entry.Value)).oneLotTimesRent.text;
                    ((UtilityCard)(cards.getValue(entry.Key))).twoLotTimesRent.text = ((UtilityCard)(entry.Value)).twoLotTimesRent.text;
                    break;
                }
            }
        }
    }

    public void destroy()
    {
        Destroy(this.gameObject);
    }
}
}
