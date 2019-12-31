using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDM_TopDownCrate : MonoBehaviour
{
    //====================================================================================================
    // Variables
    public GameObject go_healthDrop;
    //====================================================================================================
    // OnCollisionEnter2D is called when the object the script is attached to collides with another collider and takes a reference to the collider
    private void OnCollisionEnter2D(Collision2D _col)
    {
        if (_col.transform.tag == "Bullet") // Check if the object that the crate collided with has the bullet tag
        {
            //----------------------------------------
            // Set a random value to represent 
            int _in_dropNum = Random.Range(0, 3);
            //----------------------------------------
            // For loops which spawns a number of health pickups equal to the random value rolled above
            for (int x = 0; x < _in_dropNum; x++)
            {
                // Instantiates a new health pickup gameobject at the same position as the crate, with a random rotation;
                Instantiate<GameObject>(go_healthDrop, new Vector2(transform.position.x, transform.position.y), Quaternion.AngleAxis(Random.Range(0, 361), Vector3.forward));
            }
            //----------------------------------------
            // Destroy the crate gameobject
            Destroy(gameObject);
        }
    }
}
