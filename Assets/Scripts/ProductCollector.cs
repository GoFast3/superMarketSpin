using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [SerializeField] private float[] lanePositions = { 0f, 3f, 6f };

        [Header("Game End Conditions")]
        [Tooltip("Score required to win the game")]
        [SerializeField] private int scoreToWin = 3;
        [Tooltip("Time limit in seconds (120 = 2 minutes)")]
        [SerializeField] private float gameTimeLimit = 120f;

        [Header("Win Message")]
        [SerializeField] private GameObject winMessagePanel;
        [SerializeField] private Button winMessageCloseButton;

        // Private variables for game management
        private GameProgress gameProgress;
        private GameObject currentProduct;
        private int score = 0;
        private float productSpawnTime;
        private float gameStartTime;
        private List<float> reactionTimes = new List<float>();
        private PlayerStats playerStats;
        private AudioManager audioManager;
        private bool gameWon = false;

        void Start()
        {
            // Validate product prefabs array
            if (productPrefabs == null || productPrefabs.Length == 0)
            {
                Debug.LogError("No product prefabs assigned to spawner!");
                return;
            }
            // Get reference to AudioManager
            audioManager = FindFirstObjectByType<AudioManager>();
            if (audioManager == null)
            {
                Debug.LogWarning("AudioManager not found in scene!");
            }

            // Hide win message panel at start
            if (winMessagePanel != null)
            {
                winMessagePanel.SetActive(false);
            }

            InitializeGame();
        }

        private void InitializeGame()
        {
            UpdateScoreUI();

            playerStats = Object.FindFirstObjectByType<PlayerStats>();
            gameProgress = Object.FindFirstObjectByType<GameProgress>();

            // Find and validate PlayerStats
            if (playerStats == null)
            {
                Debug.LogError("PlayerStats not found in scene!");
            }

            // Find and validate GameProgress
            if (gameProgress == null)
            {
                Debug.LogError("GameProgress not found in scene!");
            }
            int gameMode = PlayerPrefs.GetInt("GameMode", 1);
            if (gameMode == 0)
            {
                lanePositions = new float[] { 0f, 3f };
            }
            else if (gameMode == 1)
            {
                lanePositions = new float[] { 0f, 3f, 6f };
            }
            else
            {
                lanePositions = new float[] { 0f, 3f, 6f };
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

            // Check for product collection with sound
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (currentProduct != null && IsPlayerOnProductLane())
                {
                    CollectProduct();
                    if (audioManager != null)
                    {
                        audioManager.PlayCorrectCollect();
                    }
                }
                else
                {
                    // Wrong collection attempt
                    if (audioManager != null)
                    {
                        audioManager.PlayWrongCollect();
                    }
                }
            }

            CheckGameEndConditions();
        }

        private void SpawnRandomProduct()
        {
            bool validPositionFound = false;
            Vector3 spawnPosition = Vector3.zero;
            int randomProductIndex = Random.Range(0, productPrefabs.Length);

            // Random lane selection
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

            // Check if player won
            if (score >= scoreToWin && !gameWon)
            {
                gameWon = true;
                ShowWinMessage();
            }
        }

        private bool IsPlayerOnProductLane()
        {
            if (currentProduct == null) return false;
            return Mathf.Abs(currentProduct.transform.position.x - player.position.x) < laneErrorMargin;
        }

        private void UpdateScoreUI()
        {
            scoreText.text = $"{score}/{scoreToWin}";
        }

        public float GetAverageReactionTime()
        {
            return reactionTimes.Count > 0 ? reactionTimes.Average() : 0f;
        }

        private void ShowWinMessage()
        {
            if (winMessagePanel != null)
            {
                winMessagePanel.SetActive(true);
                Time.timeScale = 0; // Pause the game

                // Add listener to close button
                if (winMessageCloseButton != null)
                {
                    winMessageCloseButton.onClick.RemoveAllListeners();
                    winMessageCloseButton.onClick.AddListener(CloseWinMessage);
                }
            }
        }

        private void CloseWinMessage()
        {
            Time.timeScale = 1; // Resume normal time
            EndGame("You Won!");
        }

        private void CheckGameEndConditions()
        {
            // Check time limit
            if (Time.time - gameStartTime >= gameTimeLimit)
            {
                EndGame("Time's Up!");
                return;
            }

            // Score goal check is now handled in CollectProduct() - removed from here
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

        public void ExitToMenu()
        {
            PlayerPrefs.SetInt("ShowTutorial", 0);
            SceneManager.LoadScene("MenuScene"); // Loads the main menu scene
        }
    }
}