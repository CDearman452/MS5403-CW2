using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics; //########################################

public class CDM_Pathfind_unoptimized : MonoBehaviour
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
        go_gm = GameObject.Find(st_gmName);
        //CDM_Grid = go_gm.GetComponent<CDM_Grid>();
        CDM_Grid = go_gm.GetComponent<CDM_Grid>();
    }
    //====================================================================================================
    // Update is called once per frame
    void Update()
    {
        if (bl_findPath)
        {
            Pathfind(new Vector2(transform.position.x, transform.position.y), new Vector2(go_pc.transform.position.x, go_pc.transform.position.y));
            bl_findPath = false;
        }

    }
    //====================================================================================================
    //
    public void Pathfind(Vector2 v2_startPos, Vector2 v2_targetPos)
    {
        Node _nd_startNode = CDM_Grid.NodeFromWorldPoint(v2_startPos);
        Node _nd_targetNode = CDM_Grid.NodeFromWorldPoint(v2_targetPos);

        List<Node> nd_openSet = new List<Node>();
        HashSet<Node> nd_closedSet = new HashSet<Node>();

        nd_openSet.Add(_nd_startNode);

        while(nd_openSet.Count > 0)
        {
            if (_nd_targetNode.bl_nodeFree == false)
            {
                foreach (Node nd_neighbour in CDM_Grid.GetNeighbours(_nd_targetNode))
                {
                    if (nd_neighbour.bl_nodeFree == false || nd_closedSet.Contains(nd_neighbour))
                    {
                        continue;
                    }
                    else
                    {
                        _nd_targetNode = nd_neighbour;
                    }

                }
            }

            Node _nd_currentNode = nd_openSet[0];

            for (int x = 1; x < nd_openSet.Count; x++)
            {
                if (nd_openSet[x].in_fCost < _nd_currentNode.in_fCost || nd_openSet[x].in_fCost == _nd_currentNode.in_fCost && nd_openSet[x].in_hCost < _nd_currentNode.in_hCost)
                {
                    _nd_currentNode = nd_openSet[x];
                }
            }

            nd_openSet.Remove(_nd_currentNode);
            nd_closedSet.Add(_nd_currentNode);

            if (_nd_currentNode == _nd_targetNode)
            {
                RetracePath(_nd_startNode, _nd_targetNode);
                return;
            }

            foreach (Node nd_neighbour in CDM_Grid.GetNeighbours(_nd_currentNode))
            {
                if (nd_neighbour.bl_nodeFree == false || nd_closedSet.Contains(nd_neighbour))
                {
                    continue;
                }

                int _in_DistToNeighbour = _nd_currentNode.in_gCost + GetDistance(_nd_currentNode, nd_neighbour);

                if (_in_DistToNeighbour < nd_neighbour.in_gCost || nd_openSet.Contains(nd_neighbour) == false)
                {
                    nd_neighbour.in_gCost = _in_DistToNeighbour;
                    nd_neighbour.in_hCost = GetDistance(nd_neighbour, _nd_targetNode);
                    nd_neighbour.nd_previous = _nd_currentNode;

                    if (nd_openSet.Contains(nd_neighbour) == false) nd_openSet.Add(nd_neighbour);
                }
            }
        }
    }
    //====================================================================================================
    //
    void RetracePath(Node _start, Node _end)
    {
        nd_path = new List<Node>();
        Node _nd_currentNode = _end;

        while (_nd_currentNode != _start)
        {
            nd_path.Add(_nd_currentNode);
            _nd_currentNode = _nd_currentNode.nd_previous;
        }

        nd_path.Reverse();

        GameObject.Find("CDM_TopDownGameManager").GetComponent<CDM_Grid>().nd_path = nd_path; //########################################
        GetComponent<CDM_TopDownNPCHostile>().nd_path = nd_path;
    }
    //====================================================================================================
    // Custom method to calculate the gCost value of each node
    public int GetDistance(Node _nodeA, Node _nodeB)
    {
        int _in_posDifX = Mathf.Abs(_nodeA.in_gridPosX - _nodeB.in_gridPosX);
        int _in_posDifY = Mathf.Abs(_nodeA.in_gridPosY - _nodeB.in_gridPosY);

        if (_in_posDifX > _in_posDifY) return 14 * _in_posDifY + 10 * (_in_posDifX - _in_posDifY);
        else return 14 * _in_posDifX + 10 * (_in_posDifY - _in_posDifX);
    }
}