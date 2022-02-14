using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    // list for transforms(positions) of all the spaces
    public SquareArrangement[] squares;
    public JailArrangement jail;
    void Start()
    {
        // referneces to all squares on the board
        squares = GetComponentsInChildren<SquareArrangement>();
        jail = GetComponentInChildren<JailArrangement>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
