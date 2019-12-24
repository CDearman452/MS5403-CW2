using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CDM_PauseManager : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------
    // Variables
    public GameObject go_pauseMenu;
    public bool bl_paused = false;
    public Texture2D tx_cursor01;
    public Texture2D tx_cursor02;
    // Start is called before the first frame update
    void Start()
    {
        CursorChange(); // Call the Cursor Change Function at the start of the scene
    }
    //----------------------------------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) // Check if the player presses either escape or the P button to pause/unpause the game
        {
            if (bl_paused == false) // Check if the game is not already paused
            {
                Time.timeScale = 0; // Stop time passage in the scene
                go_pauseMenu.SetActive(true); // Activate the Pause Menu parent object
                bl_paused = true; // Update the paused boolian to reflect the current paused state
                CursorChange(); // Call the Cursor Change function
            }
            else // If the game is already paused
            {
                Time.timeScale = 1; // Restart time passage in the scene
                go_pauseMenu.SetActive(false); // Turn off the Pause Menu parent Object
                bl_paused = false; // Update the paused boolian to reflect the current un-paused state
                CursorChange();// Call the Cursor Change function
            }
        }
    }
    //----------------------------------------------------------------------------------------------------
    // Function that alters the cursor
    void CursorChange()
    {
        if (bl_paused == false) // Check if the game is not paused
        {
            Cursor.lockState = CursorLockMode.Confined; // Lock the cursor to the game window
            Cursor.SetCursor(tx_cursor01, new Vector2(5, -5), CursorMode.Auto); // Change the cursor sprite to a custom reticule and off-set the cursors active point to center on the reticule
        }
        else // If the game is paused
        {
            Cursor.lockState = CursorLockMode.None; // Unlock the Cursor
            Cursor.SetCursor(tx_cursor02, new Vector2(0, 0), CursorMode.Auto); // Change the cursor sprite to a custom cursor with a standard active point
        }
    }
    //----------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------
    // Continue Pause Button Function
    public void Continue()
    {
        Time.timeScale = 1; // Restart time passage in the scene
        go_pauseMenu.SetActive(false); // Turn off the Pause Menu parent Object
        bl_paused = false; // Update the paused boolian to reflect the current un-paused state
        CursorChange();// Call the Cursor Change function
    }
    //----------------------------------------------------------------------------------------------------
    // Restart Pause Button Function
    public void Restart()
    {
        Time.timeScale = 1; // Restart time passage in the scene
        SceneManager.LoadScene(1); // Initiate the second scene in the build
    }
    //----------------------------------------------------------------------------------------------------
    // Quit Pause Button Function
    public void QuitToMenu()
    {
        Time.timeScale = 1; // Restart time passage in the scene
        SceneManager.LoadScene(0); // Initiate the first scene in the build
    }
    //----------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------
}