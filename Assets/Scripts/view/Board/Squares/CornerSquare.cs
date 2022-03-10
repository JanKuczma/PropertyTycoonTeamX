using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace View{
public class CornerSquare : Square
{
    protected string _first = "";
    protected string _sceond = "";
    
    virtual public void assignSpots()
    {
        float offsetSmall = offsetS*(transform.localScale.x);
        float offsetBig = offsetB*(transform.localScale.x);

        spots[0] = transform.position + transform.right*(offsetBig + offsetSmall/2);
        spots[1] = transform.position + transform.right*offsetBig + transform.forward*(offsetBig + offsetSmall/2);
        spots[2] = transform.position + transform.right*offsetBig - transform.forward*(offsetBig + offsetSmall/2);
        spots[3] = transform.position - transform.right*offsetBig;
        spots[4] = transform.position - transform.right*(offsetBig + offsetSmall/2) + transform.forward*(offsetBig + offsetSmall/2);
        spots[5] = transform.position - transform.right*(offsetBig + offsetSmall/2) - transform.forward*(offsetBig + offsetSmall/2);
    }
}
}
