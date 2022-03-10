using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View {
public class Board : MonoBehaviour
{
    [System.NonSerialized] public Square[] squares; // list for squares references
    [System.NonSerialized] public JailSquare jail;    // parameter for jail square reference

    void Awake()
    {
        squares = new Square[40];
    }
    public static Board Create(Transform parent,Model.Board boardData = null)
    {
        if(boardData == null)
        {
            return Instantiate(Asset.Board(),parent).GetComponent<Board>();
        }
        // get the data from Model.Board and init all the squares
        return Instantiate(Asset.Board(),parent).GetComponent<Board>();
    }
    /// SqType is just enum, instantiates squre depending on the type
    public void initSquare(SqType type,int position, string name="", string price="",int group=((int)Group.BROWN))
    {
        Square square = Square.Create(type,transform,position,name,price,group);
        squares[position-1] = square;
        if (type == SqType.JAILVISIT) jail = square.GetComponent<JailSquare>();
    }
}
}