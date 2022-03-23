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
    public Text ownedByText;

    public static PropertyManager Create(Transform parent, Model.Player player, Dictionary<int,PurchasableCard> propertyCards)
    {
        PropertyManager manager = Instantiate(Asset.propertyManager(),parent).GetComponent<PropertyManager>();
        manager.ownedByText.text = "Propierties owned by " + player.name;
        manager.setUpCards(player,propertyCards);
        return manager;
    }

    void setUpCards(Model.Player player, Dictionary<int,PurchasableCard> propertyCards)
    {
        foreach(KeyValuePair<int, PurchasableCard> entry in propertyCards)
        {
            if(Random.Range(0,2) == 1)
            //if(!player.owned_spaces.Contains(entry.Value.property))
            {
                cards.getValue(entry.Key).gameObject.SetActive(false);
            } else {
                cards.getValue(entry.Key).gameObject.SetActive(true);
                //change color or whatever
                switch(entry.Value.property.type)
                {
                    case SqType.PROPERTY:
                    ((PropertyCard)(cards.getValue(entry.Key))).setUpCard((PropertyCard)entry.Value);
                    //((PropertyCard)(cards.getValue(entry.Key))).showHouse(((Model.Space.Property)(cards.getValue(entry.Key).property)).noOfHouses);
                    ((PropertyCard)(cards.getValue(entry.Key))).showHouse(Random.Range(0,6));
                    break;
                    case SqType.STATION:
                    ((StationCard)(cards.getValue(entry.Key))).setUpCard((StationCard)entry.Value);
                    break;
                    case SqType.UTILITY:
                    ((UtilityCard)(cards.getValue(entry.Key))).setUpCard((UtilityCard)entry.Value);
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
