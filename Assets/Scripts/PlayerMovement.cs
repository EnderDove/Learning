using System;
using UnityEngine;

[RequireComponent(typeof(AttackHandler), typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public BaseState CurrentState { get; private set; }
    public Rigidbody2D PlayerBody { get; private set; }
    public Vector2 MoveInput => moveInput;
    private Vector2 moveInput;
    private AttackHandler attackHandler;

    public ParticleSystem.EmissionModule FootstepsEmissionModule;
    public ParticleSystem LandingParticles;
    [SerializeField] private ParticleSystem FootstepsParticles;

    public PlayerState playerState;

    // Timers
    public float LastPressedJumpTime { get; private set; }

    // Checkers
    public bool IsFacingRight { get; private set; }


    public Transform GroundCheckerPoint => groundCheckerPoint;
    public Vector2 GroundCheckerSize => groundCheckerSize;
    [SerializeField] private Transform groundCheckerPoint;
    [SerializeField] private Vector2 groundCheckerSize = new(0.5f, 0.03f);

    public Transform WallCheckerPoint => wallCheckerPoint;
    public Vector2 WallCheckerSize => wallCheckerSize;
    [SerializeField] private Transform wallCheckerPoint;
    [SerializeField] private Vector2 wallCheckerSize = new(0.5f, 0.03f);


    public LayerMask WhatIsGround => whatIsGround;
    [Header("Layers")]
    [SerializeField] private LayerMask whatIsGround;

    private void Awake()
    {
        CurrentState = PlayerStateFactory.Grounded(this);

        IsFacingRight = true;
        FootstepsEmissionModule = FootstepsParticles.emission;
        PlayerBody = GetComponent<Rigidbody2D>();
        attackHandler = GetComponent<AttackHandler>();
    }

    public void ChangeState(BaseState state)
    {
        CurrentState.ExitState();
        GC.SuppressFinalize(CurrentState);
        CurrentState = state;
        CurrentState.EnterState();
    }

    private void Update()
    {
        CurrentState.UpdateState(Time.deltaTime);

        LastPressedJumpTime -= Time.deltaTime;

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpInput();
        }

        if (Input.GetKeyDown(KeyCode.X))
            attackHandler.ballPool.Get();

        if (MoveInput.x != 0)
        {
            CheckFaceDirection(MoveInput.x > 0.01f);
        }
    }

    private void FixedUpdate()
    {
        CurrentState.FixedUpdateState(Time.fixedDeltaTime);
    }

    public void Run(float lerpAmount, float accel, float deccel)
    {
        float targetSpeed = MoveInput.x * playerState.runMaxSpeed;

        targetSpeed = Mathf.Lerp(PlayerBody.velocity.x, targetSpeed, lerpAmount);

        float accelRate;
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : deccel;


        float speedDif = targetSpeed - PlayerBody.velocity.x;
        float movement = speedDif * accelRate;

        PlayerBody.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    public void Jump(Vector2 jumpDir)
    {
        LastPressedJumpTime = 0;

        Vector2 force = jumpDir * playerState.jumpForce;
        force.y -= PlayerBody.velocity.y;

        PlayerBody.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnJumpInput()
    {
        LastPressedJumpTime = playerState.jumpInputBufferTime;
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
        Gizmos.DrawWireCube(wallCheckerPoint.position, wallCheckerSize);
    }
}
