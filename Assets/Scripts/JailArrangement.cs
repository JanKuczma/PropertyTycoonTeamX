using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailArrangement : SquareArrangement
{
    void Start()
    {
        spotsIs = new List<int> {0,1,2,3,4,5};
        spots = new Vector3[6];
        // the offsets adjust according to the square scale
        float offsetSmall = 0.6f*(transform.localScale.x/100);
        float offsetBig = 0.8f*(transform.localScale.x/100);

        spots[0] = transform.position - Vector3.forward*(offsetBig+offsetSmall) + Vector3.right*(offsetSmall/2.0f);
        spots[1] = transform.position - Vector3.forward*(offsetBig+offsetSmall) - Vector3.right*offsetSmall;
        spots[2] = transform.position - Vector3.right*(offsetBig+offsetSmall) - Vector3.forward*(offsetBig+offsetSmall);
        spots[3] = transform.position + Vector3.right*(offsetSmall/2.0f);
        spots[4] = transform.position - Vector3.right*offsetSmall;
        spots[5] = transform.position - Vector3.right*(offsetBig+offsetSmall);
        
    }
}
