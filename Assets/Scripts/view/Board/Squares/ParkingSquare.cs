using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace View{
public class ParkingSquare : CornerSquare
{
    public override void setName(string name)
    {
        GetComponentsInChildren<TextMeshPro>()[0].SetText("FREE");
        GetComponentsInChildren<TextMeshPro>()[1].SetText("PARKING");
    }

}
}
