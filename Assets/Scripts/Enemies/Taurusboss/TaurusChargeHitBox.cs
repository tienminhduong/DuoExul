using UnityEngine;

/// <summary>
/// Attach lên child GameObject chứa Collider2D của hitbox Charge.
/// Collider2D phải set isTrigger = true và disabled mặc định.
/// TaurusBoss sẽ enable/disable qua chargeHitbox reference.
/// </summary>
public class TaurusChargeHitbox : MonoBehaviour
{
    [Tooltip("Kéo TaurusBoss vào đây")]
    public TaurusBoss boss;

    // Tránh hit cùng 1 player 2 lần trong 1 lần Charge
    bool _hitThisCharge;

    void OnEnable() => _hitThisCharge = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_hitThisCharge || !other.CompareTag("Player")) return;
        _hitThisCharge = true;

        var playerHealthComponent = other.GetComponent<HealthComponent>();

        if (playerHealthComponent != null)
            playerHealthComponent.TakeDamage(Mathf.FloorToInt(boss.config.chargeDamage));
        else
            Debug.LogWarning($"TaurusChargeHitbox: Collider {other.name} không có HealthComponent");

            var rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float dir = Mathf.Sign(other.transform.position.x - boss.transform.position.x);
            rb.linearVelocity = new Vector2(dir * boss.config.chargeKnockbackX, boss.config.chargeKnockbackY);
        }
    }
}