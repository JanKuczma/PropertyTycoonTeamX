using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace View{
public class JailSquare : CornerSquare
{
    /// <summary>
    /// <inheritdoc cref="Square.spotsIs" path="/summary"/>
    /// </summary>
    List<int> cellIs;
    /// <summary>
    /// <inheritdoc cref="Square.spots" path="/summary"/>
    /// </summary>
    Vector3[] cells;

    new void Awake()
    {
        base.Awake();
        cellIs = new List<int> {0,1,2,3,4,5};
        cells = new Vector3[6];
        assignCells();
        assignSpots();
    }
    public static JailSquare Create(Transform parent, int position, string name)
    {
        JailSquare square = Instantiate(Asset.Board(SqType.JAILVISIT),parent).GetComponent<JailSquare>();
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = Square.generateCoordinates(position);
        square.transform.localRotation = getRotation(position);
        square._position = position;
        square.setName(name);
        square.assignSpots();
        square.assignCells();
        return square;
    }
    /// <param name="cellI">Spot index (0-5)./></param>
    /// <returns><inheritdoc cref="Square.peekSpot" path="/returns"/></returns>
    public Vector3 peekCell(int cellI)
    {
        return cells[cellI];
    }

/// <summary>
/// Works just like <see cref="Square.releaseSpotI(int)"/>.
/// </summary>
/// <param name="cellI"></param>
    public void releaseCellI(int cellI)
    {
        if(!cellIs.Contains(cellI) && cellI >= 0) cellIs.Add(cellI);
        cellIs.Sort();
    }
    /// <summary>
    /// Returns and Removes next free cell spot index.
    /// </summary>
    public int popCellI()
    {
        int cellIndex;
        if(cellIs.Count > 0)
        {
            cellIndex = cellIs[0];
            cellIs.Remove(cellIndex);
            return cellIndex;
        } else {
            return -1;
        }
    }
    private void assignCells()
    {
        float offsetSmall = offsetS*(transform.localScale.x);
        float offsetBig = offsetB*(transform.localScale.x);

        cells[0] = transform.position - transform.forward*offsetSmall;
        cells[3] = transform.position + transform.forward*(offsetSmall/2.0f);
        cells[2] = transform.position + transform.forward*(offsetSmall/2.0f)        + transform.right*(offsetBig+offsetSmall);
        cells[4] = transform.position - transform.forward*offsetSmall               + transform.right*(offsetBig+offsetSmall);
        cells[1] = transform.position - transform.forward*(offsetBig+offsetSmall)   + transform.right*(offsetBig+offsetSmall);
        cells[5] = transform.position - transform.forward*(offsetBig+offsetSmall);
    }
    override protected void assignSpots()
    {
        float offsetSmall = offsetS*(transform.localScale.x);
        float offsetBig = offsetB*(transform.localScale.x);
        spots[0] = transform.position + transform.forward*(2*offsetSmall)           + transform.right*(2*offsetSmall);
        spots[1] = transform.position + transform.forward*(2*offsetSmall)           - transform.right*(2*offsetSmall);
        spots[2] = transform.position + transform.forward*(2*offsetSmall);
        spots[3] = transform.position + transform.forward*(offsetSmall/2.0f)        - transform.right*(2*offsetSmall);
        spots[4] = transform.position - transform.forward*offsetSmall               - transform.right*(2*offsetSmall);
        spots[5] = transform.position - transform.forward*(offsetBig+offsetSmall)   - transform.right*(2*offsetSmall);
    }

    public override void setName(string name)
    {
        GetComponentsInChildren<TextMeshPro>()[0].SetText("JUST");
        GetComponentsInChildren<TextMeshPro>()[1].SetText("VISITING");
    }
}
}