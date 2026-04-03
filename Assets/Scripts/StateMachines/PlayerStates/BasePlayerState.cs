using UnityEngine;

public abstract class BasePlayerState : IState
{
    protected PlayerController player;
    protected Animator animator => player.Animator;

    protected int IdleHash = Animator.StringToHash("IdleAnim");
    protected int WalkHash = Animator.StringToHash("WalkAnim");
    protected int JumpHash = Animator.StringToHash("JumpAnim");
    protected int FallHash = Animator.StringToHash("FallAnim");

    public BasePlayerState(PlayerController player)
    {
        this.player = player;
    }
    
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
}