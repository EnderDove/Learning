using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState")]
public class PlayerState : ScriptableObject
{

    [Header("Run")]
    public float runMaxSpeed;
    [Min(1)] public float runAccelAmount;
    [Min(1)] public float runDeccelAmount;

    [Space(5)]

    [Header("Jump")]
    public float jumpForce;
    public float coyoteTime = 0.2f;
    public float jumpInputBufferTime = 0.2f;

    public float accelInAir;
    public float deccelInAir;
}
