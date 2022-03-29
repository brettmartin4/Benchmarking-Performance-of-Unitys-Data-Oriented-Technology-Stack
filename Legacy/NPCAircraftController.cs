using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class NPCAircraftController : MonoBehaviour
{

    float speed, radian, radius, height, centerX, centerZ;
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        speed = Random.Range(0.1f, 0.5f);           // Aircraft speed
        radian = 0.0f;                              // Radian, or point along flight path
        radius = Random.Range(10.0f, 100.0f);       // Flight path radius
        height = Random.Range(10.0f, 100.0f);       // Cruising altitude
        centerX = Random.Range(-2300.0f, 2300.0f);  // Aircraft orbit center X coordinate
        centerZ = Random.Range(-2300.0f, 2300.0f);  // Aircraft orbit center Z coordinate
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        float elapsedTime = Time.time - startTime;
        radian = speed * elapsedTime;
        Vector3 newPos = new Vector3(centerX + radius * Mathf.Cos(radian), height, centerZ + radius * Mathf.Sin(radian));
        transform.position = newPos;
        transform.rotation = math.mul(transform.rotation, quaternion.RotateZ(-speed * deltaTime)); ;
    }
}
