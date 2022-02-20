using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour
{
    PieceBehaviour[] pieces;
    void Start()
    {
        pieces = GetComponentsInChildren<PieceBehaviour>();
    }

    // Update is called once per frame
    public PieceBehaviour get(int index)
    {
        return pieces[index];
    }

    public int Count()
    {
        return pieces.Length;
    }

    public void addPiece(GameObject prefab)
    {

    }
}
