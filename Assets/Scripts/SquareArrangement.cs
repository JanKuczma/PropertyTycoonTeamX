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
        // the offsets adjust according to the square scale
        float offsetSmall = 0.6f*(transform.localScale.x/100);
        float offsetBig = 0.8f*(transform.localScale.x/100);
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
            case int n when (n > 1 && n < 11):
                horSideDown(offsetSmall,offsetBig);
                break;
            case int n when (n > 11 && n < 21):
                verSideLeft(offsetSmall,offsetBig);
                break;
            case int n when (n > 21 && n < 31):
                horSideUp(offsetSmall,offsetBig);
                break;
            case int n when (n > 31 && n < 40):
                verSideRight(offsetSmall,offsetBig);
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
        spaces[0] = transform.position + Vector3.right*(2*offsetSmall);
        spaces[1] = transform.position + Vector3.right*(2*offsetSmall) + Vector3.forward*(2*offsetSmall);
        spaces[2] = transform.position + Vector3.right*(2*offsetSmall) - Vector3.forward*(2*offsetSmall);
        spaces[3] = transform.position + Vector3.forward*(2*offsetSmall) + Vector3.right*(offsetSmall/2.0f);
        spaces[4] = transform.position + Vector3.forward*(2*offsetSmall) - Vector3.right*offsetSmall;
        spaces[5] = transform.position + Vector3.forward*(2*offsetSmall) - Vector3.right*(offsetBig+offsetSmall);
    }
    private void horSideDown(float offsetSmall, float offsetBig)
    {
        spaces[0] = transform.position + Vector3.right*offsetSmall + Vector3.forward*(offsetSmall/2);
        spaces[1] = transform.position + Vector3.right*offsetSmall + Vector3.forward*(offsetBig + offsetSmall/2);
        spaces[2] = transform.position + Vector3.right*offsetSmall - Vector3.forward*(offsetBig -offsetSmall/2);
        spaces[3] = transform.position - Vector3.right*offsetSmall + Vector3.forward*(offsetSmall/2);
        spaces[4] = transform.position - Vector3.right*offsetSmall + Vector3.forward*(offsetBig + offsetSmall/2);
        spaces[5] = transform.position - Vector3.right*offsetSmall - Vector3.forward*(offsetBig - offsetSmall/2);
    }
    private void horSideUp(float offsetSmall, float offsetBig)
    {
        spaces[0] = transform.position + Vector3.right*offsetSmall - Vector3.forward*(offsetSmall/2);
        spaces[1] = transform.position + Vector3.right*offsetSmall + Vector3.forward*(offsetBig - offsetSmall/2);
        spaces[2] = transform.position + Vector3.right*offsetSmall - Vector3.forward*(offsetBig + offsetSmall/2);
        spaces[3] = transform.position - Vector3.right*offsetSmall - Vector3.forward*(offsetSmall/2);
        spaces[4] = transform.position - Vector3.right*offsetSmall + Vector3.forward*(offsetBig - offsetSmall/2);
        spaces[5] = transform.position - Vector3.right*offsetSmall - Vector3.forward*(offsetBig + offsetSmall/2);
    }
    private void verSideLeft(float offsetSmall, float offsetBig)
    {
        spaces[0] = transform.position + Vector3.forward*offsetSmall + Vector3.right*(offsetSmall/2);
        spaces[1] = transform.position + Vector3.forward*offsetSmall + Vector3.right*(offsetBig + offsetSmall/2);
        spaces[2] = transform.position + Vector3.forward*offsetSmall - Vector3.right*(offsetBig - offsetSmall/2);
        spaces[3] = transform.position - Vector3.forward*offsetSmall + Vector3.right*(offsetSmall/2);
        spaces[4] = transform.position - Vector3.forward*offsetSmall + Vector3.right*(offsetBig + offsetSmall/2);
        spaces[5] = transform.position - Vector3.forward*offsetSmall - Vector3.right*(offsetBig - offsetSmall/2);
    }
    private void verSideRight(float offsetSmall, float offsetBig)
    {
        spaces[0] = transform.position + Vector3.forward*offsetSmall - Vector3.right*(offsetSmall/2);
        spaces[1] = transform.position + Vector3.forward*offsetSmall + Vector3.right*(offsetBig - offsetSmall/2);
        spaces[2] = transform.position + Vector3.forward*offsetSmall - Vector3.right*(offsetBig + offsetSmall/2);
        spaces[3] = transform.position - Vector3.forward*offsetSmall - Vector3.right*(offsetSmall/2);
        spaces[4] = transform.position - Vector3.forward*offsetSmall + Vector3.right*(offsetBig - offsetSmall/2);
        spaces[5] = transform.position - Vector3.forward*offsetSmall - Vector3.right*(offsetBig + offsetSmall/2);
    }
}
