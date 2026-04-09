using UnityEngine;

public interface IEntity
{
    void Attack(AttackData attackData);

    AnimationController AnimationController { get; }
    HealthComponent HealthComponent { get; }
    Rigidbody2D Rigidbody { get; }
    int BaseAttack { get; }
    int Direction { get; }

}