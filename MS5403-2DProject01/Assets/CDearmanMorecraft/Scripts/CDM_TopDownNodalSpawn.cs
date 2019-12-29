using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CDM_TopDownNodalSpawn : MonoBehaviour
{
    //====================================================================================================
    // Variables
    private GameObject[] go_spawnNodes;
    private GameObject[] go_unseenSpawnNodes;
    private GameObject[] go_randomSpawnNodes;
    public GameObject go_pc;
    public GameObject go_npcPrefab;

    public Camera cm_mainCam;

    private float fl_time;
    private float fl_enemyDif;

    private int in_time;
    private int in_arrayPos;
    private int in_enemyNum;
    private int in_rand;
    public int in_spawnDelay;
    public int in_nodeNum;
    //====================================================================================================
    // Start is called before the first frame update
    void Start()
    {
        go_spawnNodes = new GameObject[in_nodeNum]; // Set Array Length to the number of nodes in the scene
        //----------------------------------------
        // Fills an array with every node object in the scene
        for (int x = 0; x < in_nodeNum; x++) // For loop checking for a number of loops equal to the defined number of nodes
        {
            go_spawnNodes[x] = GameObject.Find("SpawnNode" + (x + 1)); // Adds a gameobject with matching name to the array each loop, using the loops value to define unique objects and array indexes
        }
        //----------------------------------------
    }
    //====================================================================================================
    // Update is called once per frame
    void Update()
    {
        //----------------------------------------
        // Time Track
        fl_time += Time.deltaTime; // Add the time since the beginning of the frame to a float
        in_time = Mathf.RoundToInt(fl_time); // Round the above float to an integer
        //----------------------------------------
        // Time Check
        if (in_time >= in_spawnDelay) // Check if the int defined above is larger than or equal to a pre-determing delay value
        {
            // Establish the size of the viable spawn nodes array
            go_unseenSpawnNodes = new GameObject[in_nodeNum];
            //----------------------------------------
            // Fill an array with spawn nodes that aren't on screen
            foreach (GameObject _obj in go_spawnNodes) // Repeat the below block for every GameObject within the array defined in Start()
            {
                // Check if the GameObjects y position is above or below the cameras bounds
                if (_obj.transform.position.y > cm_mainCam.orthographicSize + 1 || _obj.transform.position.y < 0 - (cm_mainCam.orthographicSize + 1))
                {
                    // Check if the GameObjects x position is to the right or left of the cameras bounds
                    if (_obj.transform.position.x > (cm_mainCam.orthographicSize + (cm_mainCam.orthographicSize / 3)) + 2 || _obj.transform.position.x < 0 - ((cm_mainCam.orthographicSize + (cm_mainCam.orthographicSize / 3)) + 2)) 
                    {
                        // Add the GameObject from the array to an array of nodes that are outside of the cameras bounds in an index position equal to the number of times the loop has successfully completed
                        go_unseenSpawnNodes[in_arrayPos] = _obj;
                        in_arrayPos++; // Increase the value that determines the index position in the array of nodes outside of the cameras bounds
                    }
                }
            }
            //----------------------------------------
            // Calculate the number of enemies to spawn based off of the difficulty level
            fl_enemyDif = GetComponent<CDM_TopDownGameManager>().fl_enemyDif; // Retrieve the enemy difficulty value from the gamemanager script
            if (fl_enemyDif > 0) in_enemyNum = Mathf.RoundToInt(fl_enemyDif) * 5; // If the difficulty value is larger than zero, the number of enemies to spawn is equal to the difficulty value multiplied by 5
            else in_enemyNum = 5; // If the difficulty value is smaller than, or equal to, zero the number of enemies to spawn is five
            go_randomSpawnNodes = new GameObject[in_enemyNum]; // Set the array representing the randomised nodes that the enemies will spawn at to be the same size as the number of enemies to be spawned
            //----------------------------------------
            // Fill an array with a number of random nodes, from the valid nodes array, equal to the number of enemies to spawn
            for (int x = 0; x < in_enemyNum; x++) // For loop checking for a number of loops equal to the number of enemies to spawn
            {
                in_rand = Mathf.RoundToInt(UnityEngine.Random.Range(0, go_unseenSpawnNodes.Length - 1)); // Randomize a value between 0 and the number of indexes within the array - 1
                // Check if the index within the array of viable nodes that matches the randomized value is null. If it is, re-randomize. Otherwise move on.
                while (go_unseenSpawnNodes[in_rand] == null) in_rand = Mathf.RoundToInt(UnityEngine.Random.Range(0, go_unseenSpawnNodes.Length - 1));
                // Each iteration of the loop, randomly selects a node from the array of viable nodes and adds it to an array of random nodes
                go_randomSpawnNodes[x] = go_unseenSpawnNodes[in_rand];
            }
            //----------------------------------------
            // Spawn an enemy object in the same position as each node in the array
            foreach (GameObject _obj in go_randomSpawnNodes) // Repeat the below block for every GameObject within the array of random nodes
            {
                // Create a new gameobject identical to a prefab, in the same position as the node of the loops iteration with 0 rotation around the z axis
                Instantiate(go_npcPrefab, new Vector2(_obj.transform.position.x, _obj.transform.position.y), Quaternion.AngleAxis(0, Vector3.forward));
            }
            //----------------------------------------
            // Reset Values
            fl_time = 0; // Reset the Timer
            in_arrayPos = 0; // Reset the array index position value
            Array.Clear(go_unseenSpawnNodes, 0, go_unseenSpawnNodes.Length); // Empty the array of viable nodes to be reconstructed when next required
            //----------------------------------------
        }
        //----------------------------------------
    }
}
