using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Square : MonoBehaviour
{
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
    public static Square Create(SqType type, Transform parent, int position, string name_first="", string price_secoond="",int group = ((int)Group.BROWN))
    {
        Square square = Instantiate(Asset.Board(type),generateCoordinates(position),getRotation(position),parent).GetComponent<Square>();
        square._position = position;
        square.setName(name_first);
        switch(type)
        {
            case SqType.PROPERTY:
            square.GetComponent<PropertySquare>().setPrice(price_secoond);
            square.GetComponent<PropertySquare>().setGroup(group);
            break;
            case SqType.STATION:
            case SqType.BULB:
            case SqType.WATER:
            square.GetComponent<UtilitySqaure>().setPrice(price_secoond);
            break;
            case SqType.SUPERTAX:
            case SqType.INCOMETAX:
            square.GetComponent<TaxSqaure>().setAmount(price_secoond);
            break;
            case SqType.POTLUCK:
            case SqType.CHANCE1:
            case SqType.CHANCE2:
            case SqType.CHANCE3:
            break;
            case SqType.GO:
            square.GetComponent<GoSquare>().setSecond(price_secoond);
            break;
            case SqType.PARKING:
            square.GetComponent<ParkingSquare>().setVisiting(price_secoond);
            break;
            case SqType.GOTOJAIL:
            square.GetComponent<GoToJailSquare>().setJailText(price_secoond);
            break;
            case SqType.JAILVISIT:
            square.GetComponent<JailSquare>().setVisiting(price_secoond);
            break;
            
        }
        return square;
    }
    virtual public void setName(string name)
    {
        _name = name;
        GetComponentsInChildren<TextMeshPro>()[0].SetText(name);
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
    /// generates square coordinates accordingly to the board center(this) postion/scale
    private static Vector3 generateCoordinates(int position)
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
}
