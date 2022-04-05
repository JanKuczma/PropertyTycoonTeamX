using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace View{
public class TaxSquare : FullSquare
{
    public TextMeshPro amount;
    public static TaxSquare Create(Transform parent, int position, string name, string amount)
    {
        TaxSquare square = Instantiate(Asset.Board(SqType.TAX,name),parent).GetComponent<TaxSquare>();
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = generateCoordinates(position);
        square.transform.localRotation = getRotation(position);
        square._position = position;
        square.setName(name);
        square.setAmount(amount);
        square.assignSpots();
        return square;
    }
    string _amount;
    public void setAmount(string amount)
    {
        _amount = amount;
        this.amount.SetText("PAY "+amount+"Q");
    }
}
}