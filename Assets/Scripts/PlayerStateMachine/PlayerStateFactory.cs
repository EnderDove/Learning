public static class PlayerStateFactory
{
    public static BaseState Grounded(PlayerController player) => new GroundedState(player);
    public static BaseState Jump(PlayerController player) => new JumpState(player);
    public static BaseState Fall(PlayerController player) => new FallState(player);
}
