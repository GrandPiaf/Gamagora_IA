using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public PathGrid grid;

    //for debugging, draws the last path calculated
    public bool drawPath;

    // Destination // Currently it's the player position
    public Player player;


    private List<Node> path;

    //for use with drawPath
    private List<Node> draw = new List<Node>();

    Vector3 oldEnd;
    bool oldAllowDiagonals;

    // Movement
    public float movementTimer = 0;
    public float movementDelay = 2.0f;

    int nextNodeIndex = -1;


    // STATE
    public float seeDistance;
    public float loosingDistance;
    public EnemyState currentState = EnemyState.Roam;



    void Update() {

        movementTimer += Time.deltaTime;

        // Cannot move until delay is finished
        if (movementDelay > movementTimer) {
            return;
        }

        // Can move
        movementTimer -= movementDelay;


        switch (currentState) {

            case EnemyState.Roam:
                roam();
                break;

            case EnemyState.Chase:
                chase();
                break;

            case EnemyState.Evade:
                evade();
                break;
        }

    }

    private void roam() {
        //Random movement around
        int horizontal = UnityEngine.Random.Range(0, 2) * 2 - 1;
        int vertical = UnityEngine.Random.Range(0, 2) * 2 - 1;

        if (!grid.allowDiagonals && horizontal != 0 && vertical != 0) {
            horizontal = 0;
        }

        Vector3 nextPosition = transform.position + new Vector3(horizontal, vertical);
        Node nextNode = grid.NodeFromWorldPoint(nextPosition);

        // We cannot move there, skipping movement
        if (nextNode.isSolid) {
            return;
        }

        // Else, moving
        transform.position = nextPosition;

        if (Vector3.Distance(transform.position, player.transform.position) <= seeDistance) {
            currentState = EnemyState.Chase;
        }
        if (player.currentState == PlayerState.Eat) {
            currentState = EnemyState.Evade;
        }
    }

    private void chase() {
        Vector3 start = transform.position;
        Vector3 end = player.transform.position;

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

        if (Vector3.Distance(transform.position, player.transform.position) > loosingDistance) {
            currentState = EnemyState.Roam;
        }
        if (player.currentState == PlayerState.Eat) {
            currentState = EnemyState.Evade;
        }
    }

    private void evade() {

        Vector3 playerToEnemy = transform.position - player.transform.position;
        playerToEnemy.Normalize();

        int horizontal = 0;
        int vertical = 0;

        if (playerToEnemy.x >= 0.5f) { //Moving horizontally
            horizontal = 1;
        } else if(playerToEnemy.x <= -0.5f) {
            horizontal = -1;
        }

        if (playerToEnemy.y >= 0.5f) { //Moving vertically
            vertical = 1;
        } else if (playerToEnemy.y <= -0.5f) {
            vertical = -1;
        }

        if (!grid.allowDiagonals && horizontal != 0 && vertical != 0) {
            horizontal = 0;
        }

        Vector3 nextPosition = transform.position + new Vector3(horizontal, vertical);
        Node nextNode = grid.NodeFromWorldPoint(nextPosition);

        // We cannot move there, skipping movement
        if (nextNode.isSolid) {
            return;
        }

        // Else, moving
        transform.position = nextPosition;

        if (player.currentState == PlayerState.Move) {
            currentState = EnemyState.Roam;
        }
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
        while (current != start) {
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