using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    [Header("Tile Settings")]
    [SerializeField] private GameObject twoLaneTilePrefab; // Prefab for the two-lane road
    [SerializeField] private GameObject threeLaneTilePrefab; // Prefab for the three-lane road
    [SerializeField] private float tileLength = 97f; // Length of each tile
    [SerializeField] private int numberOfTiles = 3; // Number of tiles to be spawned at the start
    [SerializeField] private Transform playerTransform; // Reference to the player to track movement
    [SerializeField] private int gameMode; // Game mode: 0 for two-lane, 1 or 2 for three-lane

    private List<GameObject> activeTiles = new List<GameObject>(); // List to store active tiles
    private float zSpawn = 33f; // Initial spawn position along the Z-axis

    private void Start()
    {
        gameMode = PlayerPrefs.GetInt("GameMode", 1);

        InitializeTiles(); // Spawn initial tiles
    }

    private void Update()
    {
        // Check if the player has passed the middle tile (index 1)
        if (playerTransform.position.z > activeTiles[1].transform.position.z)
        {
            RecycleTile(); // Move the oldest tile to the end to create an endless road
        }
    }

    // Spawns the initial set of tiles
    private void InitializeTiles()
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            SpawnTile();
        }
    }

    // Instantiates a new tile based on the game mode
    private void SpawnTile()
    {
        GameObject tilePrefab;

        // Select the correct tile type based on the game mode
        if (gameMode == 0)
        {
            tilePrefab = twoLaneTilePrefab; // Use two-lane road
        }
        else if (gameMode == 1 || gameMode == 2)
        {
            tilePrefab = threeLaneTilePrefab; // Use three-lane road
        }
        else
        {
            Debug.LogWarning("Invalid game mode! Defaulting to two-lane road.");
            tilePrefab = twoLaneTilePrefab;
        }

        GameObject tile = Instantiate(tilePrefab, transform); // Create a new tile instance
        tile.transform.position = new Vector3(0, 0, zSpawn); // Set tile position along the Z-axis
        activeTiles.Add(tile); // Add tile to the active list
        zSpawn += tileLength; // Move the spawn position forward
    }

    // Recycles the first tile by moving it to the end instead of destroying and re-instantiating
    private void RecycleTile()
    {
        GameObject movedTile = activeTiles[0]; // Get the first tile in the list
        activeTiles.RemoveAt(0); // Remove it from the list
        Destroy(movedTile); // Destroy the old tile

        SpawnTile(); // Spawn a new tile at the end
    }
}
