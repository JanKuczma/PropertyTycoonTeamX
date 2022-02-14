using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareArrangement : MonoBehaviour
{
    // each square is divided into 6 areas stored in spaces
    public Vector3[] spots;
    public List<int> spotsIs;
    protected virtual void Start()
    {
        // get the index of the square
        int index = transform.GetSiblingIndex();
        spotsIs = new List<int> {0,1,2,3,4,5};
        spots = new Vector3[6];
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
    public int peekSpotI()
    {
        if(spotsIs.Count > 0) return spotsIs[Random.Range(0,spotsIs.Count)];
        else return -1; 
    }

    // returns index of free area at random and removes it from free spot list
    public int popSpotI()
    {
        int spotIndex;
        if(spotsIs.Count > 0)
        {
            spotIndex = spotsIs[Random.Range(0,spotsIs.Count)];
            spotsIs.Remove(spotIndex);
            return spotIndex;
        } else {
            return -1;
        }
    }
    public void removeSpotI(int spotI)
    {
        spotsIs.Remove(spotI);
    }

    // adds spotI to list of free spots
    public void releaseSpotI(int spotI)
    {
        if(!spotsIs.Contains(spotI) && spotI >= 0) spotsIs.Add(spotI);
    }
    // returns Vector3 of the next free spot, (0,0,0) if no free spots
    public Vector3 peekSpot()
    {
        if(spotsIs.Count > 0)
        {
            int spotIndex = spotsIs[Random.Range(0,spotsIs.Count)];
            return spots[spotIndex];
        } else {
            return Vector3.zero;
        }
    }
    // returns Vector3 of the specified spot
    public Vector3 peekSpot(int spotI)
    {
        return spots[spotI];
    }
    private void corner(float offsetSmall, float offsetBig)
    {
        spots[0] = transform.position + Vector3.right*offsetBig;
        spots[1] = transform.position + Vector3.right*offsetBig + Vector3.forward*offsetBig;
        spots[2] = transform.position + Vector3.right*offsetBig - Vector3.forward*offsetBig;
        spots[3] = transform.position - Vector3.right*offsetBig;
        spots[4] = transform.position - Vector3.right*offsetBig + Vector3.forward*offsetBig;
        spots[5] = transform.position - Vector3.right*offsetBig - Vector3.forward*offsetBig;
    }
    private void visitJail(float offsetSmall, float offsetBig)
    {
        spots[0] = transform.position + Vector3.right*(2*offsetSmall);
        spots[1] = transform.position + Vector3.right*(2*offsetSmall) + Vector3.forward*(2*offsetSmall);
        spots[2] = transform.position + Vector3.right*(2*offsetSmall) - Vector3.forward*(2*offsetSmall);
        spots[3] = transform.position + Vector3.forward*(2*offsetSmall) + Vector3.right*(offsetSmall/2.0f);
        spots[4] = transform.position + Vector3.forward*(2*offsetSmall) - Vector3.right*offsetSmall;
        spots[5] = transform.position + Vector3.forward*(2*offsetSmall) - Vector3.right*(offsetBig+offsetSmall);
    }
    private void horSideDown(float offsetSmall, float offsetBig)
    {
        spots[0] = transform.position + Vector3.right*offsetSmall + Vector3.forward*(offsetSmall/2);
        spots[1] = transform.position + Vector3.right*offsetSmall + Vector3.forward*(offsetBig + offsetSmall/2);
        spots[2] = transform.position + Vector3.right*offsetSmall - Vector3.forward*(offsetBig -offsetSmall/2);
        spots[3] = transform.position - Vector3.right*offsetSmall + Vector3.forward*(offsetSmall/2);
        spots[4] = transform.position - Vector3.right*offsetSmall + Vector3.forward*(offsetBig + offsetSmall/2);
        spots[5] = transform.position - Vector3.right*offsetSmall - Vector3.forward*(offsetBig - offsetSmall/2);
    }
    private void horSideUp(float offsetSmall, float offsetBig)
    {
        spots[0] = transform.position + Vector3.right*offsetSmall - Vector3.forward*(offsetSmall/2);
        spots[1] = transform.position + Vector3.right*offsetSmall + Vector3.forward*(offsetBig - offsetSmall/2);
        spots[2] = transform.position + Vector3.right*offsetSmall - Vector3.forward*(offsetBig + offsetSmall/2);
        spots[3] = transform.position - Vector3.right*offsetSmall - Vector3.forward*(offsetSmall/2);
        spots[4] = transform.position - Vector3.right*offsetSmall + Vector3.forward*(offsetBig - offsetSmall/2);
        spots[5] = transform.position - Vector3.right*offsetSmall - Vector3.forward*(offsetBig + offsetSmall/2);
    }
    private void verSideLeft(float offsetSmall, float offsetBig)
    {
        spots[0] = transform.position + Vector3.forward*offsetSmall + Vector3.right*(offsetSmall/2);
        spots[1] = transform.position + Vector3.forward*offsetSmall + Vector3.right*(offsetBig + offsetSmall/2);
        spots[2] = transform.position + Vector3.forward*offsetSmall - Vector3.right*(offsetBig - offsetSmall/2);
        spots[3] = transform.position - Vector3.forward*offsetSmall + Vector3.right*(offsetSmall/2);
        spots[4] = transform.position - Vector3.forward*offsetSmall + Vector3.right*(offsetBig + offsetSmall/2);
        spots[5] = transform.position - Vector3.forward*offsetSmall - Vector3.right*(offsetBig - offsetSmall/2);
    }
    private void verSideRight(float offsetSmall, float offsetBig)
    {
        spots[0] = transform.position + Vector3.forward*offsetSmall - Vector3.right*(offsetSmall/2);
        spots[1] = transform.position + Vector3.forward*offsetSmall + Vector3.right*(offsetBig - offsetSmall/2);
        spots[2] = transform.position + Vector3.forward*offsetSmall - Vector3.right*(offsetBig + offsetSmall/2);
        spots[3] = transform.position - Vector3.forward*offsetSmall - Vector3.right*(offsetSmall/2);
        spots[4] = transform.position - Vector3.forward*offsetSmall + Vector3.right*(offsetBig - offsetSmall/2);
        spots[5] = transform.position - Vector3.forward*offsetSmall - Vector3.right*(offsetBig + offsetSmall/2);
    }
}
