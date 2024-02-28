using UnityEngine;

public class GroundedState : BaseState
{
    public GroundedState(PlayerController player)
    {
        Player = player;
    }

    public float LastOnGroundTime { get; private set; }

    public override void FixedUpdateState(float deltaTime)
    {
        LastOnGroundTime -= Time.deltaTime;
        Player.FootstepsEmissionModule.enabled = false;

        if (Physics2D.OverlapBox(Player.GroundCheckerPoint.position, Player.GroundCheckerSize, 0, Player.WhatIsGround))
        {
            LastOnGroundTime = Player.playerState.coyoteTime;
        }
        if (LastOnGroundTime < 0)
        {
            Player.SwitchState(PlayerStateFactory.Fall(Player));
            return;
        }

        Player.Run(1, Player.playerState.runAccelAmount, Player.playerState.runDeccelAmount);

        if (CanJump() && Player.InputHandler.LastPressedJumpTime > 0)
        {
            Player.SwitchState(PlayerStateFactory.Jump(Player));
            return;
        }
        if (Player.InputHandler.MoveInput.x != 0)
        {
            Player.FootstepsEmissionModule.enabled = true;
        }
    }

    public override void ExitState()
    {
        Player.FootstepsEmissionModule.enabled = false;
    }

    private bool CanJump()
    {
        return LastOnGroundTime > 0;
    }
}
