using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(PlayerMovement player, Vector2 jumpDir) => (Player, _jumpDir) = (player, jumpDir);
    private Vector2 _jumpDir;

    public override void EnterState()
    {
        Player.Jump(_jumpDir);
        Player.LandingParticles.Play();
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