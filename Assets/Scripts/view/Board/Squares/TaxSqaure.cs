using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaxSqaure : FullSquare
{
    string _amount;
    public void setAmount(string amount)
    {
        _amount = amount;
        GetComponentsInChildren<TextMeshPro>()[1].SetText("PAY £"+amount);
    }
}
