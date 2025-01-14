
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering;
/// <summary>
/// Controls player movement, lane switching, and collision detection
/// </summary>
public class PlayerController1 : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Forward movement speed")]
    public float forwardSpeed;
    [Tooltip("Distance between lanes")]
    public float laneDistance = 2f;
    [Tooltip("Jump force amount")]
    public float JumpForce = 8f;
    [Tooltip("Gravity force")]
    public float Gravity = -20f;
    public float minSpawnDistance = 10f;

    [Header("Lane Settings")]
    [SerializeField] private int desiredLane = 0;
    [SerializeField] private float[] lanePositions = new float[] { 0f, 2f, 4f };

    private CharacterController controller;
    private UnityEngine.Vector3 direction;
    private Touch touch;
    private UnityEngine.Vector2 initPos;
    private UnityEngine.Vector2 endPos;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Debug.Log($"Starting speed: {PlayerPrefs.GetFloat("forwardSpeed")}");
        Debug.Log($"Starting min distance: {PlayerPrefs.GetFloat("minSpawnDistance")}");
        forwardSpeed = PlayerPrefs.GetFloat("forwardSpeed");
        minSpawnDistance = PlayerPrefs.GetFloat("minSpawnDistance");
    }

    void Update()
    {
        // Update movement direction
        direction.z = forwardSpeed;
        direction.y += Gravity * Time.deltaTime;

        // Handle jumping
        if (controller.isGrounded && Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }

        // Handle lane switching
        HandleLaneSwitching();

        // Update player position
        UpdatePlayerPosition();
    }

    /// <summary>
    /// Handles player lane switching input
    /// </summary>
    private void HandleLaneSwitching()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && desiredLane < 2)
        {
            desiredLane++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && desiredLane > 0)
        {
            desiredLane--;
        }
    }

    /// <summary>
    /// Updates player position based on current lane
    /// </summary>
    private void UpdatePlayerPosition()
    {
        UnityEngine.Vector3 targetPosition = transform.position;
        targetPosition.x = lanePositions[desiredLane];
        transform.position = targetPosition;
        controller.center = controller.center;
    }

    void FixedUpdate()
    {
        controller.Move(direction * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Applies jump force to player
    /// </summary>
    private void Jump()
    {
        direction.y = JumpForce;
    }

    /// <summary>
    /// Handles collision with obstacles
    /// </summary>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("obstacle"))
        {
            PlayerManager.gameOver = true;
            Debug.Log("Game Over - Collision with obstacle");
        }
    }
}