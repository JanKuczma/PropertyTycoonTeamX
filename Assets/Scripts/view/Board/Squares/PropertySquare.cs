using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertySquare : Square
{
    string _group;
    string _price;
    public void setGroup(string group) { _group = group; }
    public void setPrice(string price) { _price = price; }
    public void assignSpots()
    {
        float offsetSmall = offsetS*transform.localScale.x;
        float offsetBig = offsetB*transform.localScale.x;

        spots[0] = transform.position + transform.right*offsetSmall + transform.forward*(offsetSmall/2);
        spots[1] = transform.position + transform.right*offsetSmall + transform.forward*(offsetBig + offsetSmall/2);
        spots[2] = transform.position + transform.right*offsetSmall - transform.forward*(offsetBig -offsetSmall/2);
        spots[3] = transform.position - transform.right*offsetSmall + transform.forward*(offsetSmall/2);
        spots[4] = transform.position - transform.right*offsetSmall + transform.forward*(offsetBig + offsetSmall/2);
        spots[5] = transform.position - transform.right*offsetSmall - transform.forward*(offsetBig - offsetSmall/2);
    }
}
