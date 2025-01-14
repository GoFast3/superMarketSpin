using UnityEngine;
using System.Collections.Generic;
using System.Collections;
/// <summary>
/// Controls camera movement and follows the player with smooth transitions
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [Tooltip("Target to follow (usually the player)")]
    [SerializeField] Transform target;

    [Tooltip("Offset from target position")]
    [SerializeField] Vector3 offset;

    [Tooltip("Camera follow speed")]
    [SerializeField] private float speedFollow = 5f;

    private float y;

    private void LateUpdate()
    {
        Vector3 followPos = target.position + offset;

        // Check ground position
        RaycastHit hit;
        if (Physics.Raycast(target.position, Vector3.down, out hit, 2.5f))
        {
            y = Mathf.Lerp(y, hit.point.y, Time.deltaTime * speedFollow);
        }
        else
        {
            y = Mathf.Lerp(y, target.position.y, Time.deltaTime * speedFollow);
        }

        // Update camera position
        followPos.y = y + offset.y;
        transform.position = followPos;
    }
}