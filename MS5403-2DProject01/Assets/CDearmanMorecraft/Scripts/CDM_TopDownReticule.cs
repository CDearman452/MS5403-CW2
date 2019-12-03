using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDM_TopDownReticule : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 l_v2_MousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        if (transform.position != new Vector3(l_v2_MousePos.x, l_v2_MousePos.y, -1))
        {
            Vector2 l_v2_PosDif = l_v2_MousePos - new Vector2(transform.position.x, transform.position.y);
            GetComponent<Rigidbody2D>().velocity = l_v2_PosDif;
        }
        else GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}
