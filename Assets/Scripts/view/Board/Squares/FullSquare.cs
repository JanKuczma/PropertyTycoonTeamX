using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullSquare : Square
{
    new void Awake()
    {
        base.Awake();
        assignSpots();
    }
        private void assignSpots()
    {
        float offsetSmall = offsetS*transform.localScale.x;
        float offsetBig = offsetB*transform.localScale.x;

        spots[0] = transform.position + transform.right*offsetSmall + transform.forward*(offsetSmall/5);
        spots[1] = transform.position + transform.right*offsetSmall + transform.forward*(offsetBig + offsetSmall/2) + transform.forward*(offsetSmall/5);
        spots[2] = transform.position + transform.right*offsetSmall - transform.forward*(offsetBig + offsetSmall/2) + transform.forward*(offsetSmall/5);
        spots[3] = transform.position - transform.right*offsetSmall - transform.forward*(offsetSmall/5);
        spots[4] = transform.position - transform.right*offsetSmall + transform.forward*(offsetBig + offsetSmall/2) - transform.forward*(offsetSmall/5);
        spots[5] = transform.position - transform.right*offsetSmall - transform.forward*(offsetBig + offsetSmall/2) - transform.forward*(offsetSmall/5);
    }
}
