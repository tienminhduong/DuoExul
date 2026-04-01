using TriInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Vector2 velocity;

    [SerializeField] private StateMachine stateMachine;


    [SerializeField] private float moveSpeed = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        stateMachine = new StateMachine();
        stateMachine.AddState(new PlayerIdleState(this));
        stateMachine.AddState(new PlayerWalkingState(this, moveSpeed));

        stateMachine.SetState<PlayerIdleState>();
    }

    public void SetVelocity(Vector2 newVelocity)
    {
        velocity = newVelocity.normalized * moveSpeed;
        if (velocity.magnitude > 0)
        {
            stateMachine.ChangeState<PlayerWalkingState>();
        }
        else
        {
            stateMachine.ChangeState<PlayerIdleState>();
        }
    }

    public void HandleMoving()
    {
        Debug.Log("Player is moving with velocity: " + velocity);
        transform.Translate(velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    void Update()
    {
        stateMachine.Update();
    }
}