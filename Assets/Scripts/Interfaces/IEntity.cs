using UnityEngine;

public interface IEntity
{
    void Attack(AttackData attackData);

    AnimationController AnimationController { get; }
}