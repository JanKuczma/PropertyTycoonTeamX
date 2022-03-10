using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace View{
public class ParkingSquare : CornerSquare
{
    public override void setName(string free="")
    {
        if(free.Equals("")) free = "FREE";
        _first = free;
        GetComponentsInChildren<TextMeshPro>()[0].SetText(free);
    }

    public void setVisiting(string parking="")
    {
        if(parking.Equals("")) parking = "PARKING";
        _sceond = parking;
        GetComponentsInChildren<TextMeshPro>()[1].SetText(parking);
    }
}
}
