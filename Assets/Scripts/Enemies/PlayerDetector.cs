using Mono.Cecil.Cil;
using UnityEngine;
using Utilities;

public interface IDetectionStrategy
{
    bool Execute(Transform detector, Transform target, LayerMask sightsBlock, LayerMask targetMask, CountdownTimer timer);
}

public class LineOfSightStrategy : IDetectionStrategy
{
    private float maxRange;
    public LineOfSightStrategy(float detectionRange)
    {
        maxRange = detectionRange;
    }
    public bool Execute(Transform detector, Transform target, LayerMask sightsBlock, LayerMask targetMask, CountdownTimer timer)
    {
        if(timer.IsRunning) return false; // If the timer is still running, we consider the target as detected without performing a new check
        Vector2 diff = target.position - detector.position;
        float distance = diff.magnitude;

        // 1. Kiểm tra khoảng cách trước
        if (maxRange > 0 && distance > maxRange)
        {
            Debug.Log("too far");
            return false;
        }

        // 2. Bắn tia. 
        RaycastHit2D hit = Physics2D.Raycast(detector.position, diff.normalized, distance + 0.1f, sightsBlock);

        if (hit.collider != null)
        {
            // 3. Sử dụng phép dịch bit để so sánh layer chính xác
            // Kiểm tra xem layer của vật va chạm có nằm trong LayerMask không
            if (((1 << hit.collider.gameObject.layer) & targetMask) != 0)
            {
                Debug.Log("Player detected");
                timer.Start(); // Start cooldown timer after a successful detection check
                return true;
            }
        }
        Debug.Log("Player not detected " + hit.collider.gameObject.name + " " + (1 << hit.collider.gameObject.layer) + " " + targetMask.value);
        return false;
    }
}
public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private float detectionCooldown = 1f; // Time in seconds between detection checks
    [SerializeField] private LayerMask sightBlockerMasks; // Layer mask to identify objects that can block sight
    [SerializeField] private LayerMask playerLayer; // Layer mask to identify the player
    [SerializeField] private float detectionRange = 5f; // Maximum range for line of sight detection
    public GameObject Player { get; private set; }
    private CountdownTimer detectionTimer;
    IDetectionStrategy detectionStrategy;

    private void Awake()
    {
        var player = FindAnyObjectByType<PlayerController>();
        if (player == null)
        {
            Debug.LogError("PlayerController not found in the scene. Please ensure there is a GameObject with a PlayerController component.");
        }
        Player = player.gameObject; // Find the player in the scene      
    }
    private void Start()
    {
        detectionStrategy = new LineOfSightStrategy(detectionRange);
        detectionTimer = new CountdownTimer(detectionCooldown);
    }

    private void Update()
    {
        detectionTimer.Tick(Time.deltaTime);
    }

    public bool CanDetectPlayer()
    {
        var canDetect = detectionTimer.IsRunning || 
            detectionStrategy.Execute(transform, Player.transform, sightBlockerMasks, playerLayer, detectionTimer);
        Debug.Log($"Can detect player: {canDetect}");
        return canDetect;
    }

    public void SetDetectionStrategy(IDetectionStrategy strategy)
    {
        detectionStrategy = strategy;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, detectionRange);
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawLine(transform.position, Player.transform.position);
    //}
}
