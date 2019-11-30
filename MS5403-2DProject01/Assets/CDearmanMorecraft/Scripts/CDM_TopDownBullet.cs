using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDM_TopDownBullet : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------
    // Variables
    private Vector2 v2_direction;
    public float fl_speed;
    //----------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        v2_direction = GameObject.Find("CDM_TopDownPC").GetComponent<CDM_TopDownPC>().v2_posDif; // Retrieve directional vector reflecting a straight path to the mouse from the PC
        v2_direction.Normalize(); // Reduce it's magnituted to maintain the direction but alter the speed
        GetComponent<Rigidbody2D>().velocity = v2_direction * fl_speed; // Set it as a new velocity, multiplied by the speed variable

        GetComponent<Rigidbody2D>().freezeRotation = true; // Lock the bullets rotation to stop it from occasionally spinning
    }
    //----------------------------------------------------------------------------------------------------
    // Called when a collision between two colliders/rigidbodies occurs
    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if the bullet has hit an enemy, if it has change a variable in the enemies script to reflect this
        if (collision.transform.tag == "HostileNPC") collision.transform.GetComponent<CDM_TopDownNPCHostile>().bl_hit = true;
        Destroy(gameObject); // Destroy the bullet
    }
}
