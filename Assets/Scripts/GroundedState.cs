using UnityEngine;

public class GroundedState : BaseState
{
    public GroundedState(PlayerMovement player) => Player = player;

    public float LastOnGroundTime { get; private set; }

    public override void FixedUpdateState(float deltaTime)
    {
        Player.FootstepsEmissionModule.enabled = Player.MoveInput.x != 0;
        Player.Run(1, Player.playerState.runAccelAmount, Player.playerState.runDeccelAmount);
        LastOnGroundTime -= deltaTime;

        if (Physics2D.OverlapBox(Player.GroundCheckerPoint.position, Player.GroundCheckerSize, 0, Player.WhatIsGround))
        {
            LastOnGroundTime = Player.playerState.coyoteTime;
        }
        if (LastOnGroundTime > 0 && Player.LastPressedJumpTime > 0)
        {
            Player.ChangeState(PlayerStateFactory.Jump(Player, Vector2.up));
            return;
        }
        if (LastOnGroundTime < 0)
        {
            Player.ChangeState(PlayerStateFactory.Fall(Player));
            return;
        }
    }

    public override void EnterState()
    {
        Player.FootstepsEmissionModule.enabled = true;
        Player.LandingParticles.Play();
    }

    public override void ExitState()
    {
        Player.FootstepsEmissionModule.enabled = false;
    }
}