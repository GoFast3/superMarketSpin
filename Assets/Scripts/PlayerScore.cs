using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    public int score = 0;
    public int obstracles = 0;
    public Text scoreText; // הצגת ניקוד
    public Text obstraclesText; // הצגת מכשולים

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreDisplay();
    }

    public void AddObstracles(int points)
    {
        obstracles += points;
        UpdateObstraclesDisplay();
    }

    void UpdateScoreDisplay()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    void UpdateObstraclesDisplay()
    {
        obstraclesText.text = "Obstacles: " + obstracles.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("obstracle"))
        {
            Debug.Log("Obstacle hit!");
            AddObstracles(1);
        }
    }
}
