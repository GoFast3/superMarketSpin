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
    public float laneDistance = 2f;
    public float jumpForce = 4f;
    public float gravity = -20f;

    [Header("Lane Settings")]

    [SerializeField] private float[] lanePositions = new float[] { 0f, 2f, 4f };
    private Vector3 velocity;
    private bool isGrounded;
    private bool isJumping = false;
    private float targetX = 0f;
    private int LandCunt;

    public float minSpawnDistance;
    private int gameMode;
    private int currentLane;
    bool hasObstacles;




    public void Start()
    {

        transform.position = new Vector3(2f, transform.position.y, transform.position.z);

        forwardSpeed = PlayerPrefs.GetFloat("forwardSpeed");
        gameMode = PlayerPrefs.GetInt("GameMode", 1);


        if (gameMode == 0)
        {
            lanePositions = new float[] { 0f, 2f };
            hasObstacles = false;
            LandCunt = 2;
            currentLane = 0;


        }
        else if (gameMode == 1)
        {
            lanePositions = new float[] { 0f, 2f, 4f };
            hasObstacles = false;
            LandCunt = 3;
            currentLane = 1;

        }
        else
        {
            lanePositions = new float[] { 0f, 2f, 4f };
            hasObstacles = true;
            LandCunt = 3;
            currentLane = 1;

        }

        Debug.Log("gamemode " + gameMode);

        targetX = lanePositions[currentLane];
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

        Debug.Log("gamemode " + gameMode);

    }


    void Update()
    {
        if (TutorialManager.IsTutorialActive) return;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.1f) && velocity.y <= 0;

        if (isGrounded)
        {
            if (!isJumping)
            {
                velocity.y = -0.9f;
            }

            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        Vector3 move = Vector3.forward * forwardSpeed;

        HandleLaneSwitching();
        float xPos = transform.position.x;
        float moveX = targetX - xPos;
        move.x = moveX;

        move.y = velocity.y * Time.deltaTime;

        HandleJumping();
        controller.Move(move);

        transform.forward = Vector3.forward;


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
        else if (Input.GetKeyDown(KeyCode.RightArrow) && currentLane < (LandCunt - 1))
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


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            Debug.Log("Jumping");
            isJumping = true;
            velocity.y = Mathf.Sqrt(jumpForce * -1f * gravity);
            //animator.CrossFadeInFixedTime("Jump", 0.1f);
            animator.Play("Jump");
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