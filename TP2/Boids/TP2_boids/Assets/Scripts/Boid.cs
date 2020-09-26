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
            if (Vector3.Distance(transform.position, other.transform.position) < manager.processingDistance) {
                closestBoids.Add(other);
            }
        }

        // If close to borders, move away
        float borderX = manager.cube.transform.localScale.x /2 * 0.9f;
        float borderY = manager.cube.transform.localScale.y /2 * 0.9f;
        float borderZ = manager.cube.transform.localScale.z /2 * 0.9f;

        if (transform.position.x < -borderX && velocity.x < 0) {
            velocity.x = -velocity.x;
        }
        if (transform.position.y < -borderY && velocity.y < 0) {
            velocity.y = -velocity.y;
        }
        if (transform.position.z < -borderZ && velocity.z < 0) {
            velocity.z = -velocity.z;
        }

        if (transform.position.x > borderX && velocity.x > 0) {
            velocity.x = -velocity.x;
        }
        if (transform.position.y > borderY && velocity.y > 0) {
            velocity.y = -velocity.y;
        }
        if (transform.position.z > borderZ && velocity.z > 0) {
            velocity.z = -velocity.z;
        }


        //Else, compute flocking

        //Vector3 cohere = MoveCloser(closestBoids);
        //Vector3 separate = MoveAway(closestBoids);
        //Vector3 align = MoweWith(closestBoids);

        //velocity = cohere * manager.weightCohere + separate * manager.weightSeperate + align * manager.weightAlign;

        // Apply movement
        transform.position += Time.deltaTime * manager.boidSpeed * velocity;
    }

    private Vector3 MoweWith(List<Boid> closestBoids) {
        throw new NotImplementedException();
    }

    private Vector3 MoveAway(List<Boid> closestBoids) {
        throw new NotImplementedException();
    }

    private Vector3 MoveCloser(List<Boid> closestBoids) {
        throw new NotImplementedException();
    }

    internal void SetVelocity(Vector3 vel) {
        velocity = vel;
    }

    internal void SetController(BoidManager boidManager) {
        manager = boidManager;
    }
}
