using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UtilitySqaure : FullSquare
{
    string _price;
    public void setPrice(string price)
    {
        _price = price;
        GetComponentsInChildren<TextMeshPro>()[1].SetText("£"+price);
    }
}
