using UnityEngine;

public class AStar : MonoBehaviour
{
    [SerializeField] Vector3Int startPosition;
    [SerializeField] Vector3Int goalPosition;
    [SerializeField] Vector3Int currentPosition;

    Node startNode;
    Node goalNode;
    Node currentNode;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            OpenNeighbours();
        }
    }

    public void OpenNeighbours()
    {
        currentPosition = currentNode.GridPosition;
        Node neighbourUp = grid.NeighbourUp(currentPosition);
        if (neighbourUp != null)
        {
            neighbourUp.NodeObject.GetComponent<Renderer>().material.color = Color.grey;
        }

        Node neighbourDown = grid.NeighbourDown(currentPosition);
        if (neighbourDown != null)
        {
            neighbourDown.NodeObject.GetComponent<Renderer>().material.color = Color.grey;
        }

        Node neighbourLeft = grid.NeighbourLeft(currentPosition);
        if (neighbourLeft != null)
        {
            neighbourLeft.NodeObject.GetComponent<Renderer>().material.color = Color.grey;
        }
        
        Node neighbourRight = grid.NeighbourRight(currentPosition);
        if (neighbourRight != null)
        {
            neighbourRight.NodeObject.GetComponent<Renderer>().material.color = Color.grey;
        }
    }
}
