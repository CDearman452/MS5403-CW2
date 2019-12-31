//====================================================================================================
// ATTENTION --- A* Scripting referenced from Sebastian Lagues youtube series covering the topic
// Link --- https://www.youtube.com/playlist?list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW
//====================================================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDM_Grid : MonoBehaviour
{
    //====================================================================================================
    // Variables
    public GameObject go_pc;

    public Vector2 v2_gridSize;
    public Vector2 v2_gridBottomLeftPos;

    public LayerMask lm_walls;

    private Node[,] nd_grid; // A 2D array of Nodes which stores information using two definitions rather than one
    public List<Node> nd_path;

    public bool bl_drawPathOnly;

    private int in_gridSizeX;
    private int in_gridSizeY;

    public float fl_nodeDiameter;
    //====================================================================================================
    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------
        // Set individual int's to represent the x and y length of the grid for easier reference
        in_gridSizeX = Mathf.RoundToInt(v2_gridSize.x);
        in_gridSizeY = Mathf.RoundToInt(v2_gridSize.y);
        //----------------------------------------
        CreateGrid(); // Call the grid constructing function
        //----------------------------------------
    }
    //====================================================================================================
    // Function that compiles a series of nodes filling the pre-determined grid size, into a single array
    void CreateGrid()
    {
        //----------------------------------------
        // Set the 2D node arrays length to match the x and y size of the grid so that it can define each node by position
        nd_grid = new Node[in_gridSizeX, in_gridSizeY];
        //----------------------------------------
        // For statement block that fills the node array row by row
        for (int x = 0; x < in_gridSizeX; x++)
        {
            for (int y = 0; y < in_gridSizeY; y++)
            {
                // Define the nodes position in a temp variable, by adding the diameter of each node (multiplied by the for loops iteration) to the bottom left node.
                Vector2 _v2_nodePos = v2_gridBottomLeftPos + new Vector2(x * fl_nodeDiameter, y * fl_nodeDiameter);
                // Define if the node is free by checking for a collision in the same space as the node, within the layer mask: "Walls"
                bool _bl_nodeFree = !(Physics2D.OverlapCircle(_v2_nodePos, (fl_nodeDiameter / 2), lm_walls));
                // Apply the above temp variables to the corresponding node in the array, as well as it's x and y value in the grid, using the for loop iteration values
                nd_grid[x, y] = new Node(_bl_nodeFree, _v2_nodePos, x, y);
            }
        }
        //----------------------------------------
    }
    //====================================================================================================
    // Custom method for nodes that converts a given worldspace position to the closest node
    public Node NodeFromWorldPoint(Vector2 v2_worldPos)
    {
        //----------------------------------------
        // Calculate the percentage of the X&Y axis that the worldspace points correspond to
        float _fl_percentX = (v2_worldPos.x + v2_gridSize.x / 2) / v2_gridSize.x;
        float _fl_percentY = (v2_worldPos.y + v2_gridSize.y / 2) / v2_gridSize.y;
        //----------------------------------------
        // Lock the percentages to an equivelent value between 0-1
        _fl_percentX = Mathf.Clamp01(_fl_percentX);
        _fl_percentY = Mathf.Clamp01(_fl_percentY);
        //----------------------------------------
        // Convert the percentages to X&Y values based on the size of the grid to find the corresponding node
        int x = Mathf.RoundToInt((in_gridSizeX - 1) * _fl_percentX);
        int y = Mathf.RoundToInt((in_gridSizeY - 1) * _fl_percentY);
        //----------------------------------------
        // Return the node with the calculated X&Y values
        return nd_grid[x, y];
        //----------------------------------------
    }
    //====================================================================================================
    // Public method for lists of nodes which returns the neighboring nodes of the input node
    public List<Node> GetNeighbours(Node _node)
    {
        //----------------------------------------
        // Initialize a list of nodes to fill with neighbours
        List<Node> nd_neighbours = new List<Node>();
        //----------------------------------------
        // For loops which evaluates 9 total nodes, each within 1 on the x and/or the y axis
        for (int x = -1; x <=1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //----------------------------------------
                // Check if the node being evaluated is the same node that was input
                if (x == 0 && y ==0 ) continue;
                //----------------------------------------
                // Set temp variables to represent the x and y position of the node being evaluated using the loop iteration and the x and y position of the input node
                int _in_checkX = _node.in_gridPosX + x;
                int _in_checkY = _node.in_gridPosY + y;
                //----------------------------------------
                // Check if the node being evaluated is within the grid
                if (_in_checkX >= 0 && _in_checkX < in_gridSizeX && _in_checkY >= 0 && _in_checkY < in_gridSizeY)
                {
                    nd_neighbours.Add(nd_grid[_in_checkX, _in_checkY]); // Add the node being evaluated to the list of neighbours
                }
            }
        }
        //----------------------------------------
        // Return the list of neighbours once it is full
        return nd_neighbours;
    }
    //====================================================================================================
    // Public interger that can be retrieved as an extension of the grid and returns the total number of nodes within the grid
    public int MaxSize
    {
        get
        {
            return in_gridSizeX * in_gridSizeY;
        }
    }
    //====================================================================================================
    // Path & grid visualization using gizmos
    void OnDrawGizmos()
    {
        //----------------------------------------
        // Draw a cube with the size of the grid
        Gizmos.DrawWireCube(transform.position, new Vector2(v2_gridSize.x, v2_gridSize.y));
        //----------------------------------------
        // Check if the boolian determining if only the path is drawn returns true
        if (bl_drawPathOnly)
        {
            if (nd_path != null) // Check that the path list isn't empty
            {
                foreach (Node n in nd_path) // For every node in the list
                {
                    //----------------------------------------
                    // Draw a cyan wireframe sphere with the position of the node
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireSphere(n.v2_nodePos, fl_nodeDiameter / 2);
                    //----------------------------------------
                }
            }
        }
        else // If the boolian determining if only the path is drawn returns false
        {
            if (nd_grid != null) // Check that the path array isn't empty
            {
                //----------------------------------------
                // Create a node to represent the PC
                Node nd_pc = NodeFromWorldPoint(new Vector2(go_pc.transform.position.x, go_pc.transform.position.y));
                //----------------------------------------
                // For every node in the grid array
                foreach (Node n in nd_grid)
                {
                    //----------------------------------------
                    // Colour the node being evaluated
                    if (n.bl_nodeFree) Gizmos.color = Color.green; // If the node is in a free position colour the node green
                    else Gizmos.color = Color.red; // If the node is in the same position as a wall, colour it red
                    if (nd_pc == n) Gizmos.color = Color.blue; // if the node is in the same position as the pc draw the node blue
                    //----------------------------------------
                    // Colour the path
                    if (nd_path != null) // Check that the path list is not empty
                    {
                        if (nd_path.Contains(n)) Gizmos.color = Color.cyan; // If the node being evaluated is in the path list of nodes colour it cyan
                    }
                    //----------------------------------------
                    // Draw a wiresphere with the colour determined above, in the position of the node being evaluated
                    Gizmos.DrawWireSphere(n.v2_nodePos, fl_nodeDiameter / 2);
                    //----------------------------------------
                }
            }
        }
    }
}
