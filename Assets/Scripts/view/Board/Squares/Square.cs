using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace View {
public abstract class Square : MonoBehaviour
{
    public TextMeshPro square_name;
    string _name;
    // each square is divided into 6 areas stored in spaces
    public Vector3[] spots;
    public List<int> spotsIs;
    protected int _position;
    // offsets used for spot arrangement
    protected const float offsetS = 0.38f;
    protected const float offsetB = 0.56f;

    protected virtual void Awake()
    {
        spots = new Vector3[6];
        spotsIs = new List<int> {0,1,2,3,4,5};
    }
    virtual public void setName(string name)
    {
        _name = name;
        this.square_name.SetText(name);
    }
    // returns index of free area at random 
    public int peekSpotI()
    {
        if(spotsIs.Count > 0) return spotsIs[0];
        else return -1; 
    }

    // returns index of free area at random and removes it from free spot list
    public int popSpotI()
    {
        int spotIndex;
        if(spotsIs.Count > 0)
        {
            spotIndex = spotsIs[0];
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
        spotsIs.Sort();
    }
    // returns Vector3 of the next free spot, (0,0,0) if no free spots
    public Vector3 peekSpot()
    {
        if(spotsIs.Count > 0)
        {
            int spotIndex = spotsIs[0];
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
    /// generates square coordinates accordingly to the board center(this) postion/scale
    protected static Vector3 generateCoordinates(int position)
    {
        float displacement = 8.0f;
        displacement =  position < 11 ? 1*displacement :
                        position < 21 ? displacement :
                        position < 21 ? -1*displacement :
                        displacement;
        displacement = displacement - (1.6f*((position-1)%10));

        return  position == 1 ? new Vector3(-8.5f,0,8.5f) :
                position < 11 ? new Vector3(-displacement,0,8.5f) :
                position == 11 ? new Vector3(8.5f,0,8.5f) :
                position < 21 ? new Vector3(8.5f,0,displacement) :
                position == 21 ? new Vector3(8.5f,0,-8.5f) :
                position < 31 ? new Vector3(displacement,0,-8.5f) :
                position == 31 ? new Vector3(-8.5f,0,-8.5f) :
                new Vector3(-8.5f,0,-displacement);
    }

    /// generates rotation depending on the which side the square is (fornt,left,top,right)
    protected static Quaternion getRotation(int position)
    {
        return  position > 30 ? Quaternion.Euler(0,-90,0) :
                position > 20 ? Quaternion.Euler(0,180,0) :
                position > 10 ? Quaternion.Euler(0,90,0) :
                                Quaternion.Euler(0,0,0);
    }
    protected abstract void assignSpots();
}
}