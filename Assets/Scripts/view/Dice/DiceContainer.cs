using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

///DICE IMPLEMENTATION NEEDS REFACTORING
namespace View{
public class DiceContainer : MonoBehaviour
{
    float move_speed = 20.0f;   // how fast dice is following cursor
    Vector3 previous_frame_pos; // parameter used to calculate dice velocity
    Dice[] dice;                // list for references to dice monobehaviour
    Vector3 init_pos;           // initial position of the container
    [System.NonSerialized] public bool start_roll; 
    void Awake()
    {
        init_pos = transform.position;
        dice = GetComponentsInChildren<Dice>();
        start_roll = false;
    }

    public static DiceContainer Create(Transform parent)
    {
        return Instantiate(Asset.Dice(),parent).GetComponent<DiceContainer>();
    }

    void OnMouseDown()
    {
        Cursor.SetCursor(Asset.Cursor(CursorType.GRAB),Vector2.zero,CursorMode.Auto); // on click change cursor to 'closed hand'
    }
    void OnMouseDrag()
    {   // when dragging keep recalculating velocity and move towards cursor
        previous_frame_pos = transform.position;
        Vector3 targetPos = getTargetPos();
        if(transform.position.y < 1.5f || Mathf.Abs(transform.position.x) > 18.3f || Mathf.Abs(transform.position.z) > 10.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position,Vector3.up*10.0f,move_speed * Time.deltaTime);
        } else {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, move_speed * Time.deltaTime);
        }
    }
    void OnMouseUp()
    {   // on mouse button release change cursor to 'poiniting hand'
        Cursor.SetCursor(Asset.Cursor(CursorType.FINGER),Vector2.zero,CursorMode.Auto);
        foreach (Dice d in dice)    // for each dice assign velcity
        {
            d.roll((transform.position - previous_frame_pos)/Time.deltaTime);
        }
        start_roll = true;
        GetComponent<BoxCollider>().enabled = false;
        enabled = false;    // disable the container
    }

    Vector3 getTargetPos()
    {       // create a plane perpendicular to the camera's forward vector
        Plane plane = new Plane(Camera.main.transform.forward,transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    // get the ray going from camera towards cursor

        float point = 0f;
        Vector3 targetPos = transform.position;
        if(plane.Raycast(ray, out point))
        {       // get the point on the plane where cursor is pointing
            targetPos = ray.GetPoint(point);
        }
        return targetPos;
    }

    /// resets dice to initial position
    public void reset()
    {
        transform.position = init_pos;
        foreach(Dice d in dice)
        {
            d.reset();
        }
        start_roll = false;
        GetComponent<BoxCollider>().enabled = true;
        enabled = true;
    }

    /// returns the sum of dice results
    public int get_result()
    {
        int result = 0;
        foreach(Dice d in dice)
        {
            result += d.get_value();
        }
        return result;
    }

    public bool is_double()
    {
        int i = 0;
        int[] dice_values = new int[2];
        foreach (Dice d in dice)
        {
            Debug.Log(d.get_value());
            dice_values[i] = d.get_value();
            i++;
        }
        
        Debug.Log(dice_values[0]);
        Debug.Log(dice_values[1]);
        Debug.Log(dice_values[0] == dice_values[1]);

        return (dice_values[0] == dice_values[1]);
    }

    /// returns true if at least one dice is rolling
    public bool areRolling()
    {
        foreach(Dice d in dice)
        {
            if(d.isRolling()) return true;
        }
        return false;
    }

    public Vector3 position()
    {
        Vector3 point = Vector3.zero;
        foreach(Dice d in dice)
        {
            point += d.transform.position;
        }
        return point/dice.Length;
    }
    public float av_distance()
    {
        return (position()-dice[0].transform.position).magnitude;
    }

    public bool belowBoard()
    {
        foreach(Dice d in dice)
        {
            if(d.transform.position.y <= 0.15f) { return true; }
        }
        return false;
    }
}
}
