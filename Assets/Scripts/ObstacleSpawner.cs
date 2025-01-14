using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages the spawning and positioning of obstacles
/// </summary>

public class ObstacleManager : MonoBehaviour
{
    [Header("Obstacle Settings")]
    [Tooltip("Obstacle prefab to spawn")]
    [SerializeField]  GameObject obstaclePrefab;

    [Tooltip("Minimum spawn distance from player")]
    [SerializeField]  float minSpawnDistance = 10f;

    [Tooltip("Maximum spawn distance from player")]
    [SerializeField]  float maxSpawnDistance = 80f;

    [Tooltip("Time between obstacle spawns")]
    [SerializeField] float spawnInterval = 5f;

    [Tooltip("Height position of obstacles")]
    [SerializeField]  float obstacleHeight = -0.5f;

    [Header("Lane Settings")]
    [Tooltip("Number of available lanes")]
    [SerializeField]  int laneCount = 3;

    [Tooltip("Distance between lanes")]
    [SerializeField]  float laneDistance = 2f;

    private float lastSpawnTime;
    [SerializeField]  private Transform playerTransform;

    void Start()
    {
        //playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
       // minSpawnDistance = PlayerPrefs.GetFloat("minSpawnDistance");
        Debug.Log($"Starting game with min distance: {minSpawnDistance}");
        lastSpawnTime = Time.time;
    }

    void Update()
    {
        if (TutorialManager.IsTutorialActive) return;

        if (Time.time - lastSpawnTime > spawnInterval)
        {
            lastSpawnTime = Time.time;
            SpawnObstacle();
        }
    }

    /// <summary>
    /// Spawns an obstacle at random lane with calculated distance
    /// </summary>
    private void SpawnObstacle()
    {
        float spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
        Vector3 spawnPosition = playerTransform.position + playerTransform.forward * spawnDistance;
        spawnPosition.y = obstacleHeight;

        int randomLane = Random.Range(0, laneCount);
        spawnPosition.x = randomLane * laneDistance;
        Debug.Log("cart in " + spawnPosition);
        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
    }
}