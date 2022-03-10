using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View{
public class Piece : MonoBehaviour
{
    const float SPEED = 6f;    // const movement speed
    float var_speed;    // variable movement speed
    float rotationSpeed;    // rotation speed
    int currentSquare;  // current position 0 - 39 (40 squares)
    int currentSpot; // current spot 0 - 5 (6 areas)
    [System.NonSerialized] public bool isMoving;   // bool to control the movement
    Board _board; // reference to Board
    void Awake()
    {
        // sets up initial values
        isMoving = false;
        var_speed = SPEED;
        rotationSpeed = 4f;
        currentSpot = -1;
        currentSquare = 0;
    }

    public static Piece Create(Token token, Transform parent, Board board)
    {
        Piece new_piece = Instantiate(Asset.Piece(token),parent).GetComponent<Piece>();
        new_piece._board = board;
        new_piece.transform.localPosition = new Vector3(0,0.1f,0);
        new_piece.moveInstant(0);
        return new_piece;
    }

    /// coroutine for movement
    /// accepts positive and negative values
    /// if negative value then moves backwards
    public IEnumerator move(int steps)
    {
        // this "if" stops another dice roll while object is in move
        if(isMoving)
        {
            yield break;
        }
        isMoving = true;
        //if value is negative then moves bakckwards
        int iterator;
        if(steps >= 0)
        {
            iterator = 1;
        } else {
            iterator = -1;
            steps *= -1;
        }
        // free the current area
        _board.squares[currentSquare].releaseSpotI(currentSpot);
        while(steps > 0)
        {
            // because negative iterator in case 0 - 1 we would like to get 39
            currentSquare = currentSquare == 0 ? 40 : currentSquare;
            int nextSquare = (currentSquare + iterator) % 40;
            // target is just position of the next square (free spot)
            currentSpot = _board.squares[nextSquare].peekSpotI();
            Vector3 target = _board.squares[nextSquare].peekSpot(currentSpot);
            // the target height is the same as current piece height
            target[1] = transform.position.y;
            // this bit basically calculates trajectory using Bezier Curve (20 points plus the target position)
            Vector3 control = (transform.position+target)/2 + new Vector3(0.0f,3.0f, 0);
            List<Vector3> path = BezierCurve(transform.position,control,target);
            // this bit depending on transition (side or corner square)
            // moves piece along the curve
            int counter = 0;
            if((iterator >= 0 ? nextSquare : currentSquare) % 10 == 0) // if nextsqure is a corner (when currentsquare for going backwards)
            {
                Vector3 targetRotation = iterator >= 0 ? transform.right : transform.right*(-1);    // if going forward turn right else turn left
                // while piece is not on the target square and not finished rotating
                while(counter < path.Count && rotate(targetRotation))
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
            // decrement steps and increment currentSquare (0 - 39)
            steps--;
            currentSquare = nextSquare;
        }
        // remove the current spot from freeSpots
        _board.squares[currentSquare].removeSpotI(currentSpot);
        var_speed = SPEED;
        isMoving = false;
    }

    // moves instantaneously to specified square
    public void moveInstant(int square)
    {
        _board.squares[currentSquare].releaseSpotI(currentSpot);
        // target is just position of the next square (free spot)
        currentSpot = _board.squares[square].popSpotI();
        Vector3 target = _board.squares[square].peekSpot(currentSpot);
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
        _board.squares[currentSquare].releaseSpotI(currentSpot);
        currentSquare = 10; // jail square
        currentSpot = _board.jail.popCellI();
        Vector3 target = _board.jail.peekCell(currentSpot);
        // the target height is the same as current piece height
        target[1] = transform.position.y;
        // create control point and build the path along curve
        Vector3 control = target + new Vector3(0.0f,3.0f, 0);
        List<Vector3> path = BezierCurve(transform.position,control,target);
        int counter = 0;
        // while piece is not on the target square and not finished rotating
        while(counter < path.Count && rotate(Vector3.right))
        {
            if(moveTo(path[counter])) // move to next point on the path
            {
                counter++;      // if already there increment the path point index
            }
            yield return null;
        }
        var_speed = SPEED;
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
        _board.jail.releaseCellI(currentSpot);
        currentSpot = _board.squares[currentSquare].popSpotI();
        Vector3 target = _board.squares[currentSquare].peekSpot(currentSpot);
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
        var_speed = SPEED;
        isMoving = false;
    }

    /// speeds up piece movement
    public void speedUp(int x = 2)
    {
        var_speed = x*SPEED;
    }

    // moves piece towards next position and returns true if already on the target
    private bool moveTo(Vector3 targetPos)
    { 
        return targetPos == (transform.position = Vector3.MoveTowards(transform.position,targetPos,var_speed*Time.deltaTime));
    }
    // rotates piece towards specified direction, returns false if on the target
    private bool rotate(Vector3 targetRotation)
    {
        return Quaternion.Euler(targetRotation) != 
            (transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward,targetRotation,rotationSpeed*Time.deltaTime,0.0f)));
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
}
