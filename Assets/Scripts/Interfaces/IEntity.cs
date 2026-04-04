using UnityEngine;

public interface IEntity
{
    void Attack();

    AnimationController AnimationController { get; }
}