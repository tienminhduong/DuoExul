using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackHitbox : MonoBehaviour
{
    public UnityAction<IEntity> OnHit;
    public LayerMask hittableLayers;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & hittableLayers) != 0)
        {
            if (collision.TryGetComponent<IEntity>(out var hitEntity))
            {
                OnHit?.Invoke(hitEntity);
            }
        }
    }
}