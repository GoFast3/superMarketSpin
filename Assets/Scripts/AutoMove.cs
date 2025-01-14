using UnityEngine;

/// <summary>
/// Controls character movement, animation and lane switching
/// </summary>
public class AutoMove : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public CharacterController controller;
    [Header("Movement Settings")]
    public float forwardSpeed = 5f;
    public float jumpHeight = 2f;
    public float laneDistance = 2f;
    [Header("Lane Settings")]
    [SerializeField] private int currentLane = 0;
    [SerializeField] private float[] lanePositions = new float[] { 0f, 2f, 4f };
    private Vector3 velocity;
    private bool isGrounded;
    private bool isJumping = false;
    private float targetX = 0f;
    void Update()
    {
        // Ground check
        isGrounded = controller.isGrounded;
        if (isGrounded && !isJumping)
        {
            velocity.y = -0.9f;
        }
        Vector3 move = Vector3.forward * forwardSpeed;
        // Handle lane switching
        HandleLaneSwitching();
        // Calculate lane movement
        float xPos = transform.position.x;
        float moveX = (targetX - xPos);
        if (Mathf.Abs(moveX) > Mathf.Abs(targetX - xPos))
        {
            moveX = targetX - xPos;
        }
        move.x = moveX;
        // Handle jumping
        HandleJumping();
        // Move character
        controller.Move(move);
        transform.forward = Vector3.forward;
        // Check if jump animation is complete
        CheckJumpAnimationEnd();
    }
    /// <summary>
    /// Handles lane switching input
    /// </summary>
    private void HandleLaneSwitching()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentLane > 0)
        {
            Debug.Log($"Moving left - Lane: {currentLane}");
            currentLane--;
            targetX = lanePositions[currentLane];
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && currentLane < 2)
        {
            Debug.Log($"Moving right - Lane: {currentLane}");
            currentLane++;
            targetX = lanePositions[currentLane];
        }
    }
    /// <summary>
    /// Handles jumping input and animation
    /// </summary>
    private void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            Debug.Log("Jumping");
            isJumping = true;
            animator.CrossFadeInFixedTime("Jump", 0.1f);
        }
    }
    /// <summary>
    /// Checks if jump animation has ended
    /// </summary>
    private void CheckJumpAnimationEnd()
    {
        if (isJumping && isGrounded && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            isJumping = false;
        }
    }
}