using TMPro;
using UnityEngine;

public class Node
{
    public GameObject NodeObject {get; private set;}
    public Vector3 WorldPosition { get; private set; }
    public Vector3Int GridPosition { get; private set; }
    public Node parent;
    public bool IsWalkable { get; private set; }

    int gCost;
    public int GCost
    {
        get
        {
            return gCost;
        }
        set
        {
            gCost = value;
            NodeObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = gCost.ToString();
            NodeObject.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = FCost.ToString();
        }
    }

    int hCost;
    public int HCost
    {
        get
        {
            return hCost;
        }
        set
        {
            hCost = value;
            NodeObject.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = hCost.ToString();
            NodeObject.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = FCost.ToString();
        }
    }

    public int FCost
    {
        get { return gCost + hCost; }
    }

    public Node(Vector3 worldPosition, Vector3Int gridPosition, bool isWalkable, GameObject nodeObject)
    {
        WorldPosition = worldPosition;
        GridPosition = gridPosition;
        IsWalkable = isWalkable;
        NodeObject = nodeObject;
        NodeObject.GetComponent<Renderer>().material.color = isWalkable ? Color.black : Color.red;
    }
}
