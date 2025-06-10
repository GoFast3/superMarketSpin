using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private Button skipButton;
    [SerializeField] private Image tutorialImage;

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

        InitializeTutorialSteps();

        if (skipButton != null)
        {
            skipButton.onClick.RemoveAllListeners();
            skipButton.onClick.AddListener(NextStep);
        }
        else
        {
            Debug.LogError("Skip button is null!");
            return;
        }

        ShowCurrentStep();
    }

    private void InitializeTutorialSteps()
    {
        tutorialSteps = new TutorialStep[]
        {
            new TutorialStep { message = "ברוכים הבאים למשחק!\nהדרכה קצרה תעזור לך ללמוד איך לשחק.\n\nלחץ 'דלג' כדי להתחיל!" },
            new TutorialStep { message = "תנועה במשחק:\nחץ שמאלה - לזוז שמאלה\nחץ ימינה - לזוז ימינה\nחץ למעלה - לקפוץ\n\nלחץ 'דלג' להמשך" },
            new TutorialStep { message = "המטרה :\nעליך לאסוף 03 מוצרים בזמן המהיר ביותר.\n\nלחץ 'דלג' להמשך" },
            new TutorialStep { message = "איך לאסוף מוצרים?\n1. הזז את השחקן לנתיב שבו נמצא המוצר.\n2. לחץ על מקש הרווח כדי לאסוף.\n\nלחץ 'דלג' להמשך" },
            new TutorialStep { message = "שים לב!\nניתן לאסוף מוצרים גם לפני שהגעת אליהם!\n\nלחץ 'דלג' להמשך" },
            new TutorialStep { message = "מוכנים לשחק!\n\nהשתמשו בחצים לתנועה\nלחצו רווח כדי לאסוף מוצרים\n\nלחצו 'דלג' כדי להתחיל!" }
        };
    }

    private void ShowCurrentStep()
    {
        if (currentStep >= tutorialSteps.Length)
        {
            EndTutorial();
            return;
        }

        if (tutorialPanel != null && tutorialText != null)
        {
            tutorialPanel.SetActive(true);
            tutorialText.text = tutorialSteps[currentStep].message;
        }
        else
        {
            Debug.LogError("Tutorial panel or text is missing!");
        }
    }

    public void NextStep()
    {
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
        // Update PlayerPrefs so tutorial won't show automatically next time
        PlayerPrefs.SetInt("ShowTutorial", 0);

        if (skipButton != null)
        {
            skipButton.gameObject.SetActive(false);
        }
        if (tutorialImage != null)
        {
            tutorialImage.gameObject.SetActive(false);
        }
    }

    private class TutorialStep
    {
        public string message;
    }
}