using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;  // פריפאב של המטבע
    public float initialSpawnDistance = 20f;  // אורך המסלול שבו ניצור מטבעות בהתחלה
    public float spawnZDistance = 10f;  // המרחק שממנו יתחילו המטבעות החדשים
    public float distanceBetweenCoins = 5f; // המרחק בין כל מטבע
    public int laneCount = 3; // מספר הנתיבים
    private Transform playerTransform;  // מיקום השחקן
    private float lastSpawnZ = 0f;  // המיקום האחרון שבו הופיע מטבע

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // יצירת מטבעות ראשוניים
        GenerateInitialCoins();
    }

    void Update()
    {
        // אם השחקן מתקרב לגבול שבו המטבע האחרון היה, נוסיף מטבעות קדימה
        if (playerTransform.position.z > lastSpawnZ - spawnZDistance)
        {
            SpawnCoins();
        }
    }

    void GenerateInitialCoins()
    {
        // יצירת מטבעות לאורך מסלול של initialSpawnDistance
        float spawnZ = 0f;
        while (spawnZ < initialSpawnDistance)
        {
            for (int i = 0; i < laneCount; i++)
            {
                // סיכוי רנדומלי ליצור מטבע בכל נתיב
                if (Random.value > 0.8f)  // סיכוי של 20% להופעת מטבע
                {
                    Vector3 spawnPosition = new Vector3(i * 2, -0.3f, spawnZ);  // מיקום המטבע
                    Instantiate(coinPrefab, spawnPosition, Quaternion.identity);  // יצירת המטבע
                }
            }
            spawnZ += distanceBetweenCoins;  // עדכון המיקום הבא
        }

        lastSpawnZ = spawnZ;  // עדכון המיקום האחרון
    }

    void SpawnCoins()
    {
        // יצירת מטבעות חדשים כשמתקרבים לגבול
        float spawnZ = lastSpawnZ + distanceBetweenCoins;

        for (int i = 0; i < laneCount; i++)
        {
            // סיכוי רנדומלי ליצור מטבע בכל נתיב
            if (Random.value > 0.8f)  // סיכוי של 20% להופעת מטבע
            {
                Vector3 spawnPosition = new Vector3(i * 2, -0.3f, spawnZ);  // מיקום המטבע
                Instantiate(coinPrefab, spawnPosition, Quaternion.identity);  // יצירת המטבע
            }
        }

        lastSpawnZ = spawnZ;  // עדכון המיקום האחרון
    }
}
