using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultsDisplay : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private Button backButton;
    private GameProgress gameProgress;

    // Convert speed level to Hebrew text
    private string GetDifficultyText(float level)
    {
        return level switch
        {
            0.2f => "קל",
            0.3f => "בינוני",
            0.4f => "קשה",
            _ => "לא ידוע"
        };
    }

    // Convert obstacle level to Hebrew text
    private string GetObstacleText(float level)
    {
        return level switch
        {
            0 => "בלי מכשולים",
            1 => "עם מכשולים",
            _ => "לא ידוע"
        };
    }

    void Start()
    {
        gameProgress = FindObjectOfType<GameProgress>();
        if (gameProgress == null)
        {
            Debug.LogError("GameProgress not found!");
            return;
        }
        PlayerPrefs.SetInt("ShowTutorial", 1);
        DisplayResults();
        SetupButton();
    }

    void DisplayResults()
    {
        var results = gameProgress.GetHistory();
        Debug.Log($"Found {results.Count} results to display");
        int gameNumber = 1;
        foreach (var result in results)
        {
            GameObject rowObj = Instantiate(rowPrefab, contentParent);
            TextMeshProUGUI[] texts = rowObj.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length < 5)
            {
                Debug.LogError($"Row prefab doesn't have enough Text components. Found: {texts.Length}, Need: 5");
                continue;
            }

            texts[0].text = result.missProduct.ToString();
            string formattedTime = $"{result.averageResponseTime:0.00}";
            Debug.Log(formattedTime+ "  formattedTime");
            Debug.Log($"{result.averageResponseTime:0.00}");
            texts[1].text = "\u200E" + ReverseNumber(result.averageResponseTime) + " שניות";

            texts[2].text = GetObstacleText(result.obstacleLevel);
            texts[3].text = GetDifficultyText(result.spSpeedLevel);
            texts[4].text = gameNumber.ToString();

            gameNumber++;
        }
    }
    private string ReverseNumber(float number)
    {
        char[] charArray = number.ToString("F2").ToCharArray();
        System.Array.Reverse(charArray);
        return new string(charArray);
    }


    void SetupButton()
    {
        if (backButton != null)
        {
            PlayerPrefs.SetInt("ShowTutorial", 0);
            backButton.onClick.AddListener(() => SceneManager.LoadScene("MenuScene"));
        }
    }
}