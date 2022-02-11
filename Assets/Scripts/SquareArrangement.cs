using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareArrangement : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3[] spaces;
    float offsetX;
    float offsetY;
    void Start()
    {
        spaces = new Vector3[6];
        for(int i = 0; i < 6; i++)
        {
            spaces[i] = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
