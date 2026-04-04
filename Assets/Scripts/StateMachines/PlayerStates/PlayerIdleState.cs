public class PlayerIdleState : BasePlayerState
{
    public PlayerIdleState(PlayerController player) : base(player)
    {

    }

    override public void Enter()
    {
        base.Enter();
        AnimationController.CrossFade(AnimationController.IdleHash, 0.1f);
    }

    public override void Update()
    {
        base.Update();
        if (player.Direction != 0)
            player.ChangeState<PlayerWalkingState>();
    }
}