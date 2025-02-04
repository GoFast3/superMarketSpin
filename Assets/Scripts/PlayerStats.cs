using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private float totalResponseTime = 0;
    private int responseCount = 0;
    private float AverageResponseTime = 0;
    private float missProduct = 0;

    private float SpeedLevel;
    private float obstacleLevel;

    void Start()
    {

        SpeedLevel = PlayerPrefs.GetFloat("forwardSpeed", 1);
        obstacleLevel = PlayerPrefs.GetFloat("hasObstacles", 1);
    }

    public void AddResponseTime(float time)
    {
        totalResponseTime += time;
        responseCount++;
        AverageResponseTime = totalResponseTime / responseCount;
    }

    public void AddMissedProduct()
    {
        missProduct++;
    }

    public float GetAverageResponseTime()
    {
        return AverageResponseTime;
    }

    public float GetMissedProducts()
    {
        return missProduct;
    }

    public float GetSpeedLevel()
    {
        return SpeedLevel;
    }

    public float GetObstacleLevel()
    {
        return obstacleLevel;
    }
}