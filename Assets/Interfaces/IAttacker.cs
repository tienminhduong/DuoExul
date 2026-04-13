public interface IAttacker : IEntity
{
    int BaseAttack { get; }
    void Attack(AttackData attackData);
    AnimationController AnimationController { get; }
}