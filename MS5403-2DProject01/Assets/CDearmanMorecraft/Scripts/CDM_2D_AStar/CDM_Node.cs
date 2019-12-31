//====================================================================================================
// ATTENTION --- A* Scripting referenced from Sebastian Lagues youtube series covering the topic
// Link --- https://www.youtube.com/playlist?list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW
//====================================================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node // New class for defining a 'node' to create an information repository for the A* algorithm
{
    //====================================================================================================
    // Variables
    public bool bl_nodeFree; // Definition for if a node's position is passable or not

    public Vector2 v2_nodePos; // Definition for the nodes position in the game world

    public Node nd_previous;

    public int in_gCost; // Distance from Start Point
    public int in_hCost; // Distance from End Point
    public int in_gridPosX;
    public int in_gridPosY;
    //====================================================================================================
    // Class Contruction
    public Node(bool _nodeFree, Vector2 _nodePos, int _gridPosX, int _gridPosY) // Class constructor for Nodes
    {
        //----------------------------------------
        // Assign class variables to the variables given to the constructor
        bl_nodeFree = _nodeFree;
        v2_nodePos = _nodePos;
        in_gridPosX = _gridPosX;
        in_gridPosY = _gridPosY;
        //----------------------------------------
    }
    //====================================================================================================
    // Reference int for retrieving the fCost of the node (The combined distance value from the origin node and from the target node
    public int in_fCost
    {
        get // Can be retrieved but not set
        {
            return in_gCost + in_hCost; // returns the nodes gCost + its hCost
        }
    }
}