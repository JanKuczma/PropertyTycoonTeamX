using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace View {
public abstract class Square : MonoBehaviour
{
    string _name;
    // each square is divided into 6 areas stored in spaces
    public Vector3[] spots;
    public List<int> spotsIs;
    // offsets used for spot arrangement
    protected const float offsetS = 0.6f;
    protected const float offsetB = 0.8f;
    protected virtual void Awake()
    {
        spots = new Vector3[6];
        spotsIs = new List<int> {0,1,2,3,4,5};
    }
    public static Square Create(SqType type, Transform parent, int position, string name="", string price_amount="",int group = ((int)Group.BROWN),string variant="")
    {
        Square square = Instantiate(Asset.Board(type,variant),parent).GetComponent<Square>();
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = generateCoordinates(position);
        square.transform.localRotation = getRotation(position);
        square.setName(name);
        square.assignSpots();
        switch(type)
        {
            case SqType.PROPERTY:
            square.GetComponent<PropertySquare>().setPrice(price_amount);
            square.GetComponent<PropertySquare>().setGroup(group);
            break;
            case SqType.STATION:
            case SqType.UTILITY:
            square.GetComponent<UtilitySqaure>().setPrice(price_amount);
            break;
            case SqType.TAX:
            square.GetComponent<TaxSqaure>().setAmount(price_amount);
            break;
            case SqType.GO:
            square.GetComponent<GoSquare>().setAmount(price_amount);
            break;
            case SqType.JAILVISIT:
            square.GetComponent<JailSquare>().assignCells();
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
    private static Vector3 generateCoordinates(int position)
    {
        float displacement = .0080f;
        displacement =  position < 11 ? 1*displacement :
                        position < 21 ? displacement :
                        position < 21 ? -1*displacement :
                        displacement;
        displacement = displacement - (.0016f*((position-1)%10));

        return  position == 1 ? new Vector3(-.0085f,0,.0085f) :
                position < 11 ? new Vector3(-displacement,0,.0085f) :
                position == 11 ? new Vector3(.0085f,0,.0085f) :
                position < 21 ? new Vector3(.0085f,0,displacement) :
                position == 21 ? new Vector3(.0085f,0,-.0085f) :
                position < 31 ? new Vector3(displacement,0,-.0085f) :
                position == 31 ? new Vector3(-.0085f,0,-.0085f) :
                new Vector3(-.0085f,0,-displacement);
    }

    /// generates rotation depending on the which side the square is (fornt,left,top,right)
    private static Quaternion getRotation(int position)
    {
        return  position > 30 ? Quaternion.Euler(0,-90,0) :
                position > 20 ? Quaternion.Euler(0,180,0) :
                position > 10 ? Quaternion.Euler(0,90,0) :
                                Quaternion.Euler(0,0,0);
    }
    protected abstract void assignSpots();
}
}