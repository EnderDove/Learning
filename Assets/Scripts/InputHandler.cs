using UnityEngine;

public class InputHandler
{
    public InputHandler(PlayerStateManager player)
    {
        this.player = player;
    }

    public float LastPressedJumpTime { get; private set; }
    public Vector2 MoveInput => moveInput;

    private readonly PlayerStateManager player;
    private Vector2 moveInput;

    public void Update(float deltaTime)
    {
        LastPressedJumpTime -= deltaTime;

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpInput();
        }

        void OnJumpInput()
        {
            LastPressedJumpTime = player.playerState.jumpInputBufferTime;
        }
    }

}