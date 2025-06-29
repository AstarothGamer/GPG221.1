using UnityEngine;

public class DynamicObstacle : MonoBehaviour
{
    private Grid grid;
    private Vector3Int lastGridPos;

    [SerializeField] private float updateRate = 0.1f; 
    [SerializeField] private int radius = 1;

    void Start()
    {
        grid = FindObjectOfType<Grid>();
        lastGridPos = grid.WorldToGridPosition(transform.position);
        InvokeRepeating(nameof(CheckPositionChange), 0f, updateRate);
    }

    void CheckPositionChange()
    {
        Vector3Int currentGridPos = grid.WorldToGridPosition(transform.position);

        if (currentGridPos != lastGridPos)
        {
            grid.UpdateNearbyNodes(lastGridPos, radius);       
            grid.UpdateNearbyNodes(currentGridPos, radius);    

            lastGridPos = currentGridPos;
        }
    }
}

