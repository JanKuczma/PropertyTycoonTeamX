using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    // list for transforms(positions) of all the spaces
    public SquareArrangement[] squares;
    void Start()
    {
        // loops through all the side spaces of the board (i = 0 is the center of the board)
        // and adds them to the list
            squares = GetComponentsInChildren<SquareArrangement>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
