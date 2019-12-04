using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDM_TopDownNPCHostile : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------
    // Variables
    public bool bl_hit;
    //----------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //----------------------------------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        
    }
    //----------------------------------------------------------------------------------------------------
    // Triggers when an object enters this objects collider
    private void OnTriggerEnter2D(Collider2D l_cl_obj)
    {
        if (l_cl_obj.transform.tag == "Bullet")
        {
            Debug.Log("Hit");
            Destroy(l_cl_obj);
            Destroy(gameObject);
        }
    }
}
