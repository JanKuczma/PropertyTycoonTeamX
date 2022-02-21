using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board : MonoBehaviour
{
    [System.NonSerialized] public SquareArrangement[] squares; // list for squares references
    [System.NonSerialized] public JailArrangement jail;    // parameter for jail square reference
    private Vector3[] squareCoordinates;

    void Awake()
    {
        squareCoordinates = new Vector3[40];
        generateSquareCoordinates();
        squares = new SquareArrangement[40];
    }
    public static Board Create(Transform parent)
    {
        return Instantiate(Asset.Board(),parent).GetComponent<Board>();
    }
    /// SqType is just enum, instantiates squre depending on the type
    /// commented stuff to be developed/implemented
    public void initSquare(SqType type,int position, string name="", string price="",string group="")
    {
        GameObject square = Instantiate(Asset.Board(type),transform);
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = squareCoordinates[position-1];
        square.transform.localRotation = getRotation(position);
        squares[position-1] = square.GetComponent<SquareArrangement>();
        squares[position-1].assignSpots(position);
        
        switch(type)
        {
            case SqType.PROPERTY:
            //square.GetComponent<CustomisableSquareProp>().setName(name);
            //square.GetComponent<CustomisableSquareProp>().setPrice(price);
            //square.GetComponent<CustomisableSquareProp>().setGroup(group);
            break;
            case SqType.STATION:
            case SqType.BULB:
            case SqType.WATER:
            //square.GetComponent<CustomisableSquare>().setName(name);
            //square.GetComponent<CustomisableSquare>().setPrice(price);
            break;
            case SqType.SUPERTAX:
            case SqType.INCOMETAX:
            //square.GetComponent<CustomisableTax>().setAmount(amount);
            break;
            case SqType.JAILVISIT:
            jail = square.GetComponent<JailArrangement>(); 
            squares[position-1] = jail;
            jail.assignSpots(position);
            jail.assignCells();
            break;
            
        }
    }
    /// generates square coordinates accordingly to the board center(this) postion/scale
    private void generateSquareCoordinates()
    {
        float displacement = .0064f;
        for(int i = 1; i < squareCoordinates.Length/4; i++)
        {
            squareCoordinates[i] = new Vector3(-displacement,0,.0085f);
            squareCoordinates[i+squareCoordinates.Length/4] = new Vector3(.0085f,0,displacement);
            squareCoordinates[i+squareCoordinates.Length/2] = new Vector3(displacement,0,-.0085f);
            squareCoordinates[i+(int)(squareCoordinates.Length*0.75f)] = new Vector3(-.0085f,0,-displacement);
            displacement -= .0016f;
        }
        squareCoordinates[0] = new Vector3(-.0085f,0,.0085f);
        squareCoordinates[10] = new Vector3(.0085f,0,.0085f);
        squareCoordinates[20] = new Vector3(.0085f,0,-.0085f);
        squareCoordinates[30] = new Vector3(-.0085f,0,-.0085f);
    }

    /// generates rotation depending on the which side the square is (fornt,left,top,right)
    private Quaternion getRotation(int position)
    {
        return  position > 30 ? Quaternion.Euler(0,-90,0) :
                position > 20 ? Quaternion.Euler(0,180,0) :
                position > 10 ? Quaternion.Euler(0,90,0) :
                                Quaternion.Euler(0,0,0);
    }
}
