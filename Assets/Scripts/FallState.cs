using UnityEngine;

public class FallState : BaseState
{
    public FallState(PlayerMovement player) => Player = player;

    public override void FixedUpdateState(float deltaTime)
    {
        Player.Run(1, Player.playerState.accelInAir, Player.playerState.deccelInAir);

        if (Physics2D.OverlapBox(Player.GroundCheckerPoint.position, Player.GroundCheckerSize, 0, Player.WhatIsGround))
        {
            Player.ChangeState(PlayerStateFactory.Grounded(Player));
            return;
        }

        if (Physics2D.OverlapBox(Player.WallCheckerPoint.position, Player.WallCheckerSize, 0, Player.WhatIsGround))
        {
            Player.ChangeState(PlayerStateFactory.OnWall(Player));
            return;
        }
    }

}