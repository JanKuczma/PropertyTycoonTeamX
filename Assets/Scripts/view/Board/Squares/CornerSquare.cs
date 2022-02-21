using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CornerSquare : Square
{
    protected string _first = "";
    protected string _sceond = "";
    
    virtual public void assignSpots()
    {
        spots[0] = transform.position + Vector3.right*offsetB;
        spots[1] = transform.position + Vector3.right*offsetB + Vector3.forward*offsetB;
        spots[2] = transform.position + Vector3.right*offsetB - Vector3.forward*offsetB;
        spots[3] = transform.position - Vector3.right*offsetB;
        spots[4] = transform.position - Vector3.right*offsetB + Vector3.forward*offsetB;
        spots[5] = transform.position - Vector3.right*offsetB - Vector3.forward*offsetB;
    }
}
