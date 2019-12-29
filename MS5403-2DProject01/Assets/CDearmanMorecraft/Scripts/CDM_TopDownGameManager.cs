using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CDM_TopDownGameManager : MonoBehaviour
{
    //====================================================================================================
    // Variables
    private int in_score = 0;
    private int in_minutes = 0;
    private int in_time = 0;
    private int in_pcHP;
    public int in_kills = 0;
    public int in_items = 0;
    public int in_damageTaken = 0;

    private float fl_gameTime = 0;
    private float fl_tempTime = 0;
    private float fl_seconds;
    private float fl_halfMin = 0;
    public float fl_enemyDif = 0;

    private string st_minutes;
    private string st_seconds;
    private string st_score;

    private GameObject go_pc;
    public GameObject[] go_healthIcons;
    public GameObject go_gameOverScreen;
    public GameObject go_scoreText;
    public GameObject go_highscore01;
    public GameObject go_highscore02;
    public GameObject go_highscore03;
    public GameObject go_highscore04;
    public GameObject go_highscore05;

    private TextMeshProUGUI tmp_timeText;

    public Sprite[] sp_health;

    private bool bl_check = false;
    public bool bl_gameOver;
    //====================================================================================================
    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------
        // Set references for object components and gameobjects
        go_pc = GameObject.Find("CDM_TopDownPC"); // The Player Character
        tmp_timeText = GameObject.Find("TimeText").GetComponent<TextMeshProUGUI>(); // The text mesh pro component attached to a ui GameObject
        //----------------------------------------
        // Get player health from the PC gameobject
        in_pcHP = go_pc.GetComponent<CDM_TopDownPC>().in_hpMax;
        //----------------------------------------
    }
    //====================================================================================================
    // Update is called once per frame
    void Update()
    {
        Timer(); // Calls the Timer Function
        if (bl_gameOver == true) GameOver(); // Checks a bool that is changed when the player dies, if the bool is true call the function for the game over screen
        //----------------------------------------
        // If statement to stop the script running the entire HealthUpdate() function every frame
        if (in_pcHP != go_pc.GetComponent<CDM_TopDownPC>().in_hp) // Check if a local version of the players hp matches the pc's version
        {
            HealthUpdate(); // Calls the function that updates the health icons
            in_pcHP = go_pc.GetComponent<CDM_TopDownPC>().in_hp; // Update the game managers version of the players hp
        }
        //----------------------------------------
    }
    //====================================================================================================
    // Function that updates the health icons to represent the players current health
    void HealthUpdate()
    {
        if (go_pc.GetComponent<CDM_TopDownPC>().in_hp >= 16) // Check if health is higher than 16
        {
            //----------------------------------------
            // Set full and empty health icons using an array containing the icon gameobjects and another containing the sprites
            go_healthIcons[0].GetComponent<Image>().sprite = sp_health[4]; 
            go_healthIcons[1].GetComponent<Image>().sprite = sp_health[4];
            go_healthIcons[2].GetComponent<Image>().sprite = sp_health[4];
            go_healthIcons[3].GetComponent<Image>().sprite = sp_health[4];
            //----------------------------------------
            // Set more specific health icons using an array containing the icon gameobjects and another containing the sprites
            if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 16) go_healthIcons[4].GetComponent<Image>().sprite = sp_health[0];
            else if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 17) go_healthIcons[4].GetComponent<Image>().sprite = sp_health[1];
            else if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 18) go_healthIcons[4].GetComponent<Image>().sprite = sp_health[2];
            else if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 19) go_healthIcons[4].GetComponent<Image>().sprite = sp_health[3];
            else if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 20) go_healthIcons[4].GetComponent<Image>().sprite = sp_health[4];
            //----------------------------------------
        }
        else if (go_pc.GetComponent<CDM_TopDownPC>().in_hp >= 12 && go_pc.GetComponent<CDM_TopDownPC>().in_hp < 16) // Check if health is between 12 and 16
        {
            //----------------------------------------
            // Set full and empty health icons using an array containing the icon gameobjects and another containing the sprites
            go_healthIcons[0].GetComponent<Image>().sprite = sp_health[4];
            go_healthIcons[1].GetComponent<Image>().sprite = sp_health[4];
            go_healthIcons[2].GetComponent<Image>().sprite = sp_health[4];
            go_healthIcons[4].GetComponent<Image>().sprite = sp_health[0];
            //----------------------------------------
            // Set more specific health icons using an array containing the icon gameobjects and another containing the sprites
            if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 12) go_healthIcons[3].GetComponent<Image>().sprite = sp_health[0];
            else if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 13) go_healthIcons[3].GetComponent<Image>().sprite = sp_health[1];
            else if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 14) go_healthIcons[3].GetComponent<Image>().sprite = sp_health[2];
            else if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 15) go_healthIcons[3].GetComponent<Image>().sprite = sp_health[3];
            //----------------------------------------
        }
        else if (go_pc.GetComponent<CDM_TopDownPC>().in_hp >= 8 && go_pc.GetComponent<CDM_TopDownPC>().in_hp < 12) // Check if health is between 8 and 12
        {
            //----------------------------------------
            // Set full and empty health icons using an array containing the icon gameobjects and another containing the sprites
            go_healthIcons[0].GetComponent<Image>().sprite = sp_health[4];
            go_healthIcons[1].GetComponent<Image>().sprite = sp_health[4];
            go_healthIcons[3].GetComponent<Image>().sprite = sp_health[0];
            go_healthIcons[4].GetComponent<Image>().sprite = sp_health[0];
            //----------------------------------------
            // Set more specific health icons using an array containing the icon gameobjects and another containing the sprites
            if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 8) go_healthIcons[2].GetComponent<Image>().sprite = sp_health[0];
            else if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 9) go_healthIcons[2].GetComponent<Image>().sprite = sp_health[1];
            else if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 10) go_healthIcons[2].GetComponent<Image>().sprite = sp_health[2];
            else if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 11) go_healthIcons[2].GetComponent<Image>().sprite = sp_health[3];
            //----------------------------------------
        }
        else if (go_pc.GetComponent<CDM_TopDownPC>().in_hp >= 4 && go_pc.GetComponent<CDM_TopDownPC>().in_hp < 8) // Check if health is between 4 and 8
        {
            //----------------------------------------
            // Set full and empty health icons using an array containing the icon gameobjects and another containing the sprites
            go_healthIcons[0].GetComponent<Image>().sprite = sp_health[4];
            go_healthIcons[2].GetComponent<Image>().sprite = sp_health[0];
            go_healthIcons[3].GetComponent<Image>().sprite = sp_health[0];
            go_healthIcons[4].GetComponent<Image>().sprite = sp_health[0];
            //----------------------------------------
            // Set more specific health icons using an array containing the icon gameobjects and another containing the sprites
            if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 4) go_healthIcons[1].GetComponent<Image>().sprite = sp_health[0];
            else if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 5) go_healthIcons[1].GetComponent<Image>().sprite = sp_health[1];
            else if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 6) go_healthIcons[1].GetComponent<Image>().sprite = sp_health[2];
            else if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 7) go_healthIcons[1].GetComponent<Image>().sprite = sp_health[3];
            //----------------------------------------
        }
        else if (go_pc.GetComponent<CDM_TopDownPC>().in_hp >= 0 && go_pc.GetComponent<CDM_TopDownPC>().in_hp < 4) // Check if health is between 0 and 4
        {
            //----------------------------------------
            // Set full and empty health icons using an array containing the icon gameobjects and another containing the sprites
            go_healthIcons[1].GetComponent<Image>().sprite = sp_health[0];
            go_healthIcons[2].GetComponent<Image>().sprite = sp_health[0];
            go_healthIcons[3].GetComponent<Image>().sprite = sp_health[0];
            go_healthIcons[4].GetComponent<Image>().sprite = sp_health[0];
            //----------------------------------------
            // Set more specific health icons using an array containing the icon gameobjects and another containing the sprites
            if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 0) go_healthIcons[0].GetComponent<Image>().sprite = sp_health[0];
            else if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 1) go_healthIcons[0].GetComponent<Image>().sprite = sp_health[1];
            else if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 2) go_healthIcons[0].GetComponent<Image>().sprite = sp_health[2];
            else if (go_pc.GetComponent<CDM_TopDownPC>().in_hp == 3) go_healthIcons[0].GetComponent<Image>().sprite = sp_health[3];
            //----------------------------------------
        }
    }
    //====================================================================================================
    // Function that tracks the time since the beginning of the game and converts it to a clear display
    void Timer()
    {
        fl_gameTime += Time.deltaTime; // Tracks time since the game start un-altered for scoring purposes (Uses the time since the begining of the frame to allow for variable frame rate)
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
        // Set the UI text element referenced on start to display the time in the format minutes:seconds/00:00 using the strings created above
        tmp_timeText.text = st_minutes + ":" + st_seconds;
        //----------------------------------------
        // Caculate global value for enemy difficulty
        fl_halfMin += Time.deltaTime; // Add the time since the start of the frame to a float
        if (fl_halfMin > 30) // If the float is larger than 30 (representing the passage of 30 seconds)
        {
            fl_enemyDif++; // Increase the global enemy difficulty value
            fl_halfMin = 0; // Reset the time float
        }
        //----------------------------------------
    }
    //====================================================================================================
    // Function that calculates the players score based off of time, kills, items, and damage taken
    void CalcScore()
    {
        in_time = Mathf.RoundToInt(fl_gameTime); // Converts the unmodified game time in seconds to the nearest int
        // Calculates score using the formula of: time in seconds + number of Kills (multiplied by five) + number of items colleced (multiplied by two) - the total points of damage the player has taken throughout the game (multiplied by 10)
        in_score = in_time + (in_kills * 5) + (in_items * 2) - (in_damageTaken * 5);
        st_score = "Score: " + in_score; // Apply the players score to a string
        //----------------------------------------
        // Store Scores
        if (PlayerPrefs.GetInt("Highscore01") > in_score) // Check if the players score is lower than the first highscore
        {
            if (PlayerPrefs.GetInt("Highscore02") > in_score) // Check if the players score is lower than the second highscore
            {
                if (PlayerPrefs.GetInt("Highscore03") > in_score) // Check if the players score is lower than the third highscore 
                {
                    if (PlayerPrefs.GetInt("Highscore04") > in_score) // Check if the players score is lower than the fourth highscore
                    {
                        if (PlayerPrefs.GetInt("Highscore05") < in_score) // Check if the players score is higher than the fifth highscore
                        {
                            PlayerPrefs.SetInt("Highscore05", in_score); // Set the fifth highscore to be the players score
                        }
                    }
                    else PlayerPrefs.SetInt("Highscore04", in_score);// If the players score is higher than the fourth highscore set it to be the players score
                }
                else PlayerPrefs.SetInt("Highscore03", in_score);// If the players score is higher than the third highscore set it to be the players score
            }
            else PlayerPrefs.SetInt("Highscore02", in_score);// If the players score is higher than the second highscore set it to be the players score
        }
        else PlayerPrefs.SetInt("Highscore01", in_score);// If the players score is higher than the first highscore set it to be the players score
        //----------------------------------------
        PlayerPrefs.Save(); // Write changes to disk
        //----------------------------------------
    }
    //====================================================================================================
    // Function that governs the end game screen and its functions
    void GameOver()
    {
        if (bl_check == false) // Check a boolian to insure that the GameOver script only runs once
        {
            Time.timeScale = 0; // Freeze the game world
            CalcScore(); // Call the score calculation function
            //----------------------------------------
            // Update GameOverScreen
            go_gameOverScreen.SetActive(true); // Activate the parent object that has all of the Game Over Screen elements childed to it
            go_scoreText.GetComponent<TextMeshProUGUI>().text = st_score; // Set the Score Text on the Game Over Screen to reflect the calculated score
            go_highscore01.GetComponent<TextMeshProUGUI>().text = "1: " + PlayerPrefs.GetInt("Highscore01"); // Set the first highscore text object to reflect the corresponding stored highscore
            go_highscore02.GetComponent<TextMeshProUGUI>().text = "2: " + PlayerPrefs.GetInt("Highscore02"); // Set the second highscore text object to reflect the corresponding stored highscore
            go_highscore03.GetComponent<TextMeshProUGUI>().text = "3: " + PlayerPrefs.GetInt("Highscore03"); // Set the third highscore text object to reflect the corresponding stored highscore
            go_highscore04.GetComponent<TextMeshProUGUI>().text = "4: " + PlayerPrefs.GetInt("Highscore04"); // Set the fourth highscore text object to reflect the corresponding stored highscore
            go_highscore05.GetComponent<TextMeshProUGUI>().text = "5: " + PlayerPrefs.GetInt("Highscore05"); // Set the fi highscore text object to reflect the corresponding stored highscore
            //----------------------------------------
            bl_check = true; // Switch boolian
            //----------------------------------------
        }
    }
}