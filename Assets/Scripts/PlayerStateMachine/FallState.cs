using UnityEngine;

public class FallState : BaseState
{
    public FallState(PlayerController player)
    {
        Player = player;
    }

    public override void FixedUpdateState(float deltaTime)
    {
        if (Physics2D.OverlapBox(Player.GroundCheckerPoint.position, Player.GroundCheckerSize, 0, Player.WhatIsGround))
        {
            Player.SwitchState(PlayerStateFactory.Grounded(Player));
            return;
        }
        Player.Run(1, Player.playerState.accelInAir, Player.playerState.deccelInAir);
    }
}
