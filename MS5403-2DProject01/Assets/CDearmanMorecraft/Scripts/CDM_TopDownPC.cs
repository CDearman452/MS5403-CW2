using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDM_TopDownPC : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------
    // Variables
    private GameObject go_bulletSource;
    private GameObject go_gm;
    public GameObject go_bullet;

    private Rigidbody2D rb_pc;

    private Vector2 v2_input;
    private Vector2 v2_mousePos;
    private Vector2 v2_rayTarget;
    public Vector2 v2_posDif;
    public Vector2 v2_speedCo = new Vector2(5,5);
    public Vector2 v2_sprintSpeedCo = new Vector2(7.5f, 7.5f);

    public Vector3 v3_mouseOffSet;

    private int in_id;
    public int in_hpMax;
    public int in_hp;

    private float fl_angle;
    private float fl_time;
    public float fl_cooldown;

    private bool bl_fire = false;
    //----------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------
        // Set references for object components and gameobjects
        go_bulletSource = GameObject.Find("PCBulletSource"); // Seperate object childed to the PC to identify a position where the bullet can spawn without colliding with the PC
        go_gm = GameObject.Find("CDM_TopDownGameManager"); // The game manager gameobject
        rb_pc = GetComponent<Rigidbody2D>(); // Assign players rigidbody
        //----------------------------------------
        in_hp = in_hpMax; // Set the players health to it's maximum
    }
    //----------------------------------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        //----------------------------------------
        // Call Functions
        PCMove(); // Call the movement function each frame
        PCMouseFollow(); // Call the mouse based rotation function each frame
        PCProjectile(); // Call the projectiles spawning function
        //----------------------------------------
    }
    //----------------------------------------------------------------------------------------------------
    // Function that detects collisions with other objects
    private void OnCollisionEnter2D(Collision2D _col)
    {
        if (_col.transform.tag == "HostileNPC") // Check if the object collided with is an enemy
        {
            in_id = 0; // Set an ID value to identify the collision as an enemy
            HP(); // Call the HP governing function
        }
        else if (_col.transform.tag == "HealthPickup")
        {
            in_id = 1; // Set an ID value to identify the collision as a health pickup
            HP(); // Call the HP governing function
        }
        else if (_col.transform.tag == "Powerup")
        {
            //Do Shit
        }
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
        //----------------------------------------
        // Update vector2 representing mouse position to reflect the mouses screen position (Modified slightly to correct for custom cursor sprite) converted to cartesian coordinates
        v2_mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x + 14, Input.mousePosition.y - 16));
        //----------------------------------------
        v2_posDif = v2_mousePos - new Vector2(transform.position.x, transform.position.y); // Calculate the difference in x and y between the PC and the mouse position
        fl_angle = Mathf.Atan2(v2_posDif.y, v2_posDif.x) * Mathf.Rad2Deg; // Use the difference in x and y to calculate the angle to rotate using trig and convert it from radions to degrees
        transform.rotation = Quaternion.AngleAxis(fl_angle, Vector3.forward); // Rotate the PC around the z axis to match the angle calculated above
        //----------------------------------------
    }
    //----------------------------------------------------------------------------------------------------
    // Funtion that causes projectiles to fire when the left mouse button is clicked
    private void PCProjectile()
    {
        //----------------------------------------
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse0)) bl_fire = true; // Check if player has pressed or is holding the left mouse button and set a bool if they are
        else bl_fire = false; // If the player isn't pressing the left mouse key, set the bool to false
        //----------------------------------------
        if (bl_fire && fl_time <= Time.timeSinceLevelLoad) // Check bool and if shot cooldown time has been completed
        {
            // Instantiate a new bullet object with a transform shifted forward to avoid hitting the PC and at the same rotation
            Instantiate(go_bullet, new Vector3(go_bulletSource.transform.position.x, go_bulletSource.transform.position.y, 0), Quaternion.AngleAxis(fl_angle, Vector3.forward));
            fl_time = Time.timeSinceLevelLoad + fl_cooldown; // reset cooldown
        }
    }
    //----------------------------------------------------------------------------------------------------
    // Function that governs the Players Health Points
    private void HP()
    {
        //----------------------------------------
        // Increase/Decrease health when needed
        if (in_id == 0) // Check the assigned ID of the collided object to see if it is an enemy
        {
            // Calculate the damage dealt to the player with the formula: 1 + Difficulty level multiplied by 1.25 and rounded to the nearest whole number
            int _in_damage = 1 + Mathf.RoundToInt(go_gm.GetComponent<CDM_TopDownGameManager>().fl_enemyDif * 1.25f);
            in_hp -= _in_damage; // Apply the damage to the players health
        }
        else if (in_id == 1) // Check the assigned ID of the collided object to see if it is a health pickup   
        {
            if (in_hp > in_hpMax) in_hp++; // Increase the players health if it is smaller than the hp max
        }
        //----------------------------------------
        // Check if the player dies and end the game
        if (in_hp <= 0) // Check the players HP
        {
            go_gm.GetComponent<CDM_TopDownGameManager>().bl_gameOver = true; // Set bool in game manager to trigger GameOver function
            gameObject.SetActive(false); // Deactivate the Player Object
        }
    }
}
