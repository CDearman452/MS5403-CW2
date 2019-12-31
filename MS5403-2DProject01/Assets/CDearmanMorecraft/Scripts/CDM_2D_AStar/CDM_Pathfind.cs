//====================================================================================================
// ATTENTION --- A* Scripting referenced from Sebastian Lagues youtube series covering the topic
// Link --- https://www.youtube.com/playlist?list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW
//====================================================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDM_Pathfind : MonoBehaviour
{
    //====================================================================================================
    // Variables
    private CDM_Grid CDM_Grid;
    private GameObject go_gm;

    public List<Node> nd_path;

    public string st_gmName;

    public bool bl_findPath = true;

    public GameObject go_pc;
    //====================================================================================================
    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------
        // Reference components and gameobjects
        go_gm = GameObject.Find(st_gmName); // The GameManager gameobject
        CDM_Grid = go_gm.GetComponent<CDM_Grid>(); // The Grid Script attached to the GameManager gameobject
        //----------------------------------------
    }
    //====================================================================================================
    // Update is called once per frame
    void Update()
    {
        //----------------------------------------
        // Boolian Dependant if statement to stop the pathfinding function being run every frame unnecessarily
        if (bl_findPath) // If bool is set to true by the npc script
        {
            // Call the pathfinding function with the NPC's current position as the start position and the PC's position as the target position
            Pathfind(new Vector2(transform.position.x, transform.position.y), new Vector2(go_pc.transform.position.x, go_pc.transform.position.y));
            bl_findPath = false; // Set the bool back to false;
        }
        //----------------------------------------

    }
    //====================================================================================================
    // Function that finds the shortest path from one node to another and sets parents to be used as a marker for retrieving the path
    public void Pathfind(Vector2 v2_startPos, Vector2 v2_targetPos)
    {
        //----------------------------------------
        // Initialize temporary node variables to represent the start and target nodes
        // Create a new node to represent the start node and use NodeFromWorldPoint method in the grid script to convert the start position to its relevent node and set it
        Node _nd_startNode = CDM_Grid.NodeFromWorldPoint(v2_startPos);
        // Create a new node to represent the target node and use NodeFromWorldPoint method in the grid script to convert the target position to its relevent node and set it
        Node _nd_targetNode = CDM_Grid.NodeFromWorldPoint(v2_targetPos);
        //----------------------------------------
        // Create a list and a hashset to store the set of nodes to be evaluated and the nodes that have already been evaluated respectively
        List<Node> nd_openSet = new List<Node>();
        HashSet<Node> nd_closedSet = new HashSet<Node>();
        //----------------------------------------
        // Add the start node to the open set of nodes
        nd_openSet.Add(_nd_startNode);
        //----------------------------------------
        // While the open count still contains nodes to evaluate
        while (nd_openSet.Count > 0)
        {
            //----------------------------------------
            // Block that finds a new target node if the target node is invalid
            if (_nd_targetNode.bl_nodeFree == false) // Check if the target node is impassable
            {
                foreach (Node nd_neighbour in CDM_Grid.GetNeighbours(_nd_targetNode)) // For every node in the list of neighbours of the target node
                {
                    if (nd_neighbour.bl_nodeFree == false || nd_closedSet.Contains(nd_neighbour)) // Check if the node is impassable or already in the set of evaluated nodes
                    {
                        continue; // Skip to the next loop
                    }
                    else // If the node is free and not already in the set of evaluated nodes
                    {
                        _nd_targetNode = nd_neighbour; // Set the target node to be that node
                    }

                }
            }
            //----------------------------------------
            // Create a new node to represent the node currently being evaluated and set it to be the first node in the open set
            Node _nd_currentNode = nd_openSet[0];
            //----------------------------------------
            // For loop which finds the lowest fCost node in the open set of nodes and sets it to be the current node
            for (int x = 1; x < nd_openSet.Count; x++) // Continues looping until it reaches an equal number of loops to the number of nodes in the open set of nodes
            {
                // Check if the fCost of the node in the open set with an index equal to the loop iteration has a lower fCost than the current node, or if it has the same fCost and a lower hCost
                if (nd_openSet[x].in_fCost < _nd_currentNode.in_fCost || nd_openSet[x].in_fCost == _nd_currentNode.in_fCost && nd_openSet[x].in_hCost < _nd_currentNode.in_hCost)
                {
                    _nd_currentNode = nd_openSet[x]; // Set the current node to be the node in the open set with an index cost equal to the loop iteration
                }
            }
            //----------------------------------------
            // Now that the current node is the most efficient next step, move it from the open set to the closed set
            nd_openSet.Remove(_nd_currentNode); // Remove current node from open set
            nd_closedSet.Add(_nd_currentNode); // Add current node to closed set
            //----------------------------------------
            // Check if the current node is the target node
            if (_nd_currentNode == _nd_targetNode)
            {
                RetracePath(_nd_startNode, _nd_targetNode); // Call the function to create a list with the complete path
                return; // Exit the while loop
            }
            //----------------------------------------
            // For every node in the list of neighbours of the current node
            foreach (Node nd_neighbour in CDM_Grid.GetNeighbours(_nd_currentNode))
            {
                //----------------------------------------
                // Check if the node being evaluated is impassable or already in the closed set of nodes
                if (nd_neighbour.bl_nodeFree == false || nd_closedSet.Contains(nd_neighbour))
                {
                    continue; // Skip to the next iteration of the loop
                }
                //----------------------------------------
                // Create a new temporary value to represent the distance between the current node and the node being evaluated
                int _in_DistToNeighbour = _nd_currentNode.in_gCost + GetDistance(_nd_currentNode, nd_neighbour);
                //----------------------------------------
                // Check if the distance to the node being evaluated is lower than it's current gCost (Which would mean that this is a more efficient path to that node) -
                // or if the open set does not already contain the node being evaluated
                if (_in_DistToNeighbour < nd_neighbour.in_gCost || nd_openSet.Contains(nd_neighbour) == false)
                {
                    //----------------------------------------
                    // Update the gCost and the hCost of the node being evaluated as well updating the variable it uses to store the node that comes prior to it on the path
                    nd_neighbour.in_gCost = _in_DistToNeighbour; // Set the gCost of the node being evaluated to be equal to the distance value calculated above
                    nd_neighbour.in_hCost = GetDistance(nd_neighbour, _nd_targetNode); // Set the hCost of the node being evaluated to be equal to its distance from the target node (as the crow flies)
                    nd_neighbour.nd_previous = _nd_currentNode; // Set the previous node in the path for the node being evaluated to be the current node
                    //----------------------------------------
                    // Check if the open set does not already contain the node being evaluated
                    if (nd_openSet.Contains(nd_neighbour) == false) nd_openSet.Add(nd_neighbour); // Add the node being evaluated to the open set
                    //----------------------------------------
                }
                //----------------------------------------
            }
            //----------------------------------------
        }
        //----------------------------------------
    }
    //====================================================================================================
    // Function that start with the target node and follows the parent nodes to find the path from the start node to the target node
    void RetracePath(Node _start, Node _end)
    {
        //----------------------------------------
        // Set the path of nodes list to be an empty list of nodes and create a new temporary node variable, and set it to be the inputed end node
        nd_path = new List<Node>();
        Node _nd_currentNode = _end;
        //----------------------------------------
        // Loops until the current node is the same as the inputed start node
        while (_nd_currentNode != _start)
        {
            nd_path.Add(_nd_currentNode); // Add the current node (which starts as the target node) to the path of nodes list
            _nd_currentNode = _nd_currentNode.nd_previous; // Change the current node to be the current nodes previous node
        }
        //----------------------------------------
        // Reverse the path of nodes list to start with the start node and end with the target node
        nd_path.Reverse();
        //----------------------------------------
        // Distribute the path of nodes list
        GameObject.Find("CDM_TopDownGameManager").GetComponent<CDM_Grid>().nd_path = nd_path; // Set the path of nodes list in the gamemanagers script to be the same as the local one for gizmo path visualization
        GetComponent<CDM_TopDownNPCHostile>().nd_path = nd_path; // Set the path of nodes list in the NPC script to be equal to the local one so that it can follow the nodes
        //----------------------------------------
    }
    //====================================================================================================
    // Custom method to calculate the gCost value of each node
    public int GetDistance(Node _nodeA, Node _nodeB)
    {
        //----------------------------------------
        // Create temporary values to represent the position difference between the input nodes
        int _in_posDifX = Mathf.Abs(_nodeA.in_gridPosX - _nodeB.in_gridPosX);
        int _in_posDifY = Mathf.Abs(_nodeA.in_gridPosY - _nodeB.in_gridPosY);
        //----------------------------------------
        // Check if the position difference on the x axis is larger than the position difference on the y axis
        if (_in_posDifX > _in_posDifY) return 14 * _in_posDifY + 10 * (_in_posDifX - _in_posDifY); // Return 14 for each diagonal node of movement to reach the same Y position as the target + 10 for each direct node of movement to reach the targets X position
        else return 14 * _in_posDifX + 10 * (_in_posDifY - _in_posDifX); // If the position difference on the Y axis is larger than the position difference on the X axis; Return 14 for each diagonal node of movement to reach the same X position as the target-
                                                                         // + 10 for each direct node of movement to reach the targets Y position
    }
}