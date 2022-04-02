using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace View{
public class GoSquare : CornerSquare
{
    public static GoSquare Create(Transform parent, int position, string name, string amount)
    {
        GoSquare square = Instantiate(Asset.Board(SqType.GO),parent).GetComponent<GoSquare>();
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = Square.generateCoordinates(position);
        square.transform.localRotation = getRotation(position);
        square._position = position;
        square.assignSpots();
        square.setAmount(amount);
        return square;
    }
    public override void setName(string name="")
    {
        square_name.SetText("COLLECT 200Q SALARY AS YOU PASS");
    }
    public void setAmount(string amount = "200")
    {
        square_name.SetText("COLLECT "+amount+"Q SALARY AS YOU PASS");
    }
}
}