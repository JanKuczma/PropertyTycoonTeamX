using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace View{
public class UtilitySquare : FullSquare
{
    string _price;

    new public static UtilitySquare Create(SqType type, Transform parent, int position, string name,string price)
    {
        UtilitySquare square = Instantiate(Asset.Board(type,name),parent).GetComponent<UtilitySquare>();
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = Square.generateCoordinates(position);
        square.transform.localRotation = getRotation(position);
        square.setName(name);
        square.assignSpots();
        square.setPrice(price);
        return square;
    }
    public void setPrice(string price)
    {
        _price = price;
        GetComponentsInChildren<TextMeshPro>()[1].SetText("£"+price);
    }
}
}
