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
        int obstacleLayerMask = ~(1 << LayerMask.NameToLayer("Node"));

        grid = new Node[cellCountX * cellCountZ];
        for (int z = 0; z < cellCountZ; z++)
        {
            for (int x = 0; x < cellCountX; x++)
            {
                int i = x + z * cellCountX;
                Vector3 worldPosition = new Vector3(x * cellSizeX, 0, z * cellSizeZ);
                Vector3Int gridPosition = new Vector3Int(x, 0, z);
                bool isWalkable = !Physics.CheckBox(new Vector3(x, 0, z), new Vector3(cellSizeX / 2.0f, 1.0f, cellSizeZ / 2.0f), Quaternion.identity, obstacleLayerMask);

                GameObject nodeObject = Instantiate(nodePrefab, gridPosition, Quaternion.identity);
                if (!isWalkable)
                {
                    nodeObject.GetComponent<Renderer>().material.color = Color.red;
                }
                else
                {
                    nodeObject.GetComponent<Renderer>().material.color = Color.black;
                }
                grid[i] = new Node(worldPosition, gridPosition, isWalkable, nodeObject);
            }
        }
    }

    public void UpdateNearbyNodes(Vector3 worldPos, int radius = 1)
    {
        Vector3Int center = WorldToGridPosition(worldPos);

        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dz = -radius; dz <= radius; dz++)
            {
                Vector3Int pos = center + new Vector3Int(dx, 0, dz);

                // Проверка выхода за границы
                if (pos.x < 0 || pos.x >= cellCountX || pos.z < 0 || pos.z >= cellCountZ)
                    continue;

                Node node = GetNode(pos);
                if (node == null) continue;

                Vector3 halfExtents = new Vector3(cellSizeX / 2f, 1f, cellSizeZ / 2f);
                bool walkable = !Physics.CheckBox(node.WorldPosition, halfExtents, Quaternion.identity, ~(1 << LayerMask.NameToLayer("Node")));

                node.SetWalkable(walkable);
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
        if (i < grid.Length)
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
        if (i > -1)
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
        if (i % cellCountX > 0)
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
        if ((i + 1) % cellCountX > 0)
        {
            return grid[i];
        }
        else
        {
            return null;
        }
    }

    public Vector3Int WorldToGridPosition(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / cellSizeX);
        int z = Mathf.RoundToInt(worldPos.z / cellSizeZ);
        return new Vector3Int(x, 0, z);
    }
}
