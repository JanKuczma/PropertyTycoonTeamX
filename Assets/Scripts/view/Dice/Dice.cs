using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    Rigidbody rb_comp;  // reference to RigidBody component
    Vector3 init_pos;   // initial position
    bool onGround;  // parameter used to keep track if dice is colliding with something
    void Awake()
    {
        rb_comp = GetComponent<Rigidbody>();
        init_pos = transform.position;
        onGround = false;
    }

    /// return the value of the side that is pointing up
    /// if none of the side is pointing up then returns -7
    public int get_value()
    {
        int val = -7;
        if(transform.right.y > .99f) {val = 3;}
        else if(transform.right.y < -.99f) {val = 5;}
        else if(transform.up.y > .99f) {val = 2;}
        else if(transform.up.y < -.99f) {val = 4;}
        else if(transform.forward.y > .99f) {val = 6;}
        else if(transform.forward.y < -.99f){val = 1;}
        return val;
        
    }

    /// returns true if dice is not colliding with anything OR velocity is not 0
    public bool isRolling()
    {
        return !(onGround && rb_comp.velocity.magnitude == 0);
    }

    /// starts the dice roll
    public void roll(Vector3 velocity)
    {
        rb_comp.useGravity = true;
        rb_comp.velocity = velocity;
    }

    /// if colliding with something the onGround set to true
    void OnTriggerStay(Collider col)
    {
        if(col.tag == "NotTrigger") return;
        onGround = true;
    }

    /// if stops colliding with something onGround set to false
    void OnTriggerExit()
    {
        onGround = false;
    }

    /// resets dice to initial position and rotates it with some pseudorandom values
    public void reset()
    {
        transform.position = init_pos;
        transform.eulerAngles = new Vector3(Random.Range(0.0f,360.0f),Random.Range(0.0f,360.0f),Random.Range(0.0f,360.0f));
        rb_comp.useGravity = false;
        rb_comp.velocity = Vector3.zero;
    }

}
