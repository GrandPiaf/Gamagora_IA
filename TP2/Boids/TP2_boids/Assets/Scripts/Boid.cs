using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Boid : MonoBehaviour
{

    private BoidManager manager;

    private Vector3 velocity;

    void Start()
    {
        
    }


    void Update() {


        //Get closests neighbours for 
        List<Boid> attractionBoids = GetNeighbours(manager.boids, manager.attractionDistance);
        Vector3 attraction = GetAttraction(attractionBoids);

        List<Boid> alignemntBoids = GetNeighbours(attractionBoids, manager.aligmentDistance);
        Vector3 align = GetAligment(alignemntBoids);

        List<Boid> repulstionBoids = GetNeighbours(alignemntBoids, manager.repulsionDistance);
        Vector3 repulsion = GetRepulsion(repulstionBoids);

        velocity = ((attraction + align + repulsion) / 3);


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

    // Compute the normalized vector from current position to average position of neighbours
    private Vector3 GetAttraction(List<Boid> attractionBoids) {

        // If no neighbours, return current velocity
        if (attractionBoids.Count == 0) {
            return velocity;
        }

        //Go towards the center of neighbours
        Vector3 centerPosition = Vector3.zero;

        foreach (Boid boid in attractionBoids) {
            centerPosition += boid.transform.position;
        }

        centerPosition /= attractionBoids.Count;

        // Returning the normalized vector from current position to centerPosition
        return (centerPosition - transform.position).normalized;

    }

    //Compute 
    private Vector3 GetAligment(List<Boid> alignemntBoids) {
        if (alignemntBoids.Count == 0) {
            return velocity;
        }

        Vector3 averageDirection = Vector3.zero;

        foreach (Boid boid in alignemntBoids) {
            averageDirection += boid.velocity;
        }

        averageDirection /= alignemntBoids.Count;

        return averageDirection.normalized;
    }

    //Compute the normalized vector going away from the average position of neighbours
    private Vector3 GetRepulsion(List<Boid> repulstionBoids) {
        if (repulstionBoids.Count == 0) {
            return velocity;
        }

        Vector3 centerPosition = Vector3.zero;

        foreach (Boid boid in repulstionBoids) {
            centerPosition += boid.transform.position;
        }

        centerPosition /= repulstionBoids.Count;

        // Returning the normalized vector form centerPosition to current position
        // Meaning we return a vector going in the other direction from the center position of neighbours

        return (transform.position - centerPosition).normalized;

    }

    private List<Boid> GetNeighbours(List<Boid> boidList, float distance) {
        List<Boid> closestBoids = new List<Boid>();

        foreach (Boid boid in manager.boids) {

            //Not taking itself
            if(boid == this) {
                continue;
            }

            if (Vector3.Distance(transform.position, boid.transform.position) <= distance) {
                closestBoids.Add(boid);
            }
        }

        return closestBoids;
    }


    internal void SetVelocity(Vector3 vel) {
        velocity = vel;
    }

    internal void SetController(BoidManager boidManager) {
        manager = boidManager;
    }
}
