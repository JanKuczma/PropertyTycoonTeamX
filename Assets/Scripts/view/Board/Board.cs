using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board : MonoBehaviour
{
    [System.NonSerialized] public Square[] squares; // list for squares references
    [System.NonSerialized] public JailSquare jail;    // parameter for jail square reference
    private Vector3[] squareCoordinates;

    void Awake()
    {
        squareCoordinates = new Vector3[40];
        squares = new Square[40];
    }
    public static Board Create(Transform parent)
    {
        return Instantiate(Asset.Board(),parent).GetComponent<Board>();
    }
    /// SqType is just enum, instantiates squre depending on the type
    /// commented stuff to be developed/implemented
    public void initSquare(SqType type,int position, string name="", string price="",Group group=Group.BROWN)
    {
        Square square = Square.Create(type,transform,position,name,price,group);
        squares[position-1] = square;
        if (type == SqType.JAILVISIT) jail = square.GetComponent<JailSquare>();
    }
}
