using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(PlayerStateManager player)
    {
        Player = player;
    }

    public override void EnterState()
    {
        float force = Player.playerState.jumpForce;

        if (Player.PlayerBody.velocity.y < 0)
        {
            force -= Player.PlayerBody.velocity.y;
        }

        Player.PlayerBody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    public override void FixedUpdateState(float deltaTime)
    {
        if (Player.PlayerBody.velocity.y < 0)
        {
            Player.SwitchState(PlayerStateFactory.Fall(Player));
            return;
        }
        Player.Run(1, Player.playerState.accelInAir, Player.playerState.deccelInAir);
    }
}
