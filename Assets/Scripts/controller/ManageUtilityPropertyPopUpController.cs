using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageUtilityPropertyPopUpController : MonoBehaviour
{
    Model.Player owner;
    Model.Space.Purchasable property;
    void Awake()
    {
        owner = transform.parent.GetComponent<View.PurchasableCard>().property.owner;
        property = transform.parent.GetComponent<View.PurchasableCard>().property;
    }
    public void buyHouseOption()
    {
        Debug.Log("no of houses: " + ((Model.Space.Property)(property)).noOfHouses);
        Debug.Log("House bought!");
        Destroy(gameObject);
    }
    public void sellHouseOption()
    {
        Debug.Log("no of houses: " + ((Model.Space.Property)(property)).noOfHouses);
        Debug.Log("House sold!");
        Destroy(gameObject);
    }
    public void sellPropertyOption()
    {
        Debug.Log(property.name);
        Debug.Log("Property sold!");
        Destroy(gameObject);
    }
    public void mortgagePropertyOption()
    {
        Debug.Log(property.name);
        Debug.Log("Property mortgaged!");
        Destroy(gameObject);
    }
}
