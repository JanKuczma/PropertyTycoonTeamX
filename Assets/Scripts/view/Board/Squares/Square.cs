using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace View {
    /// <summary>
    /// Extends <see cref="MonoBehaviour"/><br/>
    /// Base class for all the square scritpts.
    /// </summary>
public abstract class Square : MonoBehaviour
{
    /// <summary>Reference to <see cref="TextMeshPro"> name.</summary>
    public TextMeshPro square_name;
    /// <summary>Square's name.</summary>
    string _name;
    /// <summary>Array of <see cref="Vector3"/> that represent spots position to be taken by tokens.</summary>
    public Vector3[] spots; // each square is divided into 6 areas stored in spaces
    /// <summary>List of indices of free spots.</summary>
    public List<int> spotsIs;
    /// <summary>
    /// Square position relative to the GO square (1).<br/>
    /// Positions are counted clockwise around the board.
    /// </summary>
    protected int _position;
    // offsets used for spot arrangement
    protected const float offsetS = 0.38f;
    protected const float offsetB = 0.56f;

    protected virtual void Awake()
    {
        spots = new Vector3[6];
        spotsIs = new List<int> {0,1,2,3,4,5};
    }
    /// <summary>
    /// Sets <see cref="TextMeshPro"/> attached to the square to specified text.
    /// </summary>
    /// <param name="name">Square title.</param>
    virtual public void setName(string name)
    {
        _name = name;
        this.square_name.SetText(name);
    }
    /// <returns>Index of next free spot.</returns>
    public int peekSpotI()
    {
        if(spotsIs.Count > 0) return spotsIs[0];
        else return -1; 
    }

    /// <returns>Index of the next free spot. The index is removed from <paramref name="spotsIs"/> </returns>
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
    /// <summary>
    /// Removes index from <paramref name="spotsIs"/>. 
    /// </summary>
    /// <param name="spotI">Spot index</param>
    public void removeSpotI(int spotI)
    {
        spotsIs.Remove(spotI);
    }

    /// <summary>
    /// Adds <paramref name="spotI"/> to list of free spots' indicies.
    /// </summary>
    /// <param name="spotI">Spot index (0-5).</param>
    public void releaseSpotI(int spotI)
    {
        if((!spotsIs.Contains(spotI)) && spotI >= 0) spotsIs.Add(spotI);
        spotsIs.Sort();
    }
    /// <returns>Vector3 of the next free spot, (0,0,0) if no free spots</returns>
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
    /// <param name="spotI">Spot index (0-5).</param>
    /// <returns>Vector3 of the specified spot by index</returns>
    public Vector3 peekSpot(int spotI)
    {
        return spots[spotI];
    }
    /// <summary>
    /// Generates square coordinates accordingly to the board center postion.
    /// </summary>
    /// <param name="position">Square index (0-39).</param>
    /// <returns>Correct square position.</returns>
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

    /// <summary>
    /// Generates rotation <see cref="Quaternion"/> depending on the which side of the board center the square is (fornt,left,top,right).
    /// </summary>
    /// <param name="position">Square index (0-39).</param>
    /// <returns>Correct rotation.</returns>
    protected static Quaternion getRotation(int position)
    {
        return  position > 30 ? Quaternion.Euler(0,-90,0) :
                position > 20 ? Quaternion.Euler(0,180,0) :
                position > 10 ? Quaternion.Euler(0,90,0) :
                                Quaternion.Euler(0,0,0);
    }
    /// <summary>
    /// Protected method used to generate spots postitions for tokens (6 positions).
    /// </summary>
    protected abstract void assignSpots();
}
}