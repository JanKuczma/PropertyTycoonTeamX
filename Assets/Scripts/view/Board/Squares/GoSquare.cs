using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoSquare : CornerSquare
{
    public override void setName(string amount="")
    {
        if(amount.Equals("")) amount = "200";
        _first = "COLLECT £"+amount+" ";
        GetComponentsInChildren<TextMeshPro>()[0].SetText(_first+_sceond);
    }
    public void setSecond(string second = "")
    {
        if(second.Equals("")) second = "SALARY AS YOU PASS";
        _sceond = second;
        GetComponentsInChildren<TextMeshPro>()[0].SetText(_first+_sceond);
    }
}
