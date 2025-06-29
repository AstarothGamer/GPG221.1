using System;
using TMPro;
using UnityEngine;

public class Node : IComparable
{
    public GameObject NodeObject { get; private set; }
    public Vector3 WorldPosition { get; private set; }
    public Vector3Int GridPosition { get; private set; }
    public Node parent;
    public int version = 0;
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

    public int CompareTo(object obj)
    {
        if (obj == null) return -1;

        Node node = obj as Node;
        if (node == null) return -1;

        int result = FCost.CompareTo(node.FCost);
        if (result == 0)
        {
            result = HCost.CompareTo(node.HCost); 
        }

        return result;
    }
    
    public void SetWalkable(bool state)
    {
        IsWalkable = state;

        if (NodeObject != null)
        {
            var renderer = NodeObject.GetComponent<Renderer>();
            if (renderer != null)
                renderer.material.color = state ? Color.black : Color.red;
        }
    }
}
