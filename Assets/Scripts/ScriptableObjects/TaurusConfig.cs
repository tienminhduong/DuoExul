using UnityEngine;

[CreateAssetMenu(menuName = "ZodiacBoss/TaurusConfig", fileName = "TaurusConfig")]
public class TaurusConfig : ScriptableObject
{
    [Header("Health")]
    public float maxHP = 700f;
    [Range(0f, 1f)]
    public float phase2HPRatio = 0.5f;

    [Header("Movement")]
    public float walkSpeed = 2.5f;
    public float walkSpeedP2 = 3.5f;
    public float detectionRange = 14f;
    public float meleeRange = 2.2f;

    [Header("Idle")]
    public float idleDuration = 1.2f;

    [Header("Hurt")]
    public float hurtDuration = 0.3f;
    public float postSkillPause = 0.5f;   // dừng ngắn sau mỗi skill

    // ── Skill 1 : Charge ─────────────────────────────────────────────────
    [Header("Skill 1 – Charge  (CD 5s)")]
    [Tooltip("Player phải cách xa ít nhất khoảng này thì boss mới Charge")]
    public float chargeMinRange = 5f;
    public float chargeCooldown = 5f;
    public float chargeWindup = 0.55f;  // cúi đầu chuẩn bị
    public float chargeSpeed = 18f;
    public float chargeDuration = 0.85f;  // thời gian lao
    public float chargeDamage = 35f;
    public float chargeKnockbackX = 12f;
    public float chargeKnockbackY = 6f;

    // ── Skill 2 : Ground Slam ────────────────────────────────────────────
    [Header("Skill 2 – Ground Slam  (CD 7s)")]
    [Tooltip("Dùng khi player ở gần trong meleeRange")]
    public float slamCooldown = 7f;
    public float slamWindup = 0.5f;
    public float slamDamage = 45f;
    public float slamShockwaveW = 7f;     // chiều rộng shockwave

    // ── Skill 3 : Stomp Barrage (Phase 2) ───────────────────────────────
    [Header("Skill 3 – Stomp Barrage  (CD 10s, Phase 2 only)")]
    public float stompCooldown = 10f;
    public int stompCount = 3;
    public float stompInterval = 0.4f;   // delay giữa các lần giậm
    public float stompDamage = 20f;
    public float stompShockwaveW = 3.5f;

    [Header("Phase 2 Scaling")]
    public float p2CooldownMult = 0.65f;  // tất cả CD × 0.65
    public float p2SpeedAdd = 1f;     // tốc độ đi thêm
}