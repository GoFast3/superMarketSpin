using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the spawning and positioning of obstacles
/// </summary>
public class ObstacleManager : MonoBehaviour
{
    private float lastSpawnDistance;
    private float spawnDistanceThreshold = 30f;
    private float spawnDistanceReductionRate = 0.1f; 

    [Header("Obstacle Settings")]
    [Tooltip("Obstacle prefab to spawn")]
    [SerializeField] private GameObject obstaclePrefab;
    [Tooltip("Height position of obstacles")]
    [SerializeField] private float obstacleHeight = -0.5f;

    [Header("Lane Settings")]
    [Tooltip("Number of available lanes")]
    [SerializeField] private int laneCount = 3;
    [Tooltip("Distance between lanes")]
    [SerializeField] private float laneDistance = 2f;
    [SerializeField] private Transform playerTransform;

    private TutorialManager tutorialManager;

    private void Awake()
    {
        Debug.Log("ObstacleManager: Awake called");
        tutorialManager = FindObjectOfType<TutorialManager>();
    }

    private void Start()
    {
        if (TutorialManager.IsTutorialActive)
        {
            return;
        }
        if (PlayerPrefs.GetInt("hasObstacles", 1) == 0)
        {
            enabled = false;
        }
        Debug.Log("ObstacleManager: Start called");

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

        lastSpawnDistance = playerTransform.position.z;
        SpawnObstacle();  // spawn the first obstacle at the beginning
    }

    private void Update()
    {
        

        // check if the player has moved enough distance to spawn a new obstacle
        float playerDistanceTravelled = playerTransform.position.z;

        if (playerDistanceTravelled - lastSpawnDistance >= spawnDistanceThreshold)
        {
            lastSpawnDistance = playerDistanceTravelled;  // Update the last spawn distance
            SpawnObstacle();  // Create a new obstacle

            // Reduce the spawn distance threshold for the next obstacle
            spawnDistanceThreshold = Mathf.Max(1f, spawnDistanceThreshold - spawnDistanceReductionRate);
        }
    }

    /// <summary>
    /// Spawns an obstacle at a random lane with calculated distance
    /// </summary>
    private void SpawnObstacle()
    {
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
        float spawnDistance = Random.Range(30f, 60f);
        int randomLane = Random.Range(0, laneCount);
        Debug.Log("randomLand " + randomLane);
        Vector3 spawnPosition = playerTransform.position + playerTransform.forward * spawnDistance;
        spawnPosition.y = obstacleHeight;
        spawnPosition.x = randomLane * laneDistance;
        Debug.Log("spawnPosition.x " + spawnPosition.x);

        RaycastHit hit;
        if (!Physics.Raycast(spawnPosition, Vector3.forward, out hit, 12f) || !hit.collider.CompareTag("product"))
        {
            GameObject newObstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
            Debug.Log($"ObstacleManager: Obstacle spawned at distance {spawnDistance} and lane {randomLane}");

        }

    }
}

