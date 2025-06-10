
using UnityEngine;
using System;
using System.Collections.Generic;
[System.Serializable]
public class GameResult
{
    public DateTime date;
    public float averageResponseTime;
    public float missProduct;
    public float spSpeedLevel;
    public float obstacleLevel;
}
public class GameProgress : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private List<GameResult> gameHistory = new List<GameResult>();

    void Start()
    {
        LoadHistory();
    }
    public void SaveGameResult(float responseTime, float missed, float speedLevel, float obstacles)
    {
        obstacles = PlayerPrefs.GetInt("GameMode", 1) == 2 ? 1 : 0;

        GameResult result = new GameResult
        {
            date = DateTime.Now,
            averageResponseTime = responseTime,
            missProduct = missed,
            spSpeedLevel = speedLevel,
            obstacleLevel = obstacles
        };
        gameHistory.Add(result);
        SaveHistory();
    }
    private void SaveHistory()
    {
        string json = JsonUtility.ToJson(new { history = gameHistory });
        PlayerPrefs.SetString("GameHistory", json);
        PlayerPrefs.Save();
        Debug.Log("History saved: " + json);
    }
    private void LoadHistory()
    {
        if (PlayerPrefs.HasKey("GameHistory"))
        {
            string json = PlayerPrefs.GetString("GameHistory");
            JsonWrapper wrapper = JsonUtility.FromJson<JsonWrapper>(json);
            gameHistory = wrapper.history;
            Debug.Log($"Loaded {gameHistory.Count} game records");
        }
    }
    public List<GameResult> GetHistory()
    {
        return gameHistory;
    }
}
[System.Serializable]
public class JsonWrapper
{
    public List<GameResult> history;
}