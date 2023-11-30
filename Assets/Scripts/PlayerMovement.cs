using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerBody;
    private Vector2 moveInput;

    public ParticleSystem FootstepsParticles;
    private ParticleSystem.EmissionModule footstepsEmissionModule;

    public PlayerState playerState;

    // Timers
    public float LastOnGroundTime { get; private set; }

    // Checkers
    private bool isFacingRight = true;

    private void Awake()
    {
        //LastOnGroundTime = 0f;

        footstepsEmissionModule = FootstepsParticles.emission;
        playerBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //LastOnGroundTime -= Time.fixedDeltaTime;

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        footstepsEmissionModule.enabled = false;
        if (moveInput.x != 0)
        {
            CheckFaceDirection(moveInput.x > 0.01f);
            footstepsEmissionModule.enabled = true;
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

    private void CheckFaceDirection(bool isMovingRight)
    {
        if (isFacingRight != isMovingRight)
            Turn();

    }

    private void Turn()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        isFacingRight = !isFacingRight;
    }
}
