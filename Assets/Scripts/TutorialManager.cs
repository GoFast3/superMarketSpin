using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Manages tutorial messages and timing for game introduction
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Text tutorialText;

    [Header("Tutorial Settings")]
    [SerializeField] private float tutorialDuration = 30f;
    [SerializeField] private float messageInterval = 6f;
    [SerializeField]
    private string[] tutorialMessages = new string[]
    {
       "Welcome to the supermarket sprint!",
       "Use the up arrow ↑ to jump",
       "Use the arrows ← → to switch between lanes",
       "To collect products, move to the product lane and press space",
       "Watch out for obstacles!",
       "Good luck!"
    };

    public static bool IsTutorialActive { get; private set; } = true;
    private float startTime;
    private bool hasStarted;

    void Start()
    {
        InitializeTutorial();
    }

    void Update()
    {
        if (!hasStarted) return;
        HandleTutorialProgress();
    }

    private void OnEnable()
    {
        ResetTutorial();
    }

    /// <summary>
    /// Initializes tutorial components and first message
    /// </summary>
    private void InitializeTutorial()
    {
        ResetTutorial();
        if (tutorialText != null)
        {
            tutorialText.text = tutorialMessages[0];
            Debug.Log($"Tutorial started: {tutorialMessages[0]}");
        }
        else
        {
            Debug.LogError("Tutorial Text component is missing!");
        }
    }

    /// <summary>
    /// Handles tutorial message progression and completion
    /// </summary>
    private void HandleTutorialProgress()
    {
        float elapsedTime = Time.time - startTime;
        int messageIndex = (int)(elapsedTime / messageInterval);

        if (messageIndex < tutorialMessages.Length)
        {
            tutorialText.text = tutorialMessages[messageIndex];
        }

        if (elapsedTime >= tutorialDuration)
        {
            EndTutorial();
        }
    }

    /// <summary>
    /// Resets tutorial timer and state
    /// </summary>
    private void ResetTutorial()
    {
        startTime = Time.time;
        hasStarted = true;
    }

    /// <summary>
    /// Ends tutorial and cleans up UI
    /// </summary>
    private void EndTutorial()
    {
        IsTutorialActive = false;
        tutorialText.text = "";
        tutorialPanel.SetActive(false);
        enabled = false;
    }
}