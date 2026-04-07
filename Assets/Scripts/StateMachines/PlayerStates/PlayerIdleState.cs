public class PlayerIdleState : BasePlayerState
{
    AnimationData idleAnimData = new(AnimationData.PriorityLevel.Standard, "Idle");
    public PlayerIdleState(PlayerController player) : base(player)
    {
    }

    override public void Enter()
    {
        base.Enter();
        var _ = AnimationController.PlayAnimation(idleAnimData);
    }

    public override void Update()
    {
        base.Update();
        if (player.Direction != 0)
            player.ChangeState<PlayerWalkingState>();
    }
}