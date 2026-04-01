public abstract class BasePlayerState : IState
{
    protected PlayerController player;

    public BasePlayerState(PlayerController player)
    {
        this.player = player;
    }
    
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
}