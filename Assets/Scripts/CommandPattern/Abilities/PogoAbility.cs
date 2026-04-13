using UnityEngine;

public class PogoAbility : IAbility
{
    [SerializeField] private float bounceForce = 10f;
    public void ApplyEffect(IEntity executor, IEntity target)
    {
        if (executor is PlayerController player)
        {
            if (target is IObject obj && obj.ObjectType == ObjectType.HardObject)
            {
                player.Rigidbody.linearVelocityY = 0f;
                player.Rigidbody.AddForce(new Vector2(0, bounceForce), ForceMode2D.Impulse);
            }
        }
    }
}