using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDM_TopDownNPCHostile : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------
    // Variables
    public bool bl_hit;

    private Vector2 v2_posDif;

    private GameObject go_pc;
    public GameObject go_bulletSource;
    public GameObject go_bullet;

    private RaycastHit2D rc_ray;
    //----------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------
        // Set references for object components and gameobjects
        go_pc = GameObject.Find("CDM_TopDownPC");
        //----------------------------------------
    }
    //----------------------------------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        v2_posDif = new Vector2(go_pc.transform.position.x - transform.position.x, go_pc.transform.position.y - transform.position.y); // Find the differnce between the PC's and the NPC's position
        if (v2_posDif.x < 10 && v2_posDif.y < 10) // Check if the difference is less than 10 on the x&y axis in any direction
        {
            if (v2_posDif.x > 10 && v2_posDif.y > 10) ShortRangeBehaviour(); // If it is, call a behaviour determining function
        }
        else Pathfind(); // If it isn't, call the pathfinding function to move towards the PC
    }
    //----------------------------------------------------------------------------------------------------
    // Triggers when an object enters this objects collider
    private void OnTriggerEnter2D(Collider2D l_cl_obj)
    {
        if (l_cl_obj.transform.tag == "Bullet") // Checks the tag of an object that enters the NPCs collider to see if it has been hit by a bullet
        {
            Debug.Log("Hit"); // Debug Check
            Destroy(l_cl_obj); // Destroy the bullet object
            Destroy(gameObject); // Destroy the NPC
        }
    }
    //----------------------------------------------------------------------------------------------------
    // Function that governs the NPCs behaviour when within a short distance of the pc
    void ShortRangeBehaviour()
    {
        //Debug.Log("Near");

        // Create raycast to check if the NPC has line of sight to the player
        rc_ray = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(go_pc.transform.position.x, go_pc.transform.position.y));

        if (rc_ray.collider.transform.tag != "Player")
        {
            // Timer to generate a new behaviour and carry it out every five seconds (strafe, wait, move forward, jitter etc)
        }
        else
        {
            //Shoot at player
        }
    }
    //----------------------------------------------------------------------------------------------------
    // Function that finds a path to a target object that avoids assigned obstacles
    void Pathfind()
    {
        //Debug.Log("You Fucked it up");
    }

}
