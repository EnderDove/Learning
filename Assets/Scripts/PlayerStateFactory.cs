public static class PlayerStateFactory
{
    public static BaseState Grounded(PlayerMovement player) => new GroundedState(player);
    public static BaseState Jump(PlayerMovement player) => new JumpState(player);
    public static BaseState Fall(PlayerMovement player) => new FallState(player);

}
