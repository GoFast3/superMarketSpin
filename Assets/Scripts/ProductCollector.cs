using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Manages product spawning and score tracking
/// </summary>
namespace SubwaySurfer
{
    public class ProductSpawner : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private GameObject productPrefab;
        [SerializeField] private Transform player;
        [SerializeField] private Text scoreText;

        [Header("Spawn Settings")]
        [SerializeField] private float spawnYPosition = -0.3f;
        [SerializeField] private float minSpawnDistance = 25f;
        [SerializeField] private float maxSpawnDistance = 35f;
        [SerializeField] private float laneErrorMargin = 0.1f;
        [SerializeField] private float[] lanePositions = { 0f, 2f, 4f };

        private GameObject currentProduct;
        private int score = 0;
        private float productSpawnTime;
        private List<float> reactionTimes = new List<float>();

        void Start()
        {
            UpdateScoreUI();
        }

        void Update()
        {
            if (TutorialManager.IsTutorialActive) return;

            if (currentProduct == null || player.position.z > currentProduct.transform.position.z)
            {
                SpawnProduct();
            }

            if (currentProduct != null && IsPlayerOnProductLane() && Input.GetKeyDown(KeyCode.Space))
            {
                CollectProduct();
            }
        }

        /// <summary>
        /// Spawns a new product in a random lane
        /// </summary>
        private void SpawnProduct()
        {
            int randomIndex = Random.Range(0, lanePositions.Length);
            float spawnZ = player.position.z + Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector3 spawnPosition = new Vector3(lanePositions[randomIndex], spawnYPosition, spawnZ);

            currentProduct = Instantiate(productPrefab, spawnPosition, Quaternion.identity);
            productSpawnTime = Time.time;
            Debug.Log($"New product spawned at position: {spawnPosition}");
        }

        /// <summary>
        /// Handles product collection and score update
        /// </summary>
        private void CollectProduct()
        {
            float reactionTime = Time.time - productSpawnTime;
            reactionTimes.Add(reactionTime);
            Debug.Log($"Reaction time: {reactionTime:F2} seconds");
            Debug.Log($"Average reaction time: {GetAverageReactionTime():F2} seconds");

            Destroy(currentProduct);
            currentProduct = null;
            score++;
            UpdateScoreUI();
        }

        private bool IsPlayerOnProductLane()
        {
            if (currentProduct == null) return false;
            return Mathf.Abs(currentProduct.transform.position.x - player.position.x) < laneErrorMargin;
        }

        private void UpdateScoreUI()
        {
            scoreText.text = $"Score: {score}";
        }

        public float GetAverageReactionTime()
        {
            return reactionTimes.Count > 0 ? reactionTimes.Average() : 0f;
        }
    }
}