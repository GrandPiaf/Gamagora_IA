using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;


public class AStar
{

    private Grid<PathNode> grid;
    private bool fourNeighbour; // true = 4 / false = 8


    private List<PathNode> openList;
    private List<PathNode> closedList;

    public AStar(int width, int height, bool fourNeighbour = true)
    {
        grid = new Grid<PathNode>(width, height, 10f, Vector3.zero, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
        this.fourNeighbour = fourNeighbour;
    }

    public Grid<PathNode> GetGrid()
    {
        return grid;
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {

        // Start and Finish nodes
        PathNode startNode = GetNode(startX, startY);
        PathNode endNode = GetNode(endX, endY);

        // Initializing lists
        openList = new List<PathNode>();
        closedList = new List<PathNode>();

        // Setting default cost to every node
        for (int x = 0; x < grid.GetWidth(); ++x)
        {
            for (int y = 0; y < grid.GetHeight(); ++y)
            {
                PathNode pathNode = GetNode(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        } 

        // Set starting cost
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        // Add starting node to openList
        openList.Add(startNode);

        /* Run algorithm */
        // Iterate on all nodes marked "to be seen"
        while (openList.Count > 0)
        {
            // Get node with lowest cost in the open list
            PathNode currentNode = GetLowestFCostNode(openList);

            // If current node is the end, we found a path
            if (currentNode == endNode)
            {
                // Calculate correct path
                return CalculatePath(endNode);
            }

            // Moving node from open to closed list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // For each neighbour of the current node, compute their cost and add them to the open list (under conditions)
            foreach(PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode) || !neighbourNode.isWalkable)
                {
                    continue;
                }

                int tempGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if(tempGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tempGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }

            }

        }
        return new List<PathNode>();

    }


    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }


    /* Depends on the neighbourhood type (4 or 8) */
    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {

        List<PathNode> neighbourList = new List<PathNode>();

        // Up
        if (currentNode.y + 1 < grid.GetHeight())   neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));
        // Down
        if (currentNode.y - 1 >= 0)                 neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));

        // Right
        if (currentNode.x + 1 < grid.GetWidth())
        {
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));

            if (!fourNeighbour) //If 8 neighbour type, add diag nodes
            {
                // Up Right
                if (currentNode.y + 1 < grid.GetHeight())   neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
                // Down Right
                if (currentNode.y - 1 >= 0)                 neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            }
        }

        // Left
        if (currentNode.x - 1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));

            if (!fourNeighbour) //If 8 neighbour type, add diag nodes
            {
                // Up Left
                if (currentNode.y + 1 < grid.GetHeight())   neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
                // Down Left
                if (currentNode.y - 1 >= 0)                 neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            }
        }



        return neighbourList;
    }

    /* Depends on the neighbourhood type (4 or 8) */
    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        if (fourNeighbour) // 4 neighbours
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }
        else // 8 neighbours
        {
            return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
        }
    }

    private PathNode GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowerFCostnode = pathNodeList[0];
        for(int i = 1; i < pathNodeList.Count; ++i)
        {
            if(pathNodeList[i].fCost < lowerFCostnode.fCost)
            {
                lowerFCostnode = pathNodeList[i];
            }
        }
        return lowerFCostnode;
    }
    
}
