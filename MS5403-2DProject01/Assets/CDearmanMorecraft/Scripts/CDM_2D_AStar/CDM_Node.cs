using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> // New class for defining a 'node' to create an information repository for the A* algorithm. Also impliments the interface from the Heap Optimization script with a type Node
{
    //====================================================================================================
    // Variables
    public bool bl_nodeFree; // Definition for if a node's position is passable or not

    public Vector2 v2_nodePos; // Definition for the nodes position in the game world

    public Node nd_previous;

    int heapIndex;
    public int in_gCost; // Distance from Start Point
    public int in_hCost; // Distance from End Point
    public int in_gridPosX;
    public int in_gridPosY;

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

    public int in_fCost
    {
        get
        {
            return in_gCost + in_hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int _comparisonValue = in_fCost.CompareTo(nodeToCompare.in_fCost);

        if (_comparisonValue == 0)
        {
            _comparisonValue = in_hCost.CompareTo(nodeToCompare.in_hCost);
        }

        return -_comparisonValue;
    }
}