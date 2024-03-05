public abstract class BaseState
{
    public PlayerMovement Player { get; protected set; }
    public virtual void EnterState()
    {
        return;
    }
    public virtual void ExitState()
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

}
