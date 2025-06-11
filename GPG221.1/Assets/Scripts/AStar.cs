using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AStar : MonoBehaviour
{
    [SerializeField] Vector3Int startPosition;
    [SerializeField] Vector3Int goalPosition;
    [SerializeField] Vector3Int currentPosition;

    Node startNode;
    Node goalNode;
    Node currentNode;

    //------techer's way neighbours-------//
    List<Node> neighbours = new List<Node>();
    //------techer's way neighbours-------//
    List<Node> openList = new List<Node>();
    List<Node> closedList = new List<Node>();


    Grid grid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = FindObjectOfType<Grid>();

        startNode = grid.GetNode(startPosition);
        startNode.NodeObject.GetComponent<Renderer>().material.color = Color.green;

        goalNode = grid.GetNode(goalPosition);
        goalNode.NodeObject.GetComponent<Renderer>().material.color = Color.blue;

        currentNode = startNode;
        openList.Add(currentNode);
    }

    int CalculateDistance(Vector3Int positionA, Vector3Int positionB)
    {
        return Mathf.Abs(positionA.x - positionB.x) + Mathf.Abs(positionA.z - positionB.z) + Mathf.Abs(positionA.y - positionB.y);
    }

    // Update is called once per frame
    void Update()
    {
        //-------------------------------------teacher's way neighbours--------------------------------------//
        if (Input.GetKeyDown(KeyCode.L))
        {
            openList.Sort();
            currentNode = openList[0];
            currentNode.NodeObject.GetComponent<Renderer>().material.color = Color.magenta;
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == goalNode)
            {
                print("Path was not found");
                return;
            }

            neighbours.Clear();
            Vector3Int leftNodePosition = currentNode.GridPosition + new Vector3Int(-1, 0, 0);
            if (leftNodePosition.x >= 0)
            {
                Node leftNode = grid.GetNode(leftNodePosition);
                neighbours.Add(leftNode);
            }

            Vector3Int rightNodePosition = currentNode.GridPosition + new Vector3Int(1, 0, 0);
            if (rightNodePosition.x < grid.cellCountX)
            {
                Node rightNode = grid.GetNode(rightNodePosition);
                neighbours.Add(rightNode);
            }

            Vector3Int downNodePosition = currentNode.GridPosition + new Vector3Int(0, 0, -1);
            if (downNodePosition.z >= 0)
            {
                Node downNode = grid.GetNode(downNodePosition);
                neighbours.Add(downNode);
            }

            Vector3Int upNodePosition = currentNode.GridPosition + new Vector3Int(0, 0, 1);
            if (upNodePosition.z < grid.cellCountZ)
            {
                Node upNode = grid.GetNode(upNodePosition);
                neighbours.Add(upNode);
            }

            for (int i = 0; i < neighbours.Count; i++)
            {
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
        }
        //-------------------------------------teacher's way neighbours--------------------------------------//
    }

    // public void OpenNeighbours()
    // {
    //     currentPosition = currentNode.GridPosition;
    //     Node neighbourUp = grid.NeighbourUp(currentPosition);
    //     if (neighbourUp != null)
    //     {
    //         neighbourUp.NodeObject.GetComponent<Renderer>().material.color = Color.grey;
    //     }

    //     Node neighbourDown = grid.NeighbourDown(currentPosition);
    //     if (neighbourDown != null)
    //     {
    //         neighbourDown.NodeObject.GetComponent<Renderer>().material.color = Color.grey;
    //     }

    //     Node neighbourLeft = grid.NeighbourLeft(currentPosition);
    //     if (neighbourLeft != null)
    //     {
    //         neighbourLeft.NodeObject.GetComponent<Renderer>().material.color = Color.grey;
    //     }

    //     Node neighbourRight = grid.NeighbourRight(currentPosition);
    //     if (neighbourRight != null)
    //     {
    //         neighbourRight.NodeObject.GetComponent<Renderer>().material.color = Color.grey;
    //     }
    // }
}
