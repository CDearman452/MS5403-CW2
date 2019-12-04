using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDM_TopDownReticule : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false; // Make the cursor invisible
        Cursor.lockState = CursorLockMode.Confined; // Lock the cursor to the game window
    }
    //----------------------------------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        // Check the mouses current position in the camera, convert to world space and store as a vector
        Vector2 l_v2_MousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        transform.position = new Vector3(l_v2_MousePos.x, l_v2_MousePos.y, -1); // Update Reticule Objects position to match the cursors
    }
}
