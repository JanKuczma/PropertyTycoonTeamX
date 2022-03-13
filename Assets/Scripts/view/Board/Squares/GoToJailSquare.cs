using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace View{
public class GoToJailSquare : CornerSquare
{
    public static GoToJailSquare Create(Transform parent, int position, string name)
    {
        GoToJailSquare square = Instantiate(Asset.Board(SqType.GOTOJAIL),parent).GetComponent<GoToJailSquare>();
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = Square.generateCoordinates(position);
        square.transform.localRotation = getRotation(position);
        square.setName(name);
        square.assignSpots();
        return square;
    }
    public override void setName(string name)
    {
        GetComponentsInChildren<TextMeshPro>()[0].SetText("GO TO");
        GetComponentsInChildren<TextMeshPro>()[1].SetText("JAIL");
    }
}
}