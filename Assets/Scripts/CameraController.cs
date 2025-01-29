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
    [SerializeField] private float fixedHeight ; // גובה קבוע למצלמה

    private void LateUpdate()
    {
        Vector3 followPos = target.position + offset;
        followPos.y = fixedHeight + offset.y;
        transform.position = followPos;
    }
}