using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailArrangement : SquareArrangement
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
        float offsetSmall = 0.6f*(transform.localScale.x);
        float offsetBig = 0.8f*(transform.localScale.x);

        cells[0] = transform.position - Vector3.forward*(offsetBig+offsetSmall) + Vector3.right*(offsetSmall/2.0f);
        cells[1] = transform.position - Vector3.forward*(offsetBig+offsetSmall) - Vector3.right*offsetSmall;
        cells[2] = transform.position - Vector3.right*(offsetBig+offsetSmall) - Vector3.forward*(offsetBig+offsetSmall);
        cells[3] = transform.position + Vector3.right*(offsetSmall/2.0f);
        cells[4] = transform.position - Vector3.right*offsetSmall;
        cells[5] = transform.position - Vector3.right*(offsetBig+offsetSmall);
    }
}