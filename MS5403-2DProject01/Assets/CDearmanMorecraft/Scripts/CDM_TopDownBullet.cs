using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDM_TopDownBullet : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------
    // Variables
    private Rigidbody2D rb_bul;
    private Vector2 v2_direction;
    public float fl_speed;
    //----------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        rb_bul = GetComponent<Rigidbody2D>(); // Assign Rigidbody to variable
        //----------------------------------------
        // Move the bullet away from the camera to fix issue of bullets not destroying fast enough after collision due to their velocity
        transform.position = new Vector3(transform.position.x, transform.position.y, 1);
        //----------------------------------------
        v2_direction = GameObject.Find("CDM_TopDownPC").GetComponent<CDM_TopDownPC>().v2_posDif; // Retrieve directional vector representing a straight path to the mouse from the PC
        v2_direction.Normalize(); // Reduce it's magnituted to maintain the direction but alter the speed
        rb_bul.velocity = v2_direction * fl_speed; // Set it as a new velocity, multiplied by the speed variable
        //----------------------------------------
        rb_bul.freezeRotation = true; // Lock the bullets rotation to stop it from occasionally spinning
    }
    //----------------------------------------------------------------------------------------------------
    // Called when a collision between two colliders occurs
    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb_bul.velocity = new Vector2(0, 0); // Stop the bullet 
        Destroy(gameObject); // Destroy the bullet
    }
}