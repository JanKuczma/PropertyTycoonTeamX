using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// enum for keeping track of the turnstate state
public enum TurnState {DICEROLL, PIECEMOVE, ACTION, END}

public class TurnController : MonoBehaviour
{
    public Texture2D pointing_hand; // cursor
    public DiceContainer dice;  // reference to dice container
    public Pieces pieces;   // game pieces, assigned in inspector
    Vector3 cam_pos_top;    // top cam position
    int current;    // current piece

    TurnState state;

    void Start()
    {       // setting up the camera position and pieces
        Cursor.SetCursor(pointing_hand,Vector2.zero,CursorMode.Auto);
        cam_pos_top = Camera.main.transform.position;

        state = TurnState.DICEROLL;
        current = 0;
    }

    void Update()
    {
        // temp code for speeding up piece movement
        if(state == TurnState.PIECEMOVE)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                pieces.get(current).speedUp();
            }
        }
        if(state == TurnState.DICEROLL)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(pieces.get(current).move(-3));
            }
        }
    }

    void FixedUpdate()
    {
        if(state == TurnState.DICEROLL) // turn begins
        {
            if(!dice.areRolling())  // if dice are not rolling anymore
            {
                int steps = dice.get_result();  // get the result
                if(steps < 0)                   // if result is negative (dice are stuck)
                {                               // reset the dice
                    dice.reset();
                } else {                        // else start moving piece and change the turn state
                    StartCoroutine(pieces.get(current).move(steps));
                    state = TurnState.PIECEMOVE;
                }
            }
        }
        else if(state == TurnState.PIECEMOVE)
        {
            if(!pieces.get(current).isMoving)   //if piece is not moving anymore
            {
                state = TurnState.ACTION;   // change turn state to action
            }
        }
        else if(state == TurnState.ACTION)  // ACTION state (buy property, pay rent etc...)
        {
            state = TurnState.END;
        }
        else if(state == TurnState.END)     // END state, when player finished his turn
        {
            dice.reset();                   // reset dice
            current = (current + 1) % pieces.Count();    // switch to next player
            state = TurnState.DICEROLL;     // change state to initial state
        }
    }

    //temp code for camera movement
    void LateUpdate()
    {
        // simply if the current piece is moving move camera towards it, else move camera towards top position
        if(pieces.get(current).isMoving)
        {
            Vector3 target = pieces.get(current).transform.position*1.5f;
            target[1] = 7.0f;
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position,target,8.0f*Time.deltaTime);
            Vector3 lookDirection = pieces.get(current).transform.position - Camera.main.transform.position;
            lookDirection.Normalize();
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.LookRotation(lookDirection), 3.0f * Time.deltaTime);
        } else {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position,cam_pos_top,10.0f*Time.deltaTime);
            Vector3 lookDirection = -1.0f*Camera.main.transform.position;
            lookDirection.Normalize();
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.LookRotation(lookDirection), 4.0f * Time.deltaTime);
        }
    }
}
