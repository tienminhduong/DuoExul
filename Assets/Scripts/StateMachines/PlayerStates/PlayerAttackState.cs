public class PlayerAttackState : BasePlayerState
{
    public PlayerAttackState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // player.Attack();
        AnimationController.CrossFade(AnimationController.AttackHash, 0.1f);
    }
}