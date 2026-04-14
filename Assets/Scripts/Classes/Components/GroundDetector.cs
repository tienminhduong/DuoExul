using UnityEngine;
using UnityEngine.Events;

public class GroundDetector : MonoBehaviour
{
    public UnityAction OnGrounded;
    public UnityAction OnLeftGround;

    [SerializeField] private LayerMask groundLayer;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (groundLayer.Contains(collision.gameObject))
            OnGrounded?.Invoke();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (groundLayer.Contains(collision.gameObject))
            OnLeftGround?.Invoke();
    }
}