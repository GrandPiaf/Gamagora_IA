using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarEnemy : MonoBehaviour {

    public PathGrid grid;

    //for debugging, draws the last path calculated
    public bool drawPath;

    public GameObject player;

    Rigidbody2D body;
    public float runSpeed = 2.0f;
    float moveLimiter = 0.7f;

    private List<Node> path;

    //for use with drawPath
    private List<Node> draw = new List<Node>();

    Vector3 oldEnd;
    bool oldAllowDiagonals;

    Node startNode;
    Node endNode;

    Node currentNode = null;
    Node nextNode = null;

    int nextNodeIndex = 0;

    void Start() {
        body = GetComponent<Rigidbody2D>();
    }

    void Update() {
        Vector3 start = transform.position;
        Vector3 end = player.transform.position;

        if (oldEnd != end || grid.allowDiagonals != oldAllowDiagonals) {

            path = FindPath(start, end);
            nextNodeIndex = 1;

            if (path != null) {
                currentNode = path[0];
                nextNode = path[1];
            } else {
                currentNode = null;
                nextNode = null;
            }

            oldEnd = end;
            oldAllowDiagonals = grid.allowDiagonals;
        }

    }

    private void FixedUpdate() {

        // No path, we do not go further
        if(currentNode == null) {
            return;
        }

        if(transform.position == grid.WorldPointFromNode(nextNode) || nextNodeIndex >= path.Count) {
            if(nextNode == endNode) {
                Debug.Log("WE REACH TARGET");
                currentNode = null;
                nextNode = null;
                return;
            } else {
                nextNodeIndex++;
                currentNode = nextNode;
                nextNode = path[nextNodeIndex];
            }
        }

        float horizontal = nextNode.posX - currentNode.posX;
        float vertical = nextNode.posY - currentNode.posY;

        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }

    public List<Node> FindPath(Vector3 sPos, Vector3 ePos) {

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        startNode = grid.NodeFromWorldPoint(sPos);
        endNode = grid.NodeFromWorldPoint(ePos);

        startNode.gCost = 0;
        startNode.hCost = ComputeDistance(startNode, endNode);
        startNode.parent = null;
        openList.Add(startNode);

        while(openList.Count != 0) {

            Node currentNode = NodeWithMinimumCost(openList);

            // We reached the end
            if(currentNode == endNode) {
                return MakePath(startNode, endNode);
            }

            //Moving current node from open to closed list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            List<Node> neighbourList = grid.GetNeighbourNodes(currentNode);

            foreach(Node neighbour in neighbourList) {
                
                // Skipping already seen nodes AND obstacles nodes
                if(closedList.Contains(neighbour) || neighbour.isSolid) {
                    continue;
                }

                // Computing current cost and changing values if needed
                int gCost = currentNode.gCost + ComputeDistance(neighbour, currentNode);
                int hCost = ComputeDistance(neighbour, endNode);

                if (!openList.Contains(neighbour)) {
                    neighbour.gCost = gCost;
                    neighbour.hCost = hCost;
                    neighbour.parent = currentNode;
                    openList.Add(neighbour);

                } else {
                    int fCost = gCost + hCost;
                    if (fCost < neighbour.fCost) {
                        neighbour.gCost = gCost;
                        neighbour.hCost = hCost;
                        neighbour.parent = currentNode;
                    }
                }

            }

        }



        return new List<Node>() ;
    }

    private int ComputeDistance(Node startNode, Node endNode) {
        if (grid.allowDiagonals) {
            return Math.Max( Math.Abs(startNode.posX - endNode.posX), Math.Abs(startNode.posY - endNode.posY) );
        } else {
            return Math.Abs(startNode.posX - endNode.posX) + Math.Abs(startNode.posY - endNode.posY);
        }
    }

    private Node NodeWithMinimumCost(List<Node> openList) {
        Node minimumCostNode = openList[0];

        for (int i = 1; i < openList.Count; ++i) {
            if(openList[i].fCost < minimumCostNode.fCost) {
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