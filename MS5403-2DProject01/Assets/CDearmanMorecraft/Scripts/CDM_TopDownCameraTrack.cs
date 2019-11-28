using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDM_TopDownCameraTrack : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------
    // Variables
    public GameObject go_target;
    //----------------------------------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(go_target.transform.position.x, go_target.transform.position.y, -10);
        // Set X&Y position to be the same as the target object.
    }
}
