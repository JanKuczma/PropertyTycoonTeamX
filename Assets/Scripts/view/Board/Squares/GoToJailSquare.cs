using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace View{
public class GoToJailSquare : CornerSquare
{
    public override void setName(string name)
    {
        GetComponentsInChildren<TextMeshPro>()[0].SetText("GO TO");
        GetComponentsInChildren<TextMeshPro>()[1].SetText("JAIL");
    }
}
}