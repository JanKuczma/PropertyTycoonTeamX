using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceContainer : MonoBehaviour
{
    public Texture2D pointing_hand; // textures for cursor
    public Texture2D closed_hand;
    float move_speed = 20.0f;   // how fast dice is following cursor
    Vector3 previous_frame_pos; // parameter used to calculate dice velocity
    Dice[] dice;                // list for references to dice monobehaviour
    Vector3 init_pos;           // initial position of the container
    void Start()
    {       // setting up the parameters
        init_pos = transform.position;
        dice = GetComponentsInChildren<Dice>();
    }

    void OnMouseDown()
    {
        Cursor.SetCursor(closed_hand,Vector2.zero,CursorMode.Auto); // on click change cursor to 'closed hand'
    }
    void OnMouseDrag()
    {   // when dragging keep recalculating velocity and move towards cursor
        previous_frame_pos = transform.position;
        Vector3 targetPos = getTargetPos();
        transform.position = Vector3.MoveTowards(transform.position, targetPos, move_speed * Time.deltaTime);
    }
    void OnMouseUp()
    {   // on mouse button release change cursor to 'poiniting hand'
        Cursor.SetCursor(pointing_hand,Vector2.zero,CursorMode.Auto);
        foreach (Dice d in dice)    // for each dice assign velcity
        {
            d.roll((transform.position - previous_frame_pos)/Time.deltaTime);
        }
        enabled = false;    // disable the container collider component
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

    /// returns true if at least one dice is rolling
    public bool areRolling()
    {
        List<bool> tmp = new List<bool>();
        foreach(Dice d in dice)
        {
            tmp.Add(d.isRolling());
        }
        return tmp.Contains(true);
    }

}
