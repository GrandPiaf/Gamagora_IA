using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public PathGrid grid;

    public float movementTimer = 0;
    public float movementDelay = 2.0f;


    /** FSM **/
    internal PlayerState currentState = PlayerState.Move;


    void Update() {

        

        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        int vertical = (int)Input.GetAxisRaw("Vertical");

        // if the game doesn't allow diagonal movements, we force horizontol movement
        if (!grid.allowDiagonals && vertical != 0 && horizontal != 0) {
            vertical = 0;
        }

        movementTimer += Time.deltaTime;

        // Cannot move until delay is finished
        if (movementDelay > movementTimer) {
            return;
        }

        // Can move
        movementTimer -= movementDelay;


        Vector3 nextPosition = transform.position + new Vector3(horizontal, vertical);
        Node nextNode = grid.NodeFromWorldPoint(nextPosition);
        
        // We cannot move there, skipping movement
        if (nextNode.isSolid) {
            return;
        }

        // Else, moving
        transform.position = nextPosition;

    }

    void OnTriggerEnter2D(Collider2D collision) {

        GameObject go = collision.gameObject;

        // If it is an enemy
        if (go.GetComponent<AStarEnemy>() != null) 
            switch (currentState) {
                case PlayerState.Move:
                    Debug.Log("PLAYER LOSE");
                    Destroy(gameObject);
                    break;
                case PlayerState.Eat:
                    Debug.Log("ENEMY LOSE");
                    Destroy(go);
                    break;
                default:
                    break;
            }
        }

        //If it is a pastille
        if (go.GetComponent<Pastille>() != null) {
            currentState = PlayerState.Eat;
            Destroy(go);
        }

        // Else, we don't know what type is the collision
        // Do nothing

    }
}
