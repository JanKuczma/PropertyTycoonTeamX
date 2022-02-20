using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // list for squares references
    public SquareArrangement[] squares;
    public JailArrangement jail;    // parameter for jail square reference
    void Start()
    {
        // referneces to all squares on the board
        squares = GetComponentsInChildren<SquareArrangement>();
        jail = GetComponentInChildren<JailArrangement>();
    }
}
