using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public PathGrid grid;

    float lastMovement = 0;
    public float delay = 2.0f;

    void FixedUpdate() {
        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        int vertical = (int)Input.GetAxisRaw("Vertical");

        float currentTime = Time.fixedTime;

        // Cannot move until delay is finished
        if (lastMovement + delay >= currentTime) {
            return;
        }

        // Can move
        lastMovement += delay;


        Vector3 nextPosition = transform.position + new Vector3(horizontal, vertical);
        Node nextNode = grid.NodeFromWorldPoint(nextPosition);
        
        // We cannot move there, skipping movement
        if (nextNode.isSolid) {
            return;
        }

        // Else, moving
        transform.position = nextPosition;

    }
}
