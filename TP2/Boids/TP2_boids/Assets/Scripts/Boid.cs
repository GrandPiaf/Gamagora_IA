using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{

    private BoidManager manager;

    private Vector3 velocity;

    void Start()
    {
        
    }

    
    void Update()
    {

        //Get closests neighbours from distance
        List<Boid> closestBoids = new List<Boid>();

        foreach (Boid other in manager.boids) {
            if (Vector3.Distance(transform.position, other.transform.position) < manager.neighbourDistance) {
                closestBoids.Add(other);
            }
        }

        // Compute flocking
        MoveCloser(closestBoids);
        MoweWith(closestBoids);
        MoveAway(closestBoids, manager.awayDistance);



        // If close to borders, move away
        float borderX = manager.cube.transform.localScale.x / 2 * 0.9f;
        float borderY = manager.cube.transform.localScale.y / 2 * 0.9f;
        float borderZ = manager.cube.transform.localScale.z / 2 * 0.9f;

        if (transform.position.x < -borderX && velocity.x < 0) {
            velocity.x = -velocity.x * UnityEngine.Random.Range(0.1f, 1.0f);
        }
        if (transform.position.y < -borderY && velocity.y < 0) {
            velocity.y = -velocity.y * UnityEngine.Random.Range(0.1f, 1.0f);
        }
        if (transform.position.z < -borderZ && velocity.z < 0) {
            velocity.z = -velocity.z * UnityEngine.Random.Range(0.1f, 1.0f);
        }

        if (transform.position.x > borderX && velocity.x > 0) {
            velocity.x = -velocity.x * UnityEngine.Random.Range(0.1f, 1.0f);
        }
        if (transform.position.y > borderY && velocity.y > 0) {
            velocity.y = -velocity.y * UnityEngine.Random.Range(0.1f, 1.0f);
        }
        if (transform.position.z > borderZ && velocity.z > 0) {
            velocity.z = -velocity.z * UnityEngine.Random.Range(0.1f, 1.0f);
        }

        velocity = velocity.normalized;

        // Apply movement
        transform.position += Time.deltaTime * manager.boidSpeed * velocity;
    }


    private void MoveCloser(List<Boid> closestBoids) {
        if (closestBoids.Count < 1) {
            return;
        }

        Vector3 average = Vector3.zero;
        foreach (Boid other in closestBoids) {
            if (other.transform == this.transform) {
                continue;
            }
            average += transform.position - other.transform.position;
        }

        average /= closestBoids.Count;

        velocity -= (average / 100.0f);
    }


    private void MoweWith(List<Boid> closestBoids) {
        if (closestBoids.Count < 1) {
            return;
        }

        Vector3 average = Vector3.zero;
        foreach (Boid other in closestBoids) {
            average += other.velocity;
        }

        average /= closestBoids.Count;

        velocity += (average / 40.0f);

    }


    private void MoveAway(List<Boid> closestBoids, float awayDistance) {
        if (closestBoids.Count < 1) {
            return;
        }

        Vector3 distanceTot = Vector3.zero;
        int numClose = 0;

        foreach (Boid other in closestBoids) {

            float dist = Vector3.Distance(transform.position, other.transform.position);

            if(dist < awayDistance) {

                ++numClose;

                Vector3 diff = transform.position - other.transform.position;

                if (diff.x >= 0) {
                    diff.x = Mathf.Sqrt(awayDistance) - diff.x;
                }

                if (diff.y >= 0) {
                    diff.y = Mathf.Sqrt(awayDistance) - diff.y;
                }

                if (diff.z >= 0) {
                    diff.z = Mathf.Sqrt(awayDistance) - diff.z;
                }

                distanceTot += diff;

            }

        }

        if(numClose == 0) {
            return;
        }

        velocity -= distanceTot / 5.0f;

    }

    

    internal void SetVelocity(Vector3 vel) {
        velocity = vel;
    }

    internal void SetController(BoidManager boidManager) {
        manager = boidManager;
    }
}
