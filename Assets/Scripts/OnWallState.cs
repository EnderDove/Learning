using UnityEngine;

public class OnWallState : BaseState
{
    public OnWallState(PlayerMovement player) => Player = player;

    private float lastOnWallTime;

    public override void FixedUpdateState(float deltaTime)
    {
        Player.PlayerBody.velocity = Vector2.down * Player.playerState.slidingSpeed;
        lastOnWallTime -= deltaTime;

        if (Physics2D.OverlapBox(Player.GroundCheckerPoint.position, Player.GroundCheckerSize, 0, Player.WhatIsGround))
        {
            Player.ChangeState(PlayerStateFactory.Grounded(Player));
            return;
        }
        if (Physics2D.OverlapBox(Player.WallCheckerPoint.position, Player.WallCheckerSize, 0, Player.WhatIsGround))
            lastOnWallTime = Player.playerState.coyoteTime;

        if (lastOnWallTime > 0 && Player.LastPressedJumpTime > 0)
        {
            Vector2 jumpDir = new (Player.IsFacingRight? -1 : 1 ,1);
            Player.ChangeState(PlayerStateFactory.Jump(Player, jumpDir));
            return;
        }
        if (lastOnWallTime < 0)
        {
            Player.ChangeState(PlayerStateFactory.Fall(Player));
            return;
        }

        if (Player.MoveInput.x < 0 && Player.IsFacingRight)
        {
            Player.PlayerBody.AddForce(Vector2.left);
            Player.ChangeState(PlayerStateFactory.Fall(Player));
            return;
        }
        if (Player.MoveInput.x > 0 && !Player.IsFacingRight)
        {
            Player.PlayerBody.AddForce(Vector2.right);
            Player.ChangeState(PlayerStateFactory.Fall(Player));
            return;
        }

    }
}
