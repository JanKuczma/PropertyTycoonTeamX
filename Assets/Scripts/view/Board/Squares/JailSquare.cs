using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class JailSquare : CornerSquare
{
    List<int> cellIs;
    Vector3[] cells;

    new void Awake()
    {
        base.Awake();
        cellIs = new List<int> {0,1,2,3,4,5};
        cells = new Vector3[6];
    }

    public Vector3 peekCell(int cellI)
    {
        return cells[cellI];
    }

    public void releaseCellI(int cellI)
    {
        if(!cellIs.Contains(cellI) && cellI >= 0) cellIs.Add(cellI);
    }

    public int popCellI()
    {
        int cellIndex;
        if(cellIs.Count > 0)
        {
            cellIndex = cellIs[Random.Range(0,cellIs.Count)];
            cellIs.Remove(cellIndex);
            return cellIndex;
        } else {
            return -1;
        }
    }
    public void assignCells()
    {
        float offsetSmall = offsetS*(transform.localScale.x);
        float offsetBig = offsetB*(transform.localScale.x);

        cells[0] = transform.position + transform.forward*(offsetSmall/2.0f)        + transform.right*(offsetBig+offsetSmall);
        cells[1] = transform.position - transform.forward*offsetSmall               + transform.right*(offsetBig+offsetSmall);
        cells[2] = transform.position - transform.forward*(offsetBig+offsetSmall)   + transform.right*(offsetBig+offsetSmall);
        cells[3] = transform.position + transform.forward*(offsetSmall/2.0f);
        cells[4] = transform.position - transform.forward*offsetSmall;
        cells[5] = transform.position - transform.forward*(offsetBig+offsetSmall);
    }
    override public void assignSpots()
    {
        float offsetSmall = offsetS*(transform.localScale.x);
        float offsetBig = offsetB*(transform.localScale.x);
        spots[0] = transform.position + transform.right*(2*offsetSmall);
        spots[1] = transform.position + transform.right*(2*offsetSmall)     + transform.forward*(2*offsetSmall);
        spots[2] = transform.position + transform.right*(2*offsetSmall)     - transform.forward*(2*offsetSmall);
        spots[3] = transform.position + transform.forward*(2*offsetSmall)   + transform.right*(offsetSmall/2.0f);
        spots[4] = transform.position + transform.forward*(2*offsetSmall)   - transform.right*offsetSmall;
        spots[5] = transform.position + transform.forward*(2*offsetSmall)   - transform.right*(offsetBig+offsetSmall);
    }

    public override void setName(string just="")
    {
        if(just.Equals("")) just = "JUST";
        _first = just;
        GetComponentsInChildren<TextMeshPro>()[0].SetText(just);
    }

    public void setVisiting(string visiting="")
    {
        if(visiting.Equals("")) visiting = "VISITNG";
        _sceond = visiting;
        GetComponentsInChildren<TextMeshPro>()[1].SetText(visiting);
    }
}
