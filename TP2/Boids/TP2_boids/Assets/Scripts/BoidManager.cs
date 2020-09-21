using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [Range(10, 100)]
    public int boidNumber;

    public Boid boidPrefab;

    public GameObject cube;

    // Boid list
    List<Boid> boids;

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

        float x, y, z, rotX, rotY, rotZ;

        for (int i = 0; i < boidNumber; ++i) {

            x = Random.Range(min, max);
            y = Random.Range(min, max);
            z = Random.Range(min, max);

            rotX = Random.Range(0, 360);
            rotY = Random.Range(0, 360);
            rotZ = Random.Range(0, 360);

            boids.Add(Instantiate(boidPrefab, new Vector3(x, y, z), Quaternion.Euler(new Vector3(rotX, rotY, rotZ))));
        }
    }

}
