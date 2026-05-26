public interface IAttacker : IEntity
{
    float BaseAttack { get; }
    void Attack(AttackData attackData);
    AnimationController AnimationController { get; }
}