using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    // Start is called before the first frame update
    public PieceBehaviour[] pieceMovements;
    int current;
    void Start()
    {
        current = 0;
        pieceMovements = GetComponentsInChildren<PieceBehaviour>();
        foreach(PieceBehaviour behaviour in pieceMovements)
        {
            behaviour.enabled = false;
        }
        pieceMovements[0].enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // temp code for piecec switch
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            pieceMovements[current].enabled = false;
            current = (current + 1) % pieceMovements.Length;
            pieceMovements[current].enabled = true;
        }
        // temporary code for dice roll simulation
        if(Input.GetKeyDown(KeyCode.Space) && !pieceMovements[current].isMoving)
        {
            int st = Random.Range(1, 13);
            StartCoroutine(pieceMovements[current].move(st));
            Debug.Log("rolled " + st);
        }
    }
}
