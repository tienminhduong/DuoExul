using UnityEngine;

// ═══════════════════════════════════════════════════════════════════════
//  Shockwave  –  dùng cho Taurus Slam & Stomp
//  Prefab yêu cầu: Collider2D (isTrigger = true)
//  Scale X của prefab = chiều rộng shockwave (set bởi TaurusBoss.SpawnShockwave)
// ═══════════════════════════════════════════════════════════════════════
public class Shockwave : MonoBehaviour
{
    public int damage = 40;
    public float lifetime = 0.35f;
    public float knockX = 8f;
    public float knockY = 10f;

    void Start() => Destroy(gameObject, lifetime);

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        other.GetComponent<HealthComponent>()?.TakeDamage(damage);

        var rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float dir = Mathf.Sign(other.transform.position.x - transform.position.x);
            rb.linearVelocity = new Vector2(dir * knockX, knockY);
        }
    }
}
