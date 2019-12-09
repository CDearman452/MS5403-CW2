using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CDM_TopDownGameManager : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------
    // Variables
    private int in_score = 0;
    private int in_time = 0;
    private int in_kills = 0;
    private int in_items = 0;
    private int in_damageTaken = 0;
    private int in_minutes = 0;

    private float fl_gameTime = 0;
    private float fl_tempTime = 0;
    private float fl_seconds;

    private string st_minutes;
    private string st_seconds;

    private TextMeshProUGUI tmp_timeText;

    public bool bl_gameOver;
    //----------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------
        // Set references for object components and gameobjects
        tmp_timeText = GameObject.Find("TimeText").GetComponent<TextMeshProUGUI>();
        //----------------------------------------
    }
    //----------------------------------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        Timer(); // Calls the Timer Function
        if (bl_gameOver == true) GameOver(); // Checks a bool that is changed when the player dies, if the bool is true call the function for the game over screen
    }
    //----------------------------------------------------------------------------------------------------
    // Function that tracks the time since the beginning of the game and converts it to a clear display
    void Timer()
    {
        fl_gameTime += Time.deltaTime; // Tracks time since the game start un-altered for scoreing purposes (Uses the time since the begining of the frame to allow for variable frame rate)
        //----------------------------------------
        fl_tempTime += Time.deltaTime; // Tracks number of seconds that have passed each minute (Uses the time since the begining of the frame to allow for variable frame rate)
        fl_seconds = Mathf.Round(fl_tempTime); // Rounds the above time to an integer for display
        //----------------------------------------
        // Statement series that sets a string to represent a minutes:second format
        if (fl_seconds <= 9) st_seconds = "0" + fl_seconds; // If less than 10 seconds have passed set the seconds string to add a zero in front of the seconds value
        else if (fl_seconds <= 59) st_seconds = fl_seconds.ToString(); // If more than 9 but less than 60 seconds have passed use only the seconds value converting to a string format
        else if (fl_seconds >= 60) // Check if 60 seconds or more have passed
        {
            in_minutes += 1; // Increase the minutes value
            fl_tempTime = 0; // Reset the seconds value by setting the time counter to zero
            st_seconds = "00"; // Set seconds string to 00
        }
        if (in_minutes <= 9) st_minutes = "0" + in_minutes; // If less than 10 minutes have passed set the minutes string to add a zero in front of the minutes value
        else if (in_minutes >= 10) st_minutes = in_minutes.ToString(); // If more than or equal to 10 set the minutes string to only use the minutes value converter to a string format
        //----------------------------------------
        tmp_timeText.text = st_minutes + ":" + st_seconds; // Set the UI text element referenced on start to display the time in the format minutes:seconds/00:00 using the strings created above
    }
    //----------------------------------------------------------------------------------------------------
    // Function that calculates the players score based off of time, kills, items, and damage taken
    void CalcScore()
    {
        in_time = Mathf.RoundToInt(fl_gameTime); // Converts the unmodified game time in seconds to the nearest int
        // Calculates score using the formula of: time in seconds + number of Kills (multiplied by five) + number of items colleced (multiplied by two) - the total points of damage the player has taken throughout the game (multiplied by 10)
        in_score = in_time + (in_kills * 5) + (in_items * 2) - (in_damageTaken * 10);
    }
    //----------------------------------------------------------------------------------------------------
    // Function that governs the end game screen and its functions
    void GameOver()
    {
        CalcScore(); // Call the score calculation function
        // Do Stuff
    }
}