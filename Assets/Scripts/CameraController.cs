using UnityEngine;
using System.Collections.Generic;
using System.Collections;
/// <summary>
/// Controls camera movement and follows the player with smooth transitions
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] private float speedFollow = 5f;

    private void Start()
    {
        int gameMode = PlayerPrefs.GetInt("GameMode", 1);
        if (gameMode == 0)
        {
            offset.x = 0.77f;
        }
        else
        {
            offset.x = 2.03f;
        }

    }
    private void LateUpdate()
    {
        Vector3 followPos = target.position + offset;
        followPos.y = offset.y;
        followPos.x = offset.x;
        transform.position = followPos;
    }

}