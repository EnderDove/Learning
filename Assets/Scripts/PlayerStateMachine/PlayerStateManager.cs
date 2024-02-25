using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public Rigidbody2D PlayerBody { get; private set; }
    public PlayerState playerState;
    public InputHandler inputHandler;

    private BaseState currentState;

    // from homework 2
    public ParticleSystem FootstepsParticles;
    public ParticleSystem.EmissionModule FootstepsEmissionModule;

    // Checkers
    public bool IsFacingRight { get; private set; }

    public Transform GroundCheckerPoint => groundCheckerPoint;
    public Vector2 GroundCheckerSize => groundCheckerSize;

    [SerializeField] private Transform groundCheckerPoint;
    [SerializeField] private Vector2 groundCheckerSize = new(0.5f, 0.03f);

    public LayerMask WhatIsGround => whatIsGround;
    [Header("Layers")]
    [SerializeField] private LayerMask whatIsGround;

    private void Awake()
    {
        IsFacingRight = true;
        FootstepsEmissionModule = FootstepsParticles.emission;
        PlayerBody = GetComponent<Rigidbody2D>();
        currentState = PlayerStateFactory.Fall(this);
        inputHandler = new(this);
    }

    private void Update()
    {
        currentState.UpdateState(Time.deltaTime);
        inputHandler.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(Time.fixedDeltaTime);
    }

    public void Run(float lerpAmount, float accel, float deccel)
    {
        float targetSpeed = inputHandler.MoveInput.x * playerState.runMaxSpeed;

        targetSpeed = Mathf.Lerp(PlayerBody.velocity.x, targetSpeed, lerpAmount);

        float accelRate;
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : deccel;


        float speedDif = targetSpeed - PlayerBody.velocity.x;
        float movement = speedDif * accelRate;

        PlayerBody.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    public void CheckFaceDirection()
    {
        if (IsFacingRight != (inputHandler.MoveInput.x > 0.01f))
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

    public void SwitchState(BaseState state)
    {
        currentState.ExitState();
        currentState = state;
        currentState.EnterState();
    }
}
