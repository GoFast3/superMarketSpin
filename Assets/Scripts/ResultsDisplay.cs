using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultsDisplay : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private Button backButton;

    private GameProgress gameProgress;

    // Convert speed level to English text
    private string GetDifficultyText(float level)
    {
        return level switch
        {
            0.2f => "Easy",
            0.3f => "Medium",
            0.6f => "Hard",
            _ => "Unknown"
        };
    }

    // Convert obstacle level to English text
    private string GetObstacleText(float level)
    {
        return level switch
        {
            0 => "No Obstacles",
            1 => "With Obstacles",
            _ => "Unknown"
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

        foreach (var result in results)
        {
            Debug.Log($"Creating row for result date: {result.date}");
            Debug.Log($"Creating row for result miss pro: {result.missProduct}");
            Debug.Log($"Creating row for resultobstacleLevel: {GetObstacleText(result.obstacleLevel)}");
            Debug.Log($"Creating row for resultspSpeedLevel: {GetDifficultyText(result.spSpeedLevel)}");

            GameObject rowObj = Instantiate(rowPrefab, contentParent);
            Text[] texts = rowObj.GetComponentsInChildren<Text>();

            if (texts.Length < 5)
            {
                Debug.LogError($"Row prefab doesn't have enough Text components. Found: {texts.Length}, Need: 5");
                continue;
            }

            Debug.Log("textttt00" + texts[0].text);
            Debug.Log("textttt11" + texts[1].text);
            Debug.Log("textttt22" + texts[2].text);
            Debug.Log("textttt33" + texts[3].text);
            Debug.Log("textttt44" + texts[4].text);

            texts[0].text = result.date.ToString();
            texts[1].text = result.missProduct.ToString();
            texts[2].text = GetObstacleText(result.obstacleLevel); 
            texts[3].text = GetDifficultyText(result.spSpeedLevel); 
            texts[4].text = $"{result.averageResponseTime:F2}s";
        }
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