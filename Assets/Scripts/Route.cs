using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    // list for transforms(positions) of all the spaces
    public List<SquareArrangement> squares;
    public JailArrangement jail;
    void Start()
    {
        // referneces to all squares on the board
        squares = GetComponentsInChildren<SquareArrangement>();
        foreach(SquareArrangement square in GetComponentsInChildren<SquareArrangement>())
        {
            if(square)squares.Add(square);
        }
        jail = GetComponentInChildren<JailArrangement>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
