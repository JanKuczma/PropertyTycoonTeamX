using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceBehaviour : MonoBehaviour
{
    // movement speed
    float speed;
    // current position 0 - 39 (40 spaces)
    int currentPos;
    // bool to control the movement
    public bool isMoving;
    // Board's Route
    // to be assigned in the unity inspector
    public Route currentRoute;
    void Start()
    {
        // sets up initial values
        isMoving = false;
        currentPos = 0;
        speed = 4f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    //coroutine for movement
    public IEnumerator move(int steps)
    {
        // this "if" stops another dice roll while object is in move
        if(isMoving)
        {
            yield break;
        }
        isMoving = true;
        while(steps > 0)
        {
            // nextPos is just position of the next square
            Vector3 nextPos = currentRoute.spaceTransforms[(currentPos + 1) % 40].position;
            // the target height is the same as current piece height
            nextPos[1] = transform.position.y;
            if((currentPos + 1) % 10 == 0)
            {
                Vector3 targetRight = transform.right;
                // while piece is not on the target squaree and not finished rotating
                while(MoveToNextSpace(nextPos) && rotateRight(targetRight)){yield return null;}
            } else {
                while(MoveToNextSpace(nextPos)){yield return null;}
            }
            // this just wait small amout of time before moving to the next square
            yield return new WaitForSeconds(0.1f);
            // decrement steps and increment currentPos (0 - 39)
            steps--;
            currentPos = ((currentPos + 1) % 40);
        }
        isMoving = false;
    }

    // moves piece to target square (next Square) and returns false if already on the target
    bool MoveToNextSpace(Vector3 target)
    { 
        return target != (transform.position = Vector3.MoveTowards(transform.position,target,speed*Time.deltaTime));
    }

    bool rotateRight(Vector3 targetright)
    {
        return Quaternion.Euler(targetright) != 
            (transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward,targetright,speed*Time.deltaTime,0.0f)));
    }
}
