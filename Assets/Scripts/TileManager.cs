using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    [Header("Tile Settings")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private float tileLength = 97f;
    [SerializeField] private int numberOfTiles = 3;  // שיניתי ל-3      
    [SerializeField] private Transform playerTransform;

    private List<GameObject> activeTiles = new List<GameObject>();
    private float zSpawn = 33f;

    private void Start()
    {
        InitializeTiles();
    }

    private void Update()
    {
        // בודקים את המיקום של ה-tile האמצעי (index 1)
        if (playerTransform.position.z > activeTiles[1].transform.position.z)
        {
            RecycleTile();
        }
    }

    private void InitializeTiles()
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            SpawnTile();
        }
    }

    private void SpawnTile()
    {
        GameObject tile = Instantiate(tilePrefab, transform);
        tile.transform.position = new Vector3(0, 0, zSpawn);
        activeTiles.Add(tile);
        zSpawn += tileLength;
    }

    private void RecycleTile()
    {
        // מעבירים את ה-tile הראשון לסוף
        GameObject movedTile = activeTiles[0];
        activeTiles.RemoveAt(0);
        movedTile.transform.position = new Vector3(0, 0, zSpawn);
        activeTiles.Add(movedTile);
        zSpawn += tileLength;
    }
}