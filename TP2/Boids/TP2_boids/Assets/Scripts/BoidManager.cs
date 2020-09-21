using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [Range(10, 100)]
    public int boidNumber;

    public GameObject boidPrefab;

    public GameObject cube;

    // Boid list
    List<Boid> boids;

    void Start()
    {
        boids = new List<Boid>(boidNumber);

        for (int i = 0; i < boidNumber; ++i) {

            float x = 0;
            float y = 0;
            float z = 0;
            
            Instantiate(boidPrefab, new Vector3(x, y, z), Quaternion.identity);
        }
    }
}
