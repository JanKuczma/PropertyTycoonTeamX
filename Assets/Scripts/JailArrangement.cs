using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailArrangement : MonoBehaviour
{
    public Vector3[] spaces;
    public List<int> freeSpaces;
    void Start()
    {
        freeSpaces = new List<int> {0,1,2,3,4,5};
        spaces = new Vector3[6];
        // the offsets adjust according to the square scale
        float offsetSmall = 0.6f*(transform.localScale.x/100);
        float offsetBig = 0.8f*(transform.localScale.x/100);

        spaces[0] = transform.position - Vector3.forward*(offsetBig+offsetSmall) + Vector3.right*(offsetSmall/2.0f);
        spaces[1] = transform.position - Vector3.forward*(offsetBig+offsetSmall) - Vector3.right*offsetSmall;
        spaces[2] = transform.position - Vector3.right*(offsetBig+offsetSmall) - Vector3.forward*(offsetBig+offsetSmall);
        spaces[3] = transform.position + Vector3.right*(offsetSmall/2.0f);
        spaces[4] = transform.position - Vector3.right*offsetSmall;
        spaces[5] = transform.position - Vector3.right*(offsetBig+offsetSmall);
        
    }

    // Update is called once per frame
    public int nextFree()
    {
        return freeSpaces[Random.Range(0,freeSpaces.Count)];
    }
}
