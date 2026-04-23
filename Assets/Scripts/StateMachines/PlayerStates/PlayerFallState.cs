public class PlayerFallState : BasePlayerState
{
    public PlayerFallState(PlayerController player) : base(player)
    {
        player.OnGrounded += () => player.ChangeState<PlayerIdleState>();
    }

    public override void Enter()
    {
        base.Enter();
        player.Rigidbody.gravityScale *= 2f;
        player.Rigidbody.linearVelocityY = 0f;

        var fallAnimData = new AnimationData(AnimationData.PriorityLevel.Standard, "Fall");
        var _ = AnimationController.PlayAnimation(fallAnimData);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.UpdateMoving();
    }

    public override void Exit()
    {
        base.Exit();
        player.Rigidbody.gravityScale /= 2f;
    }
}