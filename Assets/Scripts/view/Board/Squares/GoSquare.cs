using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace View{
public class GoSquare : CornerSquare
{
    public override void setName(string name="")
    {
        GetComponentsInChildren<TextMeshPro>()[0].SetText("COLLECT £200 SALARY AS YOU PASS");
    }
    public void setAmount(string amount = "200")
    {
        GetComponentsInChildren<TextMeshPro>()[0].SetText("COLLECT £"+amount+" SALARY AS YOU PASS");
    }
}
}