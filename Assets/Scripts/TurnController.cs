using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    // list of references to piecesBehaviour
    public PieceBehaviour[] pieces;
    // current piece
    int current;
    void Start()
    {
        current = 0;
        pieces = GetComponentsInChildren<PieceBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        // temp code for setting up pieces on start
        if(Input.GetKeyDown("z"))
        {
            foreach(PieceBehaviour piece in pieces)
            {
                piece.moveInstant(0);
            }
        }
        // temp code for piecec switch
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            current = (current + 1) % pieces.Length;
            Debug.Log("Current: " + current);
        }
        // temporary code for dice roll simulation
        if(Input.GetKeyDown(KeyCode.Space) && !pieces[current].isMoving)
        {
            int st = Random.Range(1, 13);
            StartCoroutine(pieces[current].move(st));
            Debug.Log("rolled " + st);
        }
        // temporary code for go to jail
        if(Input.GetKeyDown("a") && !pieces[current].isMoving)
        {
            StartCoroutine(pieces[current].goToJail());
        }
        // temporary code for leave jail
        if(Input.GetKeyDown("s") && !pieces[current].isMoving)
        {
            StartCoroutine(pieces[current].leaveJail());
        }
    }
}
