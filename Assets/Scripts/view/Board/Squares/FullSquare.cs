using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View{
public class FullSquare : Square
{
        public void assignSpots()
    {
        float offsetSmall = offsetS*transform.localScale.x;
        float offsetBig = offsetB*transform.localScale.x;

        spots[0] = transform.position + transform.right*offsetSmall;
        spots[1] = transform.position - transform.right*offsetSmall + transform.forward*(offsetBig + offsetSmall/2);
        spots[2] = transform.position - transform.right*offsetSmall - transform.forward*(offsetBig + offsetSmall/2);
        spots[3] = transform.position + transform.right*offsetSmall + transform.forward*(offsetBig + offsetSmall/2);
        spots[4] = transform.position - transform.right*offsetSmall;
        spots[5] = transform.position + transform.right*offsetSmall - transform.forward*(offsetBig + offsetSmall/2);
    }
}
}