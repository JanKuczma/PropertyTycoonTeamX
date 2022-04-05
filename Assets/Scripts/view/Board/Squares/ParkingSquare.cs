using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace View{
public class ParkingSquare : CornerSquare
{
    public static ParkingSquare Create(Transform parent, int position, string name)
    {
        ParkingSquare square = Instantiate(Asset.Board(SqType.PARKING),parent).GetComponent<ParkingSquare>();
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = Square.generateCoordinates(position);
        square.transform.localRotation = getRotation(position);
        square._position = position;
        square.setName(name);
        square.assignSpots();
        return square;
    }
    public override void setName(string name)
    {
        GetComponentsInChildren<TextMeshPro>()[0].SetText("FREE");
        GetComponentsInChildren<TextMeshPro>()[1].SetText("PARKING");
    }

}
}
