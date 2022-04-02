using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace View{
public class UtilitySquare : FullSquare
{
    public TextMeshPro amount;
    string _price;
    public GameObject ribbon;

    new public static UtilitySquare Create(SqType type, Transform parent, int position, string name,string price)
    {
        UtilitySquare square = Instantiate(Asset.Board(type,name),parent).GetComponent<UtilitySquare>();
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = Square.generateCoordinates(position);
        square.transform.localRotation = getRotation(position);
        square.setName(name);
        square._position = position;
        square.assignSpots();
        square.setPrice(price);
        return square;
    }
    public void setPrice(string price)
    {
        _price = price;
        this.amount.SetText(price+"Q");
    }

    public void showRibbon(Color color)
    {
        ribbon.SetActive(true);
        color.a = 150f/255f;
        ribbon.GetComponent<Renderer>().material.SetColor("_Color",color);
    }

    public void removeRibbon()
    {
        ribbon.SetActive(false);
    }
}
}
