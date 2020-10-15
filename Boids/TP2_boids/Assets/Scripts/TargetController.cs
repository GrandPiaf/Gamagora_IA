using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    [Range(1, 100)]
    public float speed = 2f;

    public BoidManager manager;

    private Rigidbody rb;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.Z)) {
            movement.z = 1;
        } else if(Input.GetKey(KeyCode.S)){
            movement.z = -1;
        }

        if (Input.GetKey(KeyCode.D)) {
            movement.x = 1;
        } else if (Input.GetKey(KeyCode.Q)) {
            movement.x = -1;
        }

        if (Input.GetKey(KeyCode.LeftShift)) {
            movement.y = 1;
        } else if (Input.GetKey(KeyCode.LeftControl)) {
            movement.y = -1;
        }

        movement.Normalize();

        float borderX = manager.cube.transform.localScale.x / 2 * 0.9f;
        float borderY = manager.cube.transform.localScale.y / 2 * 0.9f;
        float borderZ = manager.cube.transform.localScale.z / 2 * 0.9f;

        if (transform.position.x < -borderX && movement.x < 0) {
            movement.x = 0;
        }
        if (transform.position.y < -borderY && movement.y < 0) {
            movement.y = 0;
        }
        if (transform.position.z < -borderZ && movement.z < 0) {
            movement.z = 0;
        }

        if (transform.position.x > borderX && movement.x > 0) {
            movement.x = 0;
        }
        if (transform.position.y > borderY && movement.y > 0) {
            movement.y = 0;
        }
        if (transform.position.z > borderZ && movement.z > 0) {
            movement.z = 0;
        }

        rb.MovePosition(transform.position + movement * Time.deltaTime * speed);

    }
}
