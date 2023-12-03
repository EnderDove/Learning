using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerBody;
    private Vector2 moveInput;

    // from homework 2
    public ParticleSystem FootstepsParticles;
    private ParticleSystem.EmissionModule footstepsEmissionModule;

    public PlayerState playerState;

    // Timers
    public float LastOnGroundTime { get; private set; }
    public float LastPressedJumpTime { get; private set; }

    // Checkers
    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }

    [SerializeField] private Transform groundCheckerPoint;
    [SerializeField] private Vector2 groundCheckerSize = new(0.5f, 0.03f);

    [Header("Layers")]
    [SerializeField] private LayerMask whatIsGround;

    private void Awake()
    {
        IsFacingRight = true;
        footstepsEmissionModule = FootstepsParticles.emission;
        playerBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        LastOnGroundTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        footstepsEmissionModule.enabled = false;
        if (moveInput.x != 0)
        {
            CheckFaceDirection(moveInput.x > 0.01f);
            if (!IsJumping)
                footstepsEmissionModule.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpInput();
        }

        if (!IsJumping)
        {
            if (Physics2D.OverlapBox(groundCheckerPoint.position, groundCheckerSize, 0, whatIsGround))
            {
                LastOnGroundTime = playerState.coyoteTime;
            }
        }

        if (IsJumping && playerBody.velocity.y < 0)
        {
            IsJumping = false;
        }

        if (CanJump() && LastPressedJumpTime > 0)
        {
            IsJumping = true;
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Run(1);
    }

    private void Run(float lerpAmount)
    {
        float targetSpeed = moveInput.x * playerState.runMaxSpeed;

        targetSpeed = Mathf.Lerp(playerBody.velocity.x, targetSpeed, lerpAmount);

        float accelRate;
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerState.runAccelAmount : playerState.runDeccelAmount;


        float speedDif = targetSpeed - playerBody.velocity.x;
        float movement = speedDif * accelRate;

        playerBody.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private void Jump()
    {
        LastOnGroundTime = 0;
        LastPressedJumpTime = 0;

        float force = playerState.jumpForce;

        if (playerBody.velocity.y < 0)
        {
            force -= playerBody.velocity.y;
        }

        playerBody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    private void OnJumpInput()
    {
        LastPressedJumpTime = playerState.jumpInputBufferTime;
    }

    private bool CanJump()
    {
        return LastOnGroundTime > 0 && !IsJumping;
    }

    private void CheckFaceDirection(bool isMovingRight)
    {
        if (IsFacingRight != isMovingRight)
            Turn();

    }

    private void Turn()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        IsFacingRight = !IsFacingRight;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(groundCheckerPoint.position, groundCheckerSize);
    }
}
