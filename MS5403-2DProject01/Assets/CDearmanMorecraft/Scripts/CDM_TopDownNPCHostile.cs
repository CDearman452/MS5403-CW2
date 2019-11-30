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
        if (bl_hit == true)
        {
            Debug.Log("Hit");
            Destroy(gameObject);
        }
    }
}
