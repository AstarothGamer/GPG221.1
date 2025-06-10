using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] public int cellCountX;
    [SerializeField] public int cellCountZ;
    [SerializeField] float cellSizeX;
    [SerializeField] float cellSizeZ;
    [SerializeField] GameObject nodePrefab;
    public Node[] grid;

    void Start()
    {
        grid = new Node[cellCountX * cellCountZ];
        for (int z = 0; z < cellCountZ; z++)
        {
            for (int x = 0; x < cellCountX; x++)
            {
                int i = x + z * cellCountX;
                Vector3 worldPosition = new Vector3(x * cellSizeX, 0, z * cellSizeZ);
                Vector3Int gridPosition = new Vector3Int(x, 0, z);
                bool isWalkable = true;

                GameObject nodeObject = Instantiate(nodePrefab, gridPosition, Quaternion.identity);
                grid[i] = new Node(worldPosition, gridPosition, isWalkable, nodeObject);
            }
        }
    }

    public Node GetNode(Vector3Int gridPosition)
    {
        int i = gridPosition.x + gridPosition.z * cellCountX;
        return grid[i];
    }

    public Node NeighbourUp(Vector3Int gridPosition)
    {
        int i = gridPosition.x + gridPosition.z * cellCountX + cellCountX;
        if(i < grid.Length)
        {
            return grid[i];
        }
        else
        {
            return null; 
        }
    }

    public Node NeighbourDown(Vector3Int gridPosition)
    {
        int i = gridPosition.x + gridPosition.z * cellCountX - cellCountX;
        if(i > -1)
        {
            return grid[i];
        }
        else
        {
            return null; 
        }
    }

    public Node NeighbourRight(Vector3Int gridPosition)
    {
        int i = gridPosition.x + gridPosition.z * cellCountX + 1;
        if(i % cellCountX > 0)
        {
            return grid[i];
        }
        else
        {
            return null; 
        }
    }
    
        public Node NeighbourLeft(Vector3Int gridPosition)
    {
        int i = gridPosition.x + gridPosition.z * cellCountX - 1;
        if((i + 1) % cellCountX > 0)
        {
            return grid[i];
        }
        else
        {
            return null; 
        }
    }
}
