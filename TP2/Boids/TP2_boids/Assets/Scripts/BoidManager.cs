using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [Range(10, 100)]
    public int boidNumber;

    [Range(1, 100)]
    public float neighbourDistance = 20.0f;

    [Range(1, 100)]
    public float awayDistance = 5.0f;

    [Range(1, 100)]
    public float boidSpeed = 2.0f;




    public Boid boidPrefab;

    public GameObject cube;

    // Boid list
    internal List<Boid> boids;

    void Start()
    {
        CreateBoids();
    }

    private void CreateBoids() {
        boids = new List<Boid>(boidNumber);

        // Range going from (-x * 0.9) to (x * 0.9)
        // To avoid putting boids too close to the 
        float min = -cube.transform.localScale.x / 2 * 0.9f;
        float max = cube.transform.localScale.x / 2 * 0.9f;

        float x, y, z;
        float velocityX, velocityY, velocityZ;

        for (int i = 0; i < boidNumber; ++i) {

            //Set position
            x = Random.Range(min, max);
            y = Random.Range(min, max);
            z = Random.Range(min, max);

            boids.Add(Instantiate(boidPrefab, new Vector3(x, y, z), Quaternion.identity));

            //Set velocity
            velocityX = Random.Range(1, 10) / 10.0f;
            velocityY = Random.Range(1, 10) / 10.0f;
            velocityZ = Random.Range(1, 10) / 10.0f;

            boids[i].SetVelocity(new Vector3(velocityX, velocityY, velocityZ).normalized);
            boids[i].SetController(this);
        }
    }



    void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

}


/* THINGS TO DO
 * Make them move in a random direction
 * Avoid borders
 * Then seprate, cohere, align ...
 * vectoriel
 */