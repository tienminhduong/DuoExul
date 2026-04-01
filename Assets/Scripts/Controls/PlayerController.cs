using TriInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Vector2 velocity;
    [SerializeField] private float moveSpeed = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void SetVelocity(Vector2 newVelocity)
    {
        velocity = newVelocity.normalized * moveSpeed;
    }

    private void HandleMoving()
    {
        Debug.Log("Player is moving with velocity: " + velocity);
        transform.Translate(velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        HandleMoving();
    }
}