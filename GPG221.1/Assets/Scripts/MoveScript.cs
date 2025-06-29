using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public AStar aStar; 
    public float moveSpeed = 2f;

    private List<Node> path;
    public int currentIndex = 0;
    private bool isFollowing = false;

    void Update()
    {
        if (isFollowing && path != null && currentIndex < path.Count)
        {
            if (!aStar.finalPath[currentIndex].IsWalkable)
            {
                aStar.RefreshPath();
                aStar.FindPath();
                return;
            }

            Vector3 targetPos = path[currentIndex].WorldPosition;
            Vector3 direction = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            transform.position = direction;

            float step = moveSpeed * Time.deltaTime;

            if ((float)step >= Vector3.Distance(transform.position, targetPos))
            {
                transform.position = targetPos;
                currentIndex++;
            }

            if (currentIndex >= path.Count)
            {
                currentIndex = 0;
                isFollowing = false;
            }
        }
    }

    public void StartFollowingPath()
    {
        path = aStar.finalPath;
        if (path == null || path.Count == 0)
            return;

        currentIndex = 0;
        isFollowing = true;
    }
}
