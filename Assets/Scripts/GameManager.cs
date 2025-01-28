

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
    public Button noObstaclesButton;
    public Button withObstaclesButton;
    public Button startButton;

    [Header("Speed Settings")]
    [SerializeField] float EasySpeed = 1f;
    [SerializeField] float MediumSpeed = 2f;
    [SerializeField] float HardSpeed = 3f;

    private GameObject player;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("ShowTutorial"))
        {
            PlayerPrefs.SetInt("ShowTutorial", 1);
        }
        Debug.Log("whithoutTer menueee " + PlayerPrefs.GetInt("ShowTutorial"));
        easyButton.onClick.AddListener(() => {
            SetDifficulty(EasySpeed);
        });
        mediumButton.onClick.AddListener(() => {
            SetDifficulty(MediumSpeed);
        });
        hardButton.onClick.AddListener(() => {
            SetDifficulty(HardSpeed);
        });

        noObstaclesButton.onClick.AddListener(() => {
            PlayerPrefs.SetInt("hasObstacles", 0);
        });
        withObstaclesButton.onClick.AddListener(() => {
            PlayerPrefs.SetInt("hasObstacles", 1);
        });

        startButton.onClick.AddListener(() => {
            Debug.Log("Starting game");
            StartGame();
        });

 
    }

    public void SetDifficulty(float speed)
    {
        PlayerPrefs.SetFloat("forwardSpeed", speed);
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("isRestarted", 1);
        SceneManager.LoadScene("player");
    }
}