using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraTest : MonoBehaviour
{

    private Dijkstra pathfinding;

    List<PathNode> path;

    private bool fourNeighbours = true;

    private void Start()
    {
        pathfinding = new Dijkstra(10, 10, false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            PathNode node = pathfinding.GetGrid().GetGridObject(mouseWorldPosition);

            node.isWalkable = !node.isWalkable;
            node.color = (node.isWalkable) ? Color.white : Color.red;

            pathfinding.GetGrid().TriggerGridObjectChanged(node.x, node.y, node.color);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);

            path = pathfinding.FindPath(0, 0, x, y);
        }

        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; ++i)
            {
                Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f, Color.green);
            }
        }
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        return worldCamera.ScreenToWorldPoint(screenPosition);
    }

}
