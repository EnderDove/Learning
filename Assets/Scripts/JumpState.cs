using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(PlayerMovement player) => Player = player;

    public override void EnterState()
    {
        Player.Jump();
    }

    public override void FixedUpdateState(float deltaTime)
    {
        Player.Run(1, Player.playerState.accelInAir, Player.playerState.deccelInAir);
        if (Player.PlayerBody.velocity.y < 0)
        {
            Player.ChangeState(PlayerStateFactory.Fall(Player));
            return;
        }
    }
}