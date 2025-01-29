using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SubwaySurfer
{
    public class ProductSpawner : MonoBehaviour
    {
        [Header("Game Objects")]
        [Tooltip("Array of different product prefabs that can be spawned")]
        [SerializeField] private GameObject[] productPrefabs;  // Array to hold different product prefabs
        [SerializeField] private Transform player;
        [SerializeField] private Text scoreText;


        [Header("Spawn Settings")]
        [Tooltip("Y position where products will spawn")]
        [SerializeField] private float spawnYPosition = -0.3f;
        [Tooltip("Minimum distance ahead of player to spawn products")]
        [SerializeField] private float minSpawnDistance = 25f;
        [Tooltip("Maximum distance ahead of player to spawn products")]
        [SerializeField] private float maxSpawnDistance = 35f;
        [Tooltip("Margin of error for lane position checking")]
        [SerializeField] private float laneErrorMargin = 0.1f;
        [Tooltip("Available lane positions on X axis")]
        [SerializeField] private float[] lanePositions = { 0f, 2f, 4f };

        [Header("Game End Conditions")]
        [Tooltip("Score required to win the game")]
        [SerializeField] private int scoreToWin = 30;
        [Tooltip("Time limit in seconds (120 = 2 minutes)")]
        [SerializeField] private float gameTimeLimit = 120f;

        // Private variables for game management
        private GameProgress gameProgress;
        private GameObject currentProduct;
        private int score = 0;
        private float productSpawnTime;
        private float gameStartTime;
        private List<float> reactionTimes = new List<float>();
        private PlayerStats playerStats;

        void Start()
        {
            // Validate product prefabs array
            if (productPrefabs == null || productPrefabs.Length == 0)
            {
                Debug.LogError("No product prefabs assigned to spawner!");
                return;
            }

            InitializeGame();
        }

        private void InitializeGame()
        {
            UpdateScoreUI();

            // Find and validate PlayerStats
            playerStats = Object.FindFirstObjectByType<PlayerStats>();
            if (playerStats == null)
            {
                Debug.LogError("PlayerStats not found in scene!");
            }

            // Find and validate GameProgress
            gameProgress = Object.FindFirstObjectByType<GameProgress>();
            if (gameProgress == null)
            {
                Debug.LogError("GameProgress not found in scene!");
            }

            gameStartTime = Time.time;
        }

        void Update()
        {
            if (TutorialManager.IsTutorialActive) return;

            // Check if player passed current product without collecting
            if (currentProduct != null && player.position.z > currentProduct.transform.position.z)
            {
                playerStats.AddMissedProduct();
            }

            // Spawn new product if needed
            if (currentProduct == null || player.position.z > currentProduct.transform.position.z)
            {
                SpawnRandomProduct();
            }

            // Check for product collection
            if (currentProduct != null && IsPlayerOnProductLane() && Input.GetKeyDown(KeyCode.Space))
            {
                CollectProduct();
            }

            CheckGameEndConditions();
        }

        private void SpawnRandomProduct()
        {
            bool validPositionFound = false;
            Vector3 spawnPosition = Vector3.zero;
            int randomProductIndex = Random.Range(0, productPrefabs.Length);

  
                // בחירת מסלול רנדומלי
                int randomLaneIndex = Random.Range(0, lanePositions.Length);
                float spawnZ = player.position.z + Random.Range(minSpawnDistance, maxSpawnDistance);
                spawnPosition = new Vector3(lanePositions[randomLaneIndex], spawnYPosition, spawnZ);


                RaycastHit hit;
            while (Physics.Raycast(spawnPosition, Vector3.back, out hit, 12f) && hit.collider.CompareTag("obstacle"))
               {
                    spawnZ += 12f;
                    spawnPosition.z = spawnZ;

                }
                
                    currentProduct = Instantiate(productPrefabs[randomProductIndex], spawnPosition, Quaternion.identity);
                productSpawnTime = Time.time;

            }
        

        private void CollectProduct()
        {
            // Calculate and store reaction time
            float reactionTime = Time.time - productSpawnTime;
            reactionTimes.Add(reactionTime);

            if (playerStats != null)
            {
                playerStats.AddResponseTime(reactionTime);
            }

            Debug.Log($"Reaction time: {reactionTime:F2} seconds");
            Debug.Log($"Average reaction time: {GetAverageReactionTime():F2} seconds");

            // Clean up and update score
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

        private void CheckGameEndConditions()
        {
            // Check time limit
            if (Time.time - gameStartTime >= gameTimeLimit)
            {
                EndGame("Time's Up!");
                return;
            }

            // Check score goal
            if (score >= scoreToWin)
            {
                EndGame("You Won!");
                return;
            }
        }

        private void EndGame(string reason)
        {
            // Save game results if all components are available
            if (gameProgress != null && playerStats != null)
            {
                Debug.Log($"Game ended: {reason}");
                gameProgress.SaveGameResult(
                    playerStats.GetAverageResponseTime(),
                    playerStats.GetMissedProducts(),
                    playerStats.GetSpeedLevel(),
                    playerStats.GetObstacleLevel()
                );
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene("ResultsScene");
        }
    }
}