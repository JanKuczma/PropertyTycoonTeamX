using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareArrangement : MonoBehaviour
{
    // each square is divided into 6 areas stored in spaces
    public Vector3[] spaces;
    public List<int> freeSpaces;
    void Start()
    {
        // get the index of the square
        int index = transform.GetSiblingIndex();
        freeSpaces = new List<int> {0,1,2,3,4,5};
        spaces = new Vector3[6];
        float offsetSmall = 0.6f;
        float offsetBig = 0.8f;
        // depending on the square (corner, jail, horizontal side, vertical side)
        // assign positions of the areas
        switch(index){
            case 1:
            case 21: 
            case 31:
                corner(offsetSmall,offsetBig);
                break;
            case 11:
                visitJail(offsetSmall,offsetBig);
                break;
            case int n when ((n > 1 && n < 11) || (n > 21 && n < 31)):
                horSide(offsetSmall,offsetBig);
                break;
            case int n when ((n > 11 && n < 21) || (n > 31 && n < 41)):
                verSide(offsetSmall,offsetBig);
                break;
        }
    }
    // returns index of free area at random 
    public int nextFree()
    {
        return freeSpaces[Random.Range(0,freeSpaces.Count)]; 
    }

    private void corner(float offsetSmall, float offsetBig)
    {
        spaces[0] = transform.position + Vector3.right*offsetBig;
        spaces[1] = transform.position + Vector3.right*offsetBig + Vector3.forward*offsetBig;
        spaces[2] = transform.position + Vector3.right*offsetBig - Vector3.forward*offsetBig;
        spaces[3] = transform.position - Vector3.right*offsetBig;
        spaces[4] = transform.position - Vector3.right*offsetBig + Vector3.forward*offsetBig;
        spaces[5] = transform.position - Vector3.right*offsetBig - Vector3.forward*offsetBig;
    }
    private void visitJail(float offsetSmall, float offsetBig)
    {
        spaces[0] = transform.position + Vector3.right*offsetBig;
        spaces[1] = transform.position + Vector3.right*offsetBig + Vector3.forward*offsetBig;
        spaces[2] = transform.position + Vector3.right*offsetBig - Vector3.forward*offsetBig;
        spaces[3] = transform.position + Vector3.forward*offsetBig;
        spaces[4] = transform.position + Vector3.forward*offsetBig - Vector3.right*offsetBig;
        spaces[5] = transform.position;
    }
    private void horSide(float offsetSmall, float offsetBig)
    {
        spaces[0] = transform.position + Vector3.right*offsetSmall;
        spaces[1] = transform.position + Vector3.right*offsetSmall + Vector3.forward*offsetBig;
        spaces[2] = transform.position + Vector3.right*offsetSmall - Vector3.forward*offsetBig;
        spaces[3] = transform.position - Vector3.right*offsetSmall;
        spaces[4] = transform.position - Vector3.right*offsetSmall + Vector3.forward*offsetBig;
        spaces[5] = transform.position - Vector3.right*offsetSmall - Vector3.forward*offsetBig;
    }
    private void verSide(float offsetSmall, float offsetBig)
    {
        spaces[0] = transform.position + Vector3.forward*offsetSmall;
        spaces[1] = transform.position + Vector3.forward*offsetSmall + Vector3.right*offsetBig;
        spaces[2] = transform.position + Vector3.forward*offsetSmall - Vector3.right*offsetBig;
        spaces[3] = transform.position - Vector3.forward*offsetSmall;
        spaces[4] = transform.position - Vector3.forward*offsetSmall + Vector3.right*offsetBig;
        spaces[5] = transform.position - Vector3.forward*offsetSmall - Vector3.right*offsetBig;
    }
}
