public abstract class BaseState
{
    public PlayerStateManager Player { get; set; }

    public virtual void EnterState()
    {
        return;
    }

    public virtual void UpdateState(float deltaTime)
    {
        return;
    }

    public virtual void FixedUpdateState(float deltaTime)
    {
        return;
    }

    public virtual void ExitState()
    {
        return;
    }
}
