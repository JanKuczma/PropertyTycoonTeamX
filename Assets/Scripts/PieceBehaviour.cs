using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceBehaviour : MonoBehaviour
{
    // movement speed
    float speed;
    // rotation speed
    float rotationSpeed;
    // current position 0 - 39 (40 squares)
    int currentSquare;
    // current spot 0 - 5 (6 areas)
    public int currentSpot;
    // bool to control the movement
    public bool isMoving;
    // Board's Route
    // to be assigned in the unity inspector
    public Route route;
    void Start()
    {
        // sets up initial values
        isMoving = false;
        speed = 6f;
        rotationSpeed = 4f;
        currentSpot = -1;
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
        // free the current area
        route.squares[currentSquare].releaseSpotI(currentSpot);
        while(steps > 0)
        {
            int nextSquare = (currentSquare + 1) % 40;
            // target is just position of the next square (free spot)
            currentSpot = route.squares[nextSquare].peekSpotI();
            Vector3 target = route.squares[nextSquare].peekSpot(currentSpot);
            // the target height is the same as current piece height
            target[1] = transform.position.y;
            // this bit basically calculates trajectory using Bezier Curve (20 points plus the target position)
            Vector3 control = (transform.position+target)/2 + new Vector3(0.0f,3.0f, 0);
            List<Vector3> path = BezierCurve(transform.position,control,target);
            // this bit depending on transition (side or corner square)
            // moves piece along the curve
            int counter = 0;
            if(nextSquare % 10 == 0)
            {
                Vector3 targetRight = transform.right;
                // while piece is not on the target square and not finished rotating
                while(counter < path.Count && rotate(targetRight))
                {
                    if(moveTo(path[counter]))
                    {
                        counter++;
                    }
                    yield return null;
                }
            } else {
                // while piece is not on the target square
                while(counter < path.Count)
                {
                    if(moveTo(path[counter]))
                    {
                        counter++;
                    }
                    yield return null;
                }
            }
            // this just wait small amout of time before moving to the next square
            yield return new WaitForSeconds(0.1f);
            // decrement steps and increment currentSquare (0 - 39)
            steps--;
            currentSquare = nextSquare;
        }
        // remove the current spot from freeSpots
        route.squares[currentSquare].removeSpotI(currentSpot);
        isMoving = false;
    }

    // moves instantaneously to specified square
    public void moveInstant(int square)
    {
        route.squares[currentSquare].releaseSpotI(currentSpot);
        // target is just position of the next square (free spot)
        currentSpot = route.squares[square].popSpotI();
        Vector3 target = route.squares[square].peekSpot(currentSpot);
        // the target height is the same as current piece height
        target[1] = transform.position.y;
        transform.position = target;
        switch(square)
        {
            case int n when (n > 29):
                transform.rotation = Quaternion.LookRotation(Vector3.left);
            break;
            case int n when (n > 19):

                transform.rotation = Quaternion.LookRotation(Vector3.back);
            break;
            case int n when (n > 9):
                transform.rotation = Quaternion.LookRotation(Vector3.right);
            break;
            default:
                transform.rotation = Quaternion.LookRotation(Vector3.forward);
            break;
        }
    }

    public IEnumerator goToJail()
    {
        // this "if" stops another dice roll while object is in move
        if(isMoving)
        {
            yield break;
        }
        isMoving = true;
        // free the current area
        route.squares[currentSquare].releaseSpotI(currentSpot);
        currentSquare = 10; // jail square
        currentSpot = route.jail.popCellI();
        Vector3 target = route.jail.peekCell(currentSpot);
        // the target height is the same as current piece height
        target[1] = transform.position.y;
        // this bit basically calculates trajectory using Bezier Curve (20 points plus the target position)
        Vector3 control = target + new Vector3(0.0f,3.0f, 0);
        List<Vector3> path = BezierCurve(transform.position,control,target);
        // moves piece along the curve
        int counter = 0;
        // while piece is not on the target square and not finished rotating
        while(counter < path.Count && rotate(Vector3.right))
        {
            if(moveTo(path[counter]))
            {
                counter++;
            }
            yield return null;
        }
        // this just wait small amout of time before moving to the next square
        yield return new WaitForSeconds(0.1f);
        isMoving = false;
    }
    public IEnumerator leaveJail()
    {
        // this "if" stops another dice roll while object is in move
        if(isMoving)
        {
            yield break;
        }
        isMoving = true;
        // free the current area
        route.jail.releaseCellI(currentSpot);
        currentSpot = route.squares[currentSquare].popSpotI();
        Vector3 target = route.squares[currentSquare].peekSpot(currentSpot);
        // the target height is the same as current piece height
        target[1] = transform.position.y;
        // this bit basically calculates trajectory using Bezier Curve (20 points plus the target position)
        Vector3 control = (transform.position+target)/2 + new Vector3(0.0f,3.0f, 0);
        List<Vector3> path = BezierCurve(transform.position,control,target);
        // moves piece along the curve
        int counter = 0;
        // while piece is not on the target square and not finished rotating
        while(counter < path.Count && rotate(Vector3.right))
        {
            if(moveTo(path[counter]))
            {
                counter++;
            }
            yield return null;
        }
        // this just wait small amout of time before moving to the next square
        yield return new WaitForSeconds(0.1f);
        isMoving = false;
    }

    // moves piece towards next position and returns true if already on the target
    private bool moveTo(Vector3 targetPos)
    { 
        return targetPos == (transform.position = Vector3.MoveTowards(transform.position,targetPos,speed*Time.deltaTime));
    }
    // rotates piece towards specified direction, returns false if on the target
    private bool rotate(Vector3 targetright)
    {
        return Quaternion.Euler(targetright) != 
            (transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward,targetright,rotationSpeed*Time.deltaTime,0.0f)));
    }
    // calculates bezier curve point
    private Vector3 BezierCurvePoint(Vector3 start, Vector3 control, Vector3 end, float t)
    {
    return (1-t)*(1-t)*start + 2*(1-t)*t*control + t*t*end;
    }
    // calculates list of points of bezier curve
    private List<Vector3> BezierCurve(Vector3 start, Vector3 control, Vector3 target)
    {
        List<Vector3> mid_positions = new List<Vector3>(31);
        for (int i = 0; i < 30; i++)
        {
            Vector3 newPosition = BezierCurvePoint(start, control, target, (float)i / 30);
            mid_positions.Add(newPosition);
        }
        mid_positions.Add(target);
        return mid_positions;
        
    }
}
