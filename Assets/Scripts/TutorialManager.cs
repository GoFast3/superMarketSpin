using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Text tutorialText;
    [SerializeField] private Button skipButton;

    public static bool IsTutorialActive { get; private set; } = true;
    private int currentStep = 0;
    private TutorialStep[] tutorialSteps;

    void Awake()
    {
       
        if (tutorialPanel == null) Debug.LogError("Tutorial Panel is not assigned!");
        if (tutorialText == null) Debug.LogError("Tutorial Text is not assigned!");
        if (skipButton == null) Debug.LogError("Skip Button is not assigned!");
    }

    void Start()
    {

        if (PlayerPrefs.GetInt("ShowTutorial") == 0)
        {
            EndTutorial();
            return;
        }
        Debug.Log("TutorialManager Starting...");
        
        InitializeTutorialSteps();

        if (skipButton != null)
        {
            Debug.Log("Setting up skip button...");
            skipButton.onClick.RemoveAllListeners();
           
            Debug.Log("Skip button listener added");
        }
        else
        {
            Debug.LogError("Skip button is null!");
            return;
        }

        ShowCurrentStep();
        Debug.Log("TutorialManager Started Successfully");
    }

   
    

    private void InitializeTutorialSteps()
    {
        tutorialSteps = new TutorialStep[]
        {
            new TutorialStep { message = "Movement Controls:\n← LEFT ARROW to move left\n→ RIGHT ARROW to move right\n↑ UP ARROW to jump\n\nPress 'Skip' to continue" },
            new TutorialStep { message = "Your Goal:\nCollect as many products as possible\nin the shortest time\n\nPress 'Skip' to continue" },
            new TutorialStep { message = "How to Collect Products:\n1. Move to the product's lane\n2. Press SPACEBAR to collect\n\nTip: You can collect from a distance!\n\nPress 'Skip' to continue" },
            new TutorialStep { message = "Ready to Play!\n\nUse arrows to move and jump\nPress SPACEBAR to collect\n\nPress 'Skip' to start" }
        };
        Debug.Log($"Tutorial steps initialized: {tutorialSteps.Length} steps");
    }

    private void ShowCurrentStep()
    {
        if (currentStep >= tutorialSteps.Length)
        {
            Debug.Log("Reached end of tutorial steps");
            EndTutorial();
            return;
        }

        if (tutorialPanel != null && tutorialText != null)
        {
            tutorialPanel.SetActive(true);
            tutorialText.text = tutorialSteps[currentStep].message;
            Debug.Log($"Showing step {currentStep}: {tutorialSteps[currentStep].message}");
        }
        else
        {
            Debug.LogError("Tutorial panel or text is missing!");
        }
    }

    public void SkipStep()
    {
        Debug.Log($"Skipping from step {currentStep} to {currentStep + 1}");
        currentStep++;
        ShowCurrentStep();
    }

    private void EndTutorial()
    {
        IsTutorialActive = false;
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
        if (tutorialText != null)
        {
            tutorialText.text = "";
        }
        if (skipButton != null)
        {
            skipButton.gameObject.SetActive(false);
        }
        Debug.Log("Tutorial ended");
    }

    private class TutorialStep
    {
        public string message;
    }
}