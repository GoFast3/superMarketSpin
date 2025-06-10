using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject playerPrefab;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public Button oneLAandWhithOutObs;
    public Button twoLAandWhithOutObs;
    public Button threeLanesWithObstaclesButton;
    public Button startButton;
    public Button tutorialButton; // Added tutorial button

    [Header("Speed Settings")]
    [SerializeField] float EasySpeed;
    [SerializeField] float MediumSpeed;
    [SerializeField] float HardSpeed;

    // Store the original colors of speed buttons
    private Color easyColor;
    private Color mediumColor;
    private Color hardColor;

    // Track currently selected buttons
    private Button selectedSpeedButton;
    private Button selectedGameModeButton;

    // Color for selected game mode buttons
    private Color selectedGameModeColor = Color.gray;

    private void Start()
    {
        // Store original button colors
        easyColor = easyButton.image.color;
        mediumColor = mediumButton.image.color;
        hardColor = hardButton.image.color;

        if (!PlayerPrefs.HasKey("ShowTutorial"))
        {
            PlayerPrefs.SetInt("ShowTutorial", 1);
        }

        SetupSpeedButtons();
        SetupGameModeButtons();

        startButton.onClick.AddListener(() =>
        {
            Debug.Log("Starting game");
            StartGame();
        });

        // Add functionality to tutorial button
        if (tutorialButton != null)
        {
            tutorialButton.onClick.AddListener(() =>
            {
                ShowTutorial();
            });
        }
    }

    private void SetupSpeedButtons()
    {
        easyButton.onClick.AddListener(() =>
        {
            SetDifficulty(EasySpeed);
            UpdateSpeedButtonVisuals(easyButton);
        });

        mediumButton.onClick.AddListener(() =>
        {
            SetDifficulty(MediumSpeed);
            UpdateSpeedButtonVisuals(mediumButton);
        });

        hardButton.onClick.AddListener(() =>
        {
            SetDifficulty(HardSpeed);
            UpdateSpeedButtonVisuals(hardButton);
        });
    }

    private void SetupGameModeButtons()
    {
        oneLAandWhithOutObs.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("GameMode", 0);
            UpdateGameModeButtonVisuals(oneLAandWhithOutObs);
        });

        twoLAandWhithOutObs.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("GameMode", 1);
            UpdateGameModeButtonVisuals(twoLAandWhithOutObs);
        });

        threeLanesWithObstaclesButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("GameMode", 2);
            UpdateGameModeButtonVisuals(threeLanesWithObstaclesButton);
        });
    }

    // Handle speed button selection while maintaining original colors
    private void UpdateSpeedButtonVisuals(Button clickedButton)
    {
        // Reset previously selected button to its original color
        if (selectedSpeedButton != null)
        {
            if (selectedSpeedButton == easyButton)
                selectedSpeedButton.image.color = easyColor;
            else if (selectedSpeedButton == mediumButton)
                selectedSpeedButton.image.color = mediumColor;
            else if (selectedSpeedButton == hardButton)
                selectedSpeedButton.image.color = hardColor;
        }

        // Update the newly selected button
        selectedSpeedButton = clickedButton;

        // Store the original color but make it slightly darker to show selection
        Color originalColor = selectedSpeedButton.image.color;
        selectedSpeedButton.image.color = new Color(
            originalColor.r * 0.8f,
            originalColor.g * 0.8f,
            originalColor.b * 0.8f,
            originalColor.a
        );
    }

    // Handle game mode button selection
    private void UpdateGameModeButtonVisuals(Button clickedButton)
    {
        if (selectedGameModeButton != null)
        {
            selectedGameModeButton.image.color = Color.white;
        }

        selectedGameModeButton = clickedButton;
        selectedGameModeButton.image.color = selectedGameModeColor;
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

    // New function to show tutorial
    public void ShowTutorial()
    {
        // Enable tutorial
        PlayerPrefs.SetInt("ShowTutorial", 1);
        PlayerPrefs.SetInt("isRestarted", 1);

        // Load game scene to show tutorial
        SceneManager.LoadScene("player");
    }
}