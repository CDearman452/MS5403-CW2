using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDM_TopDownNPCHostile : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------
    // Variables
    public bool bl_hit;

    private Vector2 v2_posDif;

    private Rigidbody2D rb_npc;

    private GameObject go_pc;
    public GameObject go_bulletSource;
    public GameObject go_bullet;

    private RaycastHit2D rc_ray;

    private float fl_time = 0;
    private float fl_angle;
    public float fl_cooldown;
    public float fl_moveSpeed;
    //----------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------
        // Set references for object components and gameobjects
        go_pc = GameObject.Find("CDM_TopDownPC"); // The player gameobject
        rb_npc = GetComponent<Rigidbody2D>(); // The NPC's rigidbody
        //----------------------------------------
    }
    //----------------------------------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        v2_posDif = new Vector2(go_pc.transform.position.x - transform.position.x, go_pc.transform.position.y - transform.position.y); // Find the differnce between the PC's and the NPC's position
        if (v2_posDif.x < 10 && v2_posDif.y < 10) // Check if the difference is less than 10 on the x&y axis in any direction
        {
            if (v2_posDif.x > -10 && v2_posDif.y > -10) ShortRangeBehaviour(); // If it is, call the function that governs close ranged behaviour
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
        //----------------------------------------
        // Create raycast to check if the NPC has line of sight to the player
        rc_ray = Physics2D.Raycast(new Vector2(go_bulletSource.transform.position.x, go_bulletSource.transform.position.y), v2_posDif, Mathf.Infinity);
        Debug.DrawLine(new Vector3(go_bulletSource.transform.position.x, go_bulletSource.transform.position.y, 0), new Vector3(rc_ray.point.x, rc_ray.point.y, 0));
        //----------------------------------------
        // Determine Behaviour
        if (rc_ray.transform.tag != "Player") // Check if the NPC does not have a line of sight to the player
        {
            Pathfind(); // Call the path-finding function again
        }
        else // If the NPC does have line of sight to the player
        {
            //----------------------------------------
            fl_angle = Mathf.Atan2(v2_posDif.y, v2_posDif.x) * Mathf.Rad2Deg; // Use the difference in x and y to calculate the angle to rotate using trig and convert it from radions to degrees
            transform.rotation = Quaternion.AngleAxis(fl_angle, Vector3.forward); // Rotate the NPC around the z axis to match the angle calculated above
            //----------------------------------------
            rb_npc.velocity = v2_posDif.normalized * fl_moveSpeed; // Apply a force equal to the difference in position between the PC and the NPC reduced to a magnitude of 1 and multiplied by a speed modifier
            //----------------------------------------
        }
        //----------------------------------------
    }
    //----------------------------------------------------------------------------------------------------
    // Function that finds a path to a target object that avoids assigned obstacles
    void Pathfind()
    {
        //Debug.Log("You Fucked it up");
    }

}
