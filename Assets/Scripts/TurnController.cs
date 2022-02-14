using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    // list of references to piecesBehaviour
    public PieceBehaviour[] pieces;
    // current piece
    int current;
    Vector3 cam_pos_top;
    Quaternion cam_rot_top;
    void Start()
    {
        current = 0;
        pieces = GetComponentsInChildren<PieceBehaviour>();
        cam_pos_top = Camera.main.transform.position;
        cam_rot_top = Camera.main.transform.rotation;
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
        // temp code for touch devices
        if(Input.touchCount > 0)
        {
            int st = Random.Range(1, 13);
            StartCoroutine(pieces[current].move(st));
            Debug.Log("rolled " + st);
        }
    }
    //temp code for camera movement
    void LateUpdate()
    {
        if(pieces[current].isMoving)
        {
            Vector3 target = pieces[current].transform.position*1.5f;
            target[1] = 7.0f;
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position,target,8.0f*Time.deltaTime);
            Vector3 lookDirection = pieces[current].transform.position - Camera.main.transform.position;
            lookDirection.Normalize();
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.LookRotation(lookDirection), 3.0f * Time.deltaTime);
        } else {
            Camera.main.transform.position = Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position,cam_pos_top,10.0f*Time.deltaTime);
            Vector3 lookDirection = -1.0f*Camera.main.transform.position;
            lookDirection.Normalize();
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.LookRotation(lookDirection), 4.0f * Time.deltaTime);
        }
    }
}
