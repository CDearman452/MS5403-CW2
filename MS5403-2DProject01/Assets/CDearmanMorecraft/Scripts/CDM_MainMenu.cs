using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CDM_MainMenu : MonoBehaviour
{
    //====================================================================================================
    // Variables
    public Texture2D tx_cursor;
    //====================================================================================================
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock the Cursor
        Cursor.SetCursor(tx_cursor, new Vector2(0, 0), CursorMode.Auto); // Change the cursor sprite to a custom cursor with a standard active point
    }
    //====================================================================================================
    // Start Button Function which will be called by a UI Button Object in the menu scene
    public void StartGame()
    {
        SceneManager.LoadScene(1); // Initiate the second scene in the build
    }
    //====================================================================================================
    // Exit Button Function which will be called by a UI Button Object in the Menu scene
    public void ExitGame()
    {
        Application.Quit(); // Shut down the game
    }
}