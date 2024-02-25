public static class PlayerStateFactory
{
    public static BaseState Grounded(PlayerStateManager player) => new GroundedState(player);
    public static BaseState Jump(PlayerStateManager player) => new JumpState(player);
    public static BaseState Fall(PlayerStateManager player) => new FallState(player);
}
