using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Manages the endless road tile system
/// </summary>
public class TileManager : MonoBehaviour
{
    [Header("Tile Settings")]
    [Tooltip("Road section prefab")]
    [SerializeField] private GameObject tilePrefab;
    [Tooltip("Length of each road section")]
    [SerializeField] private float tileLength = 10f;
    [Tooltip("Number of active road sections")]
    [SerializeField] private int numberOfTiles = 3;
    [Tooltip("Player transform to track position")]
    [SerializeField] private Transform playerTransform;

    private List<GameObject> activeTiles = new List<GameObject>();
    private float zSpawn = 0;

    private void Start()
    {
        InitializeTiles();
    }

    private void Update()
    {
        if (playerTransform.position.z > zSpawn - (numberOfTiles * tileLength))
        {
            RecycleTile();
        }
    }

    /// <summary>
    /// Creates initial road sections
    /// </summary>
    private void InitializeTiles()
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            SpawnTile();
        }
    }

    /// <summary>
    /// Spawns a new road section
    /// </summary>
    private void SpawnTile()
    {
        GameObject tile = Instantiate(tilePrefab, transform);
        tile.transform.position = new Vector3(0, 0, zSpawn);
        activeTiles.Add(tile);
        zSpawn += tileLength;
    }

    /// <summary>
    /// Recycles the first road section to the end
    /// </summary>
    private void RecycleTile()
    {
        GameObject tile = activeTiles[0];
        activeTiles.RemoveAt(0);
        tile.transform.position = new Vector3(0, 0, zSpawn);
        activeTiles.Add(tile);
        zSpawn += tileLength;
    }
}