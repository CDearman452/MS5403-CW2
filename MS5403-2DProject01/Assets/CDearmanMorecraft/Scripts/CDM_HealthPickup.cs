using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDM_HealthPickup : MonoBehaviour
{
    //====================================================================================================
    // Variables
    private float fl_life;
    public bool bl_destroy;
    //====================================================================================================
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)); // Give the pickup a small velocity in a random direction when it spawns from its crate
    }
    //====================================================================================================
    // Update is called once per frame
    void Update()
    {
        //----------------------------------------
        // Update the total lifetime of the object by adding the time since the end of the last frame
        fl_life += Time.deltaTime;
        //----------------------------------------
        // Liftime statement block
        if (fl_life >= 30) Destroy(gameObject); // Destroy the gameobject if it has been active from 30 or more seconds
        else if (fl_life >= 27) // Check if the object has been active for less than 30 seconds but more than 27
        {
            // Activate and deactivate the sprite renderer every other frame to simulate flashing, showing that the object is reaching the end of its lifetime;
            if (GetComponent<SpriteRenderer>().enabled == false) GetComponent<SpriteRenderer>().enabled = true;
            else GetComponent<SpriteRenderer>().enabled = false;
        }
        //----------------------------------------
        if (bl_destroy) // Check if the destory boolian has been made true by the PC script
        {
            Destroy(gameObject); // Destroy the pickup
        }
        //----------------------------------------
    }
}