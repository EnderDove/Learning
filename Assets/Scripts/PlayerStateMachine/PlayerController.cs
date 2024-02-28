using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D PlayerBody { get; private set; }
    public InputHandler InputHandler { get; private set; }
    public PlayerState playerState;

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

    [Header("Layers")]
    [SerializeField] private LayerMask whatIsGround;
    public LayerMask WhatIsGround => whatIsGround;

    private void Awake()
    {
        IsFacingRight = true;
        FootstepsEmissionModule = FootstepsParticles.emission;
        PlayerBody = GetComponent<Rigidbody2D>();
        currentState = PlayerStateFactory.Fall(this);
        InputHandler = new(this);
    }

    private void Update()
    {
        currentState.UpdateState(Time.deltaTime);
        InputHandler.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(Time.fixedDeltaTime);
    }

    public void SwitchState(BaseState state)
    {
        currentState.ExitState();
        GC.SuppressFinalize(currentState);
        currentState = state;
        currentState.EnterState();
    }

    public void Run(float lerpAmount, float accel, float deccel)
    {
        float targetSpeed = InputHandler.MoveInput.x * playerState.runMaxSpeed;

        targetSpeed = Mathf.Lerp(PlayerBody.velocity.x, targetSpeed, lerpAmount);

        float accelRate;
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : deccel;


        float speedDif = targetSpeed - PlayerBody.velocity.x;
        float movement = speedDif * accelRate;

        PlayerBody.AddForce(movement * Vector2.right, ForceMode2D.Force);
        if (InputHandler.MoveInput.x != 0)
        {
            CheckFaceDirection();
        }
    }

    public void CheckFaceDirection()
    {
        if (IsFacingRight != (InputHandler.MoveInput.x > 0.01f))
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
