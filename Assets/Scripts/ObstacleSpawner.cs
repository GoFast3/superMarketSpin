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
    [SerializeField] private GameObject obstaclePrefab;
    [Tooltip("Minimum spawn distance from player")]
    [SerializeField] private float minSpawnDistance = 10f;
    [Tooltip("Maximum spawn distance from player")]
    [SerializeField] private float maxSpawnDistance = 80f;
    [Tooltip("Time between obstacle spawns")]
    [SerializeField] private float spawnInterval = 5f;
    [Tooltip("Height position of obstacles")]
    [SerializeField] private float obstacleHeight = -0.5f;

    [Header("Lane Settings")]
    [Tooltip("Number of available lanes")]
    [SerializeField] private int laneCount = 3;
    [Tooltip("Distance between lanes")]
    [SerializeField] private float laneDistance = 2f;
    [SerializeField] private Transform playerTransform;

    private float lastSpawnTime;
    private TutorialManager tutorialManager;

    private void Awake()
    {
        Debug.Log("ObstacleManager: Awake called");
        // Find the TutorialManager in the scene
        tutorialManager = FindObjectOfType<TutorialManager>();
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("hasObstacles", 1) == 0)
        {
            enabled = false;
        }
        Debug.Log("ObstacleManager: Start called");

        // If playerTransform isn't set in inspector, find it by tag
        if (playerTransform == null)
        {
            Debug.Log("ObstacleManager: Looking for player by tag");
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            if (playerTransform == null)
            {
                Debug.LogError("ObstacleManager: Player not found!");
            }
            else
            {
                Debug.Log($"ObstacleManager: Found player at position {playerTransform.position}");
            }
        }

        if (obstaclePrefab == null)
        {
            Debug.LogError("ObstacleManager: Obstacle prefab is not assigned!");
        }

        minSpawnDistance = PlayerPrefs.GetFloat("minSpawnDistance");
        Debug.Log($"ObstacleManager: Starting game with min distance: {minSpawnDistance}");
        lastSpawnTime = Time.time;
    }

    private void Update()
    {
        // Check if tutorial is active
        if (TutorialManager.IsTutorialActive)
        {
            return; // Don't spawn obstacles during tutorial
        }

        float timeSinceLastSpawn = Time.time - lastSpawnTime;
        if (timeSinceLastSpawn > spawnInterval)
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
        // Check if required components are available
        if (playerTransform == null)
        {
            Debug.LogError("ObstacleManager: playerTransform is null!");
            return;
        }

        if (obstaclePrefab == null)
        {
            Debug.LogError("ObstacleManager: obstaclePrefab is null!");
            return;
        }

        // Calculate spawn position
        float spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
        int randomLane = Random.Range(1, laneCount);
        Vector3 spawnPosition = playerTransform.position +
                              playerTransform.forward * spawnDistance;

        // Set height and lane position
        spawnPosition.y = obstacleHeight;
        spawnPosition.x = (randomLane - 1) * laneDistance;

        GameObject newObstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        Debug.Log($"ObstacleManager: Obstacle spawned: {newObstacle != null}");
    }
}