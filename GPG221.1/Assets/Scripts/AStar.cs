using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    [SerializeField] Vector3Int startPosition;
    [SerializeField] Vector3Int goalPosition;
    [SerializeField] Vector3Int currentPosition;

    [SerializeField] UnitMovement unit;

    Node startNode;
    Node goalNode;
    Node currentNode;

    List<Node> neighbours = new List<Node>();
    List<Node> openList = new List<Node>();
    List<Node> closedList = new List<Node>();
    public List<Node> finalPath = new List<Node>();

    int globalVersion = 0;


    Grid grid;

    void Start()
    {
        grid = FindObjectOfType<Grid>();

        startPosition = grid.WorldToGridPosition(unit.transform.position);
        startNode = grid.GetNode(startPosition);
        startNode.NodeObject.GetComponent<Renderer>().material.color = Color.green;

        currentNode = startNode;
        openList.Add(currentNode);
        globalVersion++;
    }

    int CalculateDistance(Vector3Int positionA, Vector3Int positionB)
    {
        return Mathf.Abs(positionA.x - positionB.x) + Mathf.Abs(positionA.z - positionB.z) + Mathf.Abs(positionA.y - positionB.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3Int clickedGridPos = grid.WorldToGridPosition(hit.point);
                Node node = grid.GetNode(clickedGridPos);

                if (goalNode == node)
                {
                    unit.StartFollowingPath();
                }

                if (node != null && node.IsWalkable)
                {
                    Debug.Log("New position found");
                    goalPosition = clickedGridPos;
                    goalNode = node;
                    goalNode.NodeObject.GetComponent<Renderer>().material.color = Color.blue;
                }
            }

            RefreshPath();

            FindPath();
        }    
    }

    public void RefreshPath()
    {
        unit.currentIndex = 0;
        startPosition = grid.WorldToGridPosition(unit.transform.position);
        startNode = grid.GetNode(startPosition);
        startNode.NodeObject.GetComponent<Renderer>().material.color = Color.green;

        for (int i = 0; i < grid.grid.Length; i++)
        {
            if (grid.grid[i].IsWalkable)
            {
                grid.grid[i].NodeObject.GetComponent<Renderer>().material.color = Color.black;
            }
            else
            {
                grid.grid[i].NodeObject.GetComponent<Renderer>().material.color = Color.red;
            }
        }

            currentNode = startNode;
            currentNode.parent = null;
            neighbours.Clear();
            openList.Clear();
            closedList.Clear();
            finalPath.Clear();
            openList.Add(currentNode);
            globalVersion++;
    }

    public void FindPath()
    {
        while (true)
        {            
            openList.Sort();
            currentNode = openList[0];
            openList.Remove(currentNode);
            closedList.Add(currentNode);
            currentNode.NodeObject.GetComponent<Renderer>().material.color = Color.magenta;

            if (currentNode == goalNode)
            {
                print("Path was found");
                GetFinalPath(currentNode);
                finalPath.Reverse();
                for (int i = 0; i < finalPath.Count; i++)
                {
                    finalPath[i].NodeObject.GetComponent<Renderer>().material.color = Color.yellow;
                }
                break;
            }

            neighbours.Clear();

            //-------------------------------Teacher's version-----------------------------

            // Vector3Int leftNodePosition = currentNode.GridPosition + new Vector3Int(-1, 0, 0);
            // if (leftNodePosition.x >= 0)
            // {
            //     Node leftNode = grid.GetNode(leftNodePosition);
            //     neighbours.Add(leftNode);
            // }

            // Vector3Int rightNodePosition = currentNode.GridPosition + new Vector3Int(1, 0, 0);
            // if (rightNodePosition.x < grid.cellCountX)
            // {
            //     Node rightNode = grid.GetNode(rightNodePosition);
            //     neighbours.Add(rightNode);
            // }

            // Vector3Int downNodePosition = currentNode.GridPosition + new Vector3Int(0, 0, -1);
            // if (downNodePosition.z >= 0)
            // {
            //     Node downNode = grid.GetNode(downNodePosition);
            //     neighbours.Add(downNode);
            // }

            // Vector3Int upNodePosition = currentNode.GridPosition + new Vector3Int(0, 0, 1);
            // if (upNodePosition.z < grid.cellCountZ)
            // {
            //     Node upNode = grid.GetNode(upNodePosition);
            //     neighbours.Add(upNode);
            // }
            //---------------------------------Teacher's version---------------------------------

            //My version to find neighbours 
            OpenNeighbours();

            for (int i = 0; i < neighbours.Count; i++)
            {

                if (neighbours[i].version < globalVersion)
                {
                    neighbours[i].version = globalVersion;
                    neighbours[i].HCost = 0;
                    neighbours[i].GCost = 0;
                    neighbours[i].parent = null;
                }
                neighbours[i].NodeObject.GetComponent<Renderer>().material.color = Color.gray;

                if (!neighbours[i].IsWalkable || closedList.Contains(neighbours[i]))
                    continue;

                int newGCost = currentNode.GCost + CalculateDistance(currentNode.GridPosition, neighbours[i].GridPosition);
                if (newGCost < neighbours[i].GCost || !openList.Contains(neighbours[i]))
                {
                    neighbours[i].GCost = newGCost;
                    neighbours[i].HCost = CalculateDistance(neighbours[i].GridPosition, goalNode.GridPosition);
                    neighbours[i].parent = currentNode;
                    if (!openList.Contains(neighbours[i]))
                    {
                        openList.Add(neighbours[i]);
                    }
                }

            }

            if (openList.Count <= 0)
            {
                break;
            }
        }
    }

    void GetFinalPath(Node node)
    {
        if (node == null || finalPath.Contains(node))
        {
            return;
        }

        finalPath.Add(node);

        if (node.parent != null)
        {
            GetFinalPath(node.parent);
        }
    }

    public void OpenNeighbours()
    {
        currentPosition = currentNode.GridPosition;
        Node neighbourUp = grid.NeighbourUp(currentPosition);
        if (neighbourUp != null)
        {
            neighbours.Add(neighbourUp);
        }

        Node neighbourDown = grid.NeighbourDown(currentPosition);
        if (neighbourDown != null)
        {
            neighbours.Add(neighbourDown);
        }

        Node neighbourLeft = grid.NeighbourLeft(currentPosition);
        if (neighbourLeft != null)
        {
            neighbours.Add(neighbourLeft);
        }

        Node neighbourRight = grid.NeighbourRight(currentPosition);
        if (neighbourRight != null)
        {
            neighbours.Add(neighbourRight);
        }
    }
}
