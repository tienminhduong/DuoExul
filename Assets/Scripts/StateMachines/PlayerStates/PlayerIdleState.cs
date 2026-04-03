public class PlayerIdleState : BasePlayerState
{
    public PlayerIdleState(PlayerController player) : base(player)
    {
        
    }

    override public void Enter()
    {
        base.Enter();
        animator.CrossFade(IdleHash, 0.1f);
    }

    public override void Update()
    {
        base.Update();
        if (player.Direction != 0)
            player.ChangeState<PlayerWalkingState>();
    }
}