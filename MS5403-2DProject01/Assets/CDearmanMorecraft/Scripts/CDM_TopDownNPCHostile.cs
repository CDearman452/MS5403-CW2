using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDM_TopDownNPCHostile : MonoBehaviour
{
    //====================================================================================================
    // Variables
    private bool bl_stunned;
    public bool bl_hit;

    private Vector2 v2_posDif;
    private Vector2 v2_nodePosDif;
    private Vector2 v2_back;

    private Rigidbody2D rb_npc;

    private GameObject go_pc;
    private GameObject go_gm;
    public GameObject go_bulletSource;
    public GameObject go_bullet;

    private RaycastHit2D rc_ray;

    public List<Node> nd_path;

    private float fl_angle;
    private float fl_nodeAngle;
    private float fl_moveSpeed;
    public float fl_cooldown;
    public float fl_baseMoveSpeed;

    private int in_hp = 1;
    //====================================================================================================
    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------
        // Set references for object components and gameobjects
        go_pc = GameObject.Find("CDM_TopDownPC"); // The player gameobject
        go_gm = GameObject.Find("CDM_TopDownGameManager"); // The game manager gameobject
        rb_npc = GetComponent<Rigidbody2D>(); // The NPC's rigidbody
        //----------------------------------------
        // Set the target object in the pathfinding script
        GetComponent<CDM_Pathfind_unoptimized>().go_pc = go_pc;
        //----------------------------------------
        // Set move speed and toughness based on difficulty
        fl_moveSpeed = fl_baseMoveSpeed + (go_gm.GetComponent<CDM_TopDownGameManager>().fl_enemyDif / 2); // Set movement speed using formula: Base + half of Difficulty Level
        in_hp = 4 + Mathf.RoundToInt(go_gm.GetComponent<CDM_TopDownGameManager>().fl_enemyDif / 4); // Set health  using formula: 4 + one quarter of Difficulty Level rounded to the nearst whole number
        //----------------------------------------
        nd_path = GetComponent<CDM_Pathfind_unoptimized>().nd_path;
        //----------------------------------------
    }
    //====================================================================================================
    // Update is called once per frame
    void Update()
    {
        //----------------------------------------
        // Create raycast to check if the NPC has line of sight to the player
        v2_posDif = new Vector2(go_pc.transform.position.x, go_pc.transform.position.y) - new Vector2(transform.position.x, transform.position.y); // Calculate the difference in position between the NPC and the PC
        rc_ray = Physics2D.Raycast(new Vector2(go_bulletSource.transform.position.x, go_bulletSource.transform.position.y), v2_posDif, Mathf.Infinity); // Set raycast results variable to results of the raycast
        Debug.DrawLine(new Vector3(go_bulletSource.transform.position.x, go_bulletSource.transform.position.y, 0), new Vector3(rc_ray.point.x, rc_ray.point.y, 0)); // Debug check of raycast
        //----------------------------------------
        // Determine Behaviour
        if (bl_stunned == false) // Check that the NPC is not stunned from a hit
        {
            if (rc_ray.transform.tag != "Player") // Check if the NPC does not have a line of sight to the player object by checking the tag of the object the raycast first collides with
            {
                PathfollowBehaviour(); // If there is an indexed node at the begining of the path list, call the pathfollow function
            }
            else LOSBehaviour(); // If the NPC does have line of sight to the player call the Line of Sight Behaviour function
        }
        else // If the NPC is stunned from a hit
        {
            bl_stunned = false; // Set stunned check variable to false to 'un-stun' the NPC
            v2_back = new Vector2(transform.position.x, transform.position.y) - new Vector2(go_pc.transform.position.x, go_pc.transform.position.y); // Calculate a knockback vector that defines a position directly away from the PC
            rb_npc.velocity = v2_back.normalized * 10; // Apply that vector, reduced to a magnitude of 1 and multiplied by 10 to give a consistant speed regardless of distance
        }
        //----------------------------------------
    }
    //====================================================================================================
    // Triggers when an object enters this objects collider
    private void OnCollisionEnter2D(Collision2D _cl_obj)
    {
        if (_cl_obj.transform.tag == "Bullet") // Checks the tag of an object that enters the NPCs collider to see if it has been hit by a bullet
        {
            in_hp--; // Decrease HP
            bl_stunned = true; // Set a stunned variable to true
            //----------------------------------------
            if (in_hp == 0) // Check the NPCs remaining health
            {
                go_gm.GetComponent<CDM_TopDownGameManager>().in_kills += 1; // Increase the number of kills the game manager has tracked
                Destroy(gameObject); // Destroy the NPC
            }
            //----------------------------------------
        }
    }
    //====================================================================================================
    // Function that governs the NPCs behaviour when it has an unblocked line of sight to the player
    void LOSBehaviour()
    {
        //----------------------------------------
        // Rotation
        fl_angle = Mathf.Atan2(v2_posDif.y, v2_posDif.x) * Mathf.Rad2Deg; // Use the difference in x and y to calculate the angle to rotate using trig and convert it from radions to degrees
        transform.rotation = Quaternion.AngleAxis(fl_angle, Vector3.forward); // Rotate the NPC around the z axis to match the angle calculated above
        //----------------------------------------
        // Motion
        rb_npc.velocity = v2_posDif.normalized * fl_moveSpeed; // Apply a force equal to the difference in position between the PC and the NPC reduced to a magnitude of 1 and multiplied by a speed modifier
        //----------------------------------------
    }
    //====================================================================================================
    // Function that follows a path to the player that the pathfinding script finds
    void PathfollowBehaviour()
    {
        //----------------------------------------
        // Temp Variables
        bool _bl_move = false; // Check for if the NPC needs to move or update its path
        //----------------------------------------
        // If statement block to determine if the NPC is within a short distance of the center of the first node in its path to determin if it should update the path
        if (transform.position.x >= (nd_path[0].v2_nodePos.x - 0.25f) && transform.position.x <= (nd_path[0].v2_nodePos.x + 0.25f)) // Check if the NPC is within 0.25 units of the node on the x axis
        {
            if (transform.position.y >= (nd_path[0].v2_nodePos.y - 0.25f) && transform.position.y <= (nd_path[0].v2_nodePos.y + 0.25f)) // Check if the NPC is within 0.25 units of the node on the y axis
            {
                GetComponent<CDM_Pathfind_unoptimized>().bl_findPath = true; // If the NPC is at the first node in it's path, trigger the pathfinding script again so that it updates the path
            }
            else if (transform.position.y >= (nd_path[0].v2_nodePos.y + 2) || transform.position.y <= (nd_path[0].v2_nodePos.y -2)) // Check if the NPC is 2 units away from the node on the y axis
            {
                GetComponent<CDM_Pathfind_unoptimized>().bl_findPath = true; // If the NPC is too far away from the first node in it's path, trigger the pathfinding script agains so that it updates the path
            }
            else _bl_move = true; // If the NPC is not within range of the node on the y axis switch the bool that determines if the NPC needs to move
        }
        else if (transform.position.x >= (nd_path[0].v2_nodePos.x + 2) || transform.position.x <= (nd_path[0].v2_nodePos.x - 2)) // Check if the NPC is 2 units away from the node on the x axis
        {
            GetComponent<CDM_Pathfind_unoptimized>().bl_findPath = true; // If the NPC is too far away from the first node in it's path, trigger the pathfinding script agains so that it updates the path
        }
        else _bl_move = true; // If the NPC is not within range of the node on the x axis switch the bool that determines if the NPC needs to move
        //----------------------------------------
        // Movement between nodes
        if (_bl_move)
        {
            v2_nodePosDif = nd_path[0].v2_nodePos - new Vector2(transform.position.x, transform.position.y); // Calculate the difference in position between the NPC and the first node of their path
            //----------------------------------------
            // Rotation
            fl_nodeAngle = Mathf.Atan2(v2_nodePosDif.y, v2_nodePosDif.x) * Mathf.Rad2Deg; // Use the difference in x and y to calculate the angle to rotate using trig and convert it from radions to degrees
            transform.rotation = Quaternion.AngleAxis(fl_nodeAngle, Vector3.forward); // Rotate the NPC around the z axis to match the angle calculated above
            //----------------------------------------
            // Motion
            rb_npc.velocity = v2_nodePosDif.normalized * fl_moveSpeed; // Apply a force equal to the difference in position between the PC and the NPC reduced to a magnitude of 1 and multiplied by a speed modifier
            //----------------------------------------
        }

    }

}
