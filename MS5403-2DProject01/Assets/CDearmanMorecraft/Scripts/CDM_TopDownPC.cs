using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDM_TopDownPC : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------
    // Variables
    private Rigidbody2D rb_pc;

    private Vector2 v2_input;
    private Vector2 v2_mousePos;
    private Vector2 v2_posDif;
    public Vector2 v2_speedCo = new Vector2(5,5);
    public Vector2 v2_sprintSpeedCo = new Vector2(7.5f, 7.5f);

    private float fl_angle;
    //----------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------
        // Set references for object components
        rb_pc = GetComponent<Rigidbody2D>();

        //----------------------------------------

    }
    //----------------------------------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        PCMove(); // Call the movement function each frame
        PCMouseFollow(); // Call the mouse based rotation function each frame
        PCProjectile(); // Call the projectiles spawning function
    }
    //----------------------------------------------------------------------------------------------------
    // Simple 2D top-down movement function
    private void PCMove()
    {
        v2_input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // Update the input vector2 to reflect wasd/up,down,left,right key inputs
        if (Input.GetKey(KeyCode.LeftShift) == true) v2_input *= v2_sprintSpeedCo; // Check if the player is pressing shift and increase input vector2 using pre-set sprinting multiplier
        else v2_input *= v2_speedCo; // Check that player isn't pressing sprint and increase input vector2 using pre-set standard multiplier
        rb_pc.velocity = v2_input; // Update PC object velocity based on the input vector2
        rb_pc.angularVelocity = 0; // Set Angular Velocity to zero -- Solves bug where traversing sideways into walls would build angular velocity causing continued sideways movement after releaseing keys
    }
    //----------------------------------------------------------------------------------------------------
    // Function that causes the pc to constantly face the mouse
    private void PCMouseFollow()
    {
        // Update vector2 representing mouse position to reflect the mouses screen position converted to cartesian coordinates
        v2_mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

        v2_posDif = v2_mousePos - new Vector2(transform.position.x, transform.position.y); // Calculate the difference in x and y between the PC and the mouse position
        fl_angle = Mathf.Atan2(v2_posDif.y, v2_posDif.x) * Mathf.Rad2Deg; // use the difference in x and y to calculate the angle to rotate using pythagoras' theorem and convert it from radions to degrees
        transform.rotation = Quaternion.AngleAxis(fl_angle, Vector3.forward); // Rotate the PC around the z axis to match the angle calculated above
    }
    //----------------------------------------------------------------------------------------------------
    // Funtion that causes projectiles to fire when the left mouse button is clicked
    private void PCProjectile()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
             
        }
    }
}
