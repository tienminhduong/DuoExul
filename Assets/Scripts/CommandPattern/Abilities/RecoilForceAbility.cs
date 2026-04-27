using UnityEngine;

public class RecoilForceAbility : IAbility
{
    [SerializeField] private float recoilForce;
    public void ApplyEffect(IEntity executor, IEntity target)
    {
        var forceDirection = new Vector2(executor.Rigidbody.transform.localScale.x, 0);
        executor.Rigidbody.AddForce(forceDirection * recoilForce, ForceMode2D.Impulse);
    }
}