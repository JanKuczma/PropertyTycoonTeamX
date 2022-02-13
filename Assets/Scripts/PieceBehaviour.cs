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
    // current area 0 - 5 (6 areas)
    public int currentArea;
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
        route.squares[currentSquare].freeSpaces.Add(currentArea);
        while(steps > 0)
        {
            // target is just position of the next square (free spot)
            currentArea = route.squares[(currentSquare + 1) % 40].nextFree();
            Vector3 target = route.squares[(currentSquare + 1) % 40].spaces[currentArea];
            // the target height is the same as current piece height
            target[1] = transform.position.y;
            // this bit basically calculates trajectory using Bezier Curve (20 points plus the target position)
            Vector3 _initialPosition = transform.position;
            List<Vector3> _allPositions = new List<Vector3>(21);
            Vector3 control = (_initialPosition+target)/2 + new Vector3(0.0f,3.0f, 0);
            for (int i = 0; i < 20; i++)
            {
                Vector3 newPosition = BezierCurve(_initialPosition, control,
                target, (float)i / 20);
                _allPositions.Add(newPosition);
            }
            _allPositions.Add(target);
            // this bit depending on transition (side or corner square)
            // moves piece along the curve
            int counter = 0;
            if((currentSquare + 1) % 10 == 0)
            {
                Vector3 targetRight = transform.right;
                // while piece is not on the target square and not finished rotating
                while(counter < _allPositions.Count && rotateRight(targetRight))
                {
                    if(MoveTo(_allPositions[counter]))
                    {
                        counter++;
                    }
                    yield return null;
                }
            } else {
                // while piece is not on the target square
                while(counter < _allPositions.Count)
                {
                    if(MoveTo(_allPositions[counter]))
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
            currentSquare = ((currentSquare + 1) % 40);
        }
        // remove the current area from freeSpaces
        route.squares[currentSquare].freeSpaces.Remove(currentArea);
        isMoving = false;
    }

    // moves piece to next position and returns false if already on the target
    bool MoveTo(Vector3 targetPos)
    { 
        return targetPos == (transform.position = Vector3.MoveTowards(transform.position,targetPos,speed*Time.deltaTime));
    }
    // rotates piece to the right
    bool rotateRight(Vector3 targetright)
    {
        return Quaternion.Euler(targetright) != 
            (transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward,targetright,rotationSpeed*Time.deltaTime,0.0f)));
    }
    // calculates bezier curve point
    private Vector3 BezierCurve(Vector3 start, Vector3 control, Vector3 end, float t)
    {
    return (1-t)*(1-t)*start + 2*(1-t)*t*control + t*t*end;
    }
    // moves instantaneously to specified square
    public void moveInstant(int square)
    {
        //route.squares[currentSquare].freeSpaces.Add(currentArea);
        currentSquare = square;
        currentArea = route.squares[square].nextFree();
        route.squares[square].freeSpaces.Remove(currentArea);
        Vector3 newPos = route.squares[square].spaces[currentArea];
        newPos[1] = transform.position.y;
        transform.position = newPos;
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
        route.squares[currentSquare].freeSpaces.Add(currentArea);
        currentSquare = 10;
        currentArea = route.jail.nextFree();
        Vector3 target = route.jail.spaces[currentArea];
        // the target height is the same as current piece height
        target[1] = transform.position.y;
        // this bit basically calculates trajectory using Bezier Curve (20 points plus the target position)
        Vector3 _initialPosition = transform.position;
        List<Vector3> _allPositions = new List<Vector3>(21);
        Vector3 control = target + new Vector3(0.0f,3.0f, 0);
        for (int i = 0; i < 20; i++)
        {
            Vector3 newPosition = BezierCurve(_initialPosition, control,
            target, (float)i / 20);
            _allPositions.Add(newPosition);
        }
        _allPositions.Add(target);
        // this bit depending on transition (side or corner square)
        // moves piece along the curve
        int counter = 0;
        // while piece is not on the target square and not finished rotating
        while(counter < _allPositions.Count && rotateRight(Vector3.right))
        {
            if(MoveTo(_allPositions[counter]))
            {
                counter++;
            }
            yield return null;
        }
        // this just wait small amout of time before moving to the next square
        yield return new WaitForSeconds(0.1f);
        // remove the current area from freeSpaces
        route.jail.freeSpaces.Remove(currentArea);
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
        route.jail.freeSpaces.Add(currentArea);
        currentArea = route.squares[currentSquare].nextFree();
        Vector3 target = route.squares[currentSquare].spaces[currentArea];
        // the target height is the same as current piece height
        target[1] = transform.position.y;
        // this bit basically calculates trajectory using Bezier Curve (20 points plus the target position)
        Vector3 _initialPosition = transform.position;
        List<Vector3> _allPositions = new List<Vector3>(21);
        Vector3 control = (_initialPosition+target)/2 + new Vector3(0.0f,3.0f, 0);
        for (int i = 0; i < 20; i++)
        {
            Vector3 newPosition = BezierCurve(_initialPosition, control,
            target, (float)i / 20);
            _allPositions.Add(newPosition);
        }
        _allPositions.Add(target);
        // this bit depending on transition (side or corner square)
        // moves piece along the curve
        int counter = 0;
        // while piece is not on the target square and not finished rotating
        while(counter < _allPositions.Count)
        {
            if(MoveTo(_allPositions[counter]))
            {
                counter++;
            }
            yield return null;
        }
        // this just wait small amout of time before moving to the next square
        yield return new WaitForSeconds(0.1f);
        // remove the current area from freeSpaces
        route.squares[currentSquare].freeSpaces.Remove(currentArea);
        isMoving = false;
    }

    public void set_on_start()
    {
        moveInstant(0);
    }
}
