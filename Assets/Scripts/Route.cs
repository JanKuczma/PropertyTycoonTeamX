using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    // list for transforms(positions) of all the spaces
    public List<Transform> spaceTransforms;
    void Start()
    {
        // loops through all the side spaces of the board (i = 0 is the center of the board)
        // and adds them to the list
        for(int i = 1; i < transform.childCount;i++)
        {
            spaceTransforms.Add(transform.GetChild(i).transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
