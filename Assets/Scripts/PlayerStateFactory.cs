using UnityEngine;

public static class PlayerStateFactory
{
    public static BaseState Grounded(PlayerMovement player) => new GroundedState(player);
    public static BaseState Jump(PlayerMovement player, Vector2 jumpDir) => new JumpState(player, jumpDir);
    public static BaseState Fall(PlayerMovement player) => new FallState(player);
    public static BaseState OnWall(PlayerMovement player) => new OnWallState(player);
}
