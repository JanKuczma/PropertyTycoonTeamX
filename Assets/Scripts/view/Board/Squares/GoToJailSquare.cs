using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace View{
public class GoToJailSquare : CornerSquare
{
    public override void setName(string goTo = "")
    {
        if(goTo.Equals("")) goTo = "GO TO";
        _first = goTo;
        GetComponentsInChildren<TextMeshPro>()[0].SetText(goTo);
    }

    public void setJailText(string jail = "")
    {
        if(jail.Equals("")) jail = "JAIL";
        _sceond = jail;
        GetComponentsInChildren<TextMeshPro>()[1].SetText(jail);
    }
}
}