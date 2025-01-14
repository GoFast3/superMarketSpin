

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// Manages the menu interface and game initialization settings
/// </summary>
public class MenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject playerPrefab;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public Button shortDistanceButton;
    public Button mediumDistanceButton;
    public Button longDistanceButton;
    public Button startButton;

    [Header("Speed Settings")]
    [Tooltip("Speed for easy difficulty")]
    [SerializeField] float EasySpeed = 5f;
    [Tooltip("Speed for medium difficulty")]
    [SerializeField] float MediumSpeed = 8f;
    [Tooltip("Speed for hard difficulty")]
    [SerializeField] float HardSpeed = 12f;

    [Header("Distance Settings")]
    [Tooltip("Minimum spawn distance for short range")]
    [SerializeField] private float shortDistance = 10f;
    [Tooltip("Minimum spawn distance for medium range")]
    [SerializeField] private float mediumDistance = 20f;
    [Tooltip("Minimum spawn distance for long range")]
    [SerializeField] private float longDistance = 30f;

    private GameObject player;

    private void Start()
    {
        // Initialize button listeners
        easyButton.onClick.AddListener(() => {
            Debug.Log("Selected easy difficulty");
            SetDifficulty(EasySpeed);
        });

        mediumButton.onClick.AddListener(() => {
            Debug.Log("Selected medium difficulty");
            SetDifficulty(MediumSpeed);
        });

        hardButton.onClick.AddListener(() => {
            Debug.Log("Selected hard difficulty");
            SetDifficulty(HardSpeed);
        });

        startButton.onClick.AddListener(() => {
            Debug.Log("Starting game");
            StartGame();
        });

        // Setup distance button listeners
        shortDistanceButton.onClick.AddListener(() => SetSpawnDistance(shortDistance));
        mediumDistanceButton.onClick.AddListener(() => SetSpawnDistance(mediumDistance));
        longDistanceButton.onClick.AddListener(() => SetSpawnDistance(longDistance));

        // Initialize default values
        PlayerPrefs.SetFloat("minSpawnDistance", 0);
        PlayerPrefs.SetFloat("forwardSpeed", 0);

        // Instantiate player
        player = Instantiate(playerPrefab);
    }

    /// <summary>
    /// Sets game difficulty by adjusting player speed
    /// </summary>
    public void SetDifficulty(float speed)
    {
        PlayerPrefs.SetFloat("forwardSpeed", speed);
        Debug.Log($"Game speed set to: {speed}");
    }

    /// <summary>
    /// Sets minimum spawn distance for obstacles
    /// </summary>
    public void SetSpawnDistance(float distance)
    {
        PlayerPrefs.SetFloat("minSpawnDistance", distance);
        Debug.Log($"Spawn distance set to: {distance}");
    }

    /// <summary>
    /// Loads the main game scene
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
