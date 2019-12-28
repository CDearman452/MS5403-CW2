using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node // New class for defining a 'node' to create an information repository for the A* algorithm
{
    //====================================================================================================
    // Variables
    public bool bl_nodeFree; // Definition for if a node's position is passable or not
    public Vector2 v2_nodePos; // Definition for the nodes position in the game world

    public Node (bool _nodeFree, Vector2 _nodePos) // Class constructor for Nodes
    {
        //----------------------------------------
        // Assign variables to the variables given to the constructor
        bl_nodeFree = _nodeFree;
        v2_nodePos = _nodePos;
        //----------------------------------------
    }
}
