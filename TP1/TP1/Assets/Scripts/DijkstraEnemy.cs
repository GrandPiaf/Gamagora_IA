using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraEnemy : MonoBehaviour {

    public PathGrid grid;

    //for debugging, draws the last path calculated
    public bool drawPath;

    public GameObject destination;


    private List<Node> path;

    //for use with drawPath
    private List<Node> draw = new List<Node>();

    Vector3 oldEnd;
    bool oldAllowDiagonals;


    // Movement
    float lastMovement = 0;
    public float delay = 2.0f;
    int nextNodeIndex = -1;

    void FixedUpdate() {

        float currentTime = Time.fixedTime;
        // Cannot move until delay is finished
        if (lastMovement + delay >= currentTime) {
            return;
        }
        // Can move
        lastMovement += delay;

        Vector3 start = transform.position;
        Vector3 end = destination.transform.position;

        if (oldEnd != end || grid.allowDiagonals != oldAllowDiagonals) {

            path = FindPath(start, end);

            if (path != null) {
                nextNodeIndex = 0;
            } else {
                nextNodeIndex = -1;
            }

            oldEnd = end;
            oldAllowDiagonals = grid.allowDiagonals;
        }

        // No path, no movement
        if (nextNodeIndex == -1) {
            return;
        }

        // We reached destination
        if (nextNodeIndex == path.Count) {
            Debug.Log("REACHED DESTINATION");
            return;
        }

        Node currentPositionNode = grid.NodeFromWorldPoint(transform.position);

        int horizontal = path[nextNodeIndex].posX - currentPositionNode.posX;
        int vertical = path[nextNodeIndex].posY - currentPositionNode.posY;

        ++nextNodeIndex;

        Vector3 nextPosition = transform.position + new Vector3(horizontal, vertical);
        Node nextNode = grid.NodeFromWorldPoint(nextPosition);

        // We cannot move there, skipping movement
        if (nextNode.isSolid) {
            return;
        }

        // Else, moving
        transform.position = nextPosition;

    }

    public List<Node> FindPath(Vector3 sPos, Vector3 ePos) {

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        Node startNode = grid.NodeFromWorldPoint(sPos);
        Node endNode = grid.NodeFromWorldPoint(ePos);

        startNode.gCost = 0;
        startNode.hCost = ComputeDistance(startNode, endNode);
        startNode.parent = null;
        openList.Add(startNode);

        while (openList.Count != 0) {

            Node currentNode = NodeWithMinimumCost(openList);

            // We reached the end
            if (currentNode == endNode) {
                return MakePath(startNode, endNode);
            }

            //Moving current node from open to closed list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            List<Node> neighbourList = grid.GetNeighbourNodes(currentNode);

            foreach (Node neighbour in neighbourList) {

                // Skipping already seen nodes AND obstacles nodes
                if (closedList.Contains(neighbour) || neighbour.isSolid) {
                    continue;
                }

                // Computing current cost and changing values if needed
                int gCost = currentNode.gCost + ComputeDistance(neighbour, currentNode);

                if (!openList.Contains(neighbour)) {
                    neighbour.gCost = gCost;
                    neighbour.parent = currentNode;
                    openList.Add(neighbour);

                } else {
                    if (gCost < neighbour.gCost) {
                        neighbour.gCost = gCost;
                        neighbour.parent = currentNode;
                    }
                }

            }

        }



        return new List<Node>();
    }

    private int ComputeDistance(Node startNode, Node endNode) {
        if (grid.allowDiagonals) {
            return Math.Max(Math.Abs(startNode.posX - endNode.posX), Math.Abs(startNode.posY - endNode.posY));
        } else {
            return Math.Abs(startNode.posX - endNode.posX) + Math.Abs(startNode.posY - endNode.posY);
        }
    }

    private Node NodeWithMinimumCost(List<Node> openList) {
        Node minimumCostNode = openList[0];

        for (int i = 1; i < openList.Count; ++i) {
            if (openList[i].gCost < minimumCostNode.gCost) {
                minimumCostNode = openList[i];
            }
        }
        return minimumCostNode;
    }

    public List<Node> MakePath(Node start, Node end) {
        List<Node> path = new List<Node>();
        Node current = end;

        //iterate across the elements adding them and their then parents, etc.
        while (current.parent != start) {
            path.Add(current);
            current = current.parent;
        }

        //and flip it so the next node in the path is at [0]
        path.Reverse();

        //if debugging, copy the path into draw
        if (drawPath) {
            draw = new List<Node>(path);
        }

        return path;
    }
    public Vector3 WorldPointFromNode(Node node) {
        //for use by entities using the pathfinding
        return grid.WorldPointFromNode(node);
    }

    private void OnDrawGizmos() {
        //Draw the path in red if debugging
        if (drawPath) {
            Gizmos.color = Color.blue;
            if (draw.Count > 0) {
                foreach (Node n in draw) {
                    Gizmos.DrawWireCube(WorldPointFromNode(n), new Vector3(1, 1, 1) * grid.resolution);
                }
            }
        }
    }
}