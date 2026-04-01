using TriInspector;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    public Rigidbody2D Rigidbody { get; private set; }
    public BoxCollider2D Collider { get; private set; }
    public Vector2 Direction { get; private set; }


    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int maxJumps = 1;

    [SerializeField] private LayerMask groundLayer;


    [Header("Debug readonly")]
    [ReadOnly][SerializeField] private StateMachine stateMachine;

    // For double jumping, reset if touch the ground, -1 when leave the ground
    [ReadOnly][SerializeField] private int viableJumps = 1;


    // Actions
    public UnityAction OnGrounded;


    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<BoxCollider2D>();

        SetupStateMachine();
    }

    private void SetupStateMachine()
    {
        stateMachine = new StateMachine();

        stateMachine.AddState(new PlayerIdleState(this));
        stateMachine.AddState(new PlayerWalkingState(this));
        stateMachine.AddState(new PlayerJumpState(this, jumpForce));
        stateMachine.AddState(new PlayerFallState(this));

        stateMachine.SetState<PlayerIdleState>();
    }

    private void ResetOnTouch()
    {
        viableJumps = maxJumps;
    }

    public void ChangeState<T>() where T : IState
    {
        stateMachine.ChangeState<T>();
    }

    public void SetDirection(Vector2 newDirection)
    {
        Direction = newDirection.normalized;
        if (Direction.magnitude > 0)
            stateMachine.ChangeState<PlayerWalkingState>();
        else if (stateMachine.IsInState<PlayerWalkingState>())
            stateMachine.ChangeState<PlayerIdleState>();
    }

    public void SetJump()
    {
        if (viableJumps <= 0)
            return;

        viableJumps--;
        stateMachine.ChangeState<PlayerJumpState>();
    }

    public void SetFall()
    {
        if (stateMachine.IsInState<PlayerJumpState>())
            stateMachine.ChangeState<PlayerFallState>();
    }

    // called in fixed update
    public void UpdateMoving()
    {
        transform.Translate(moveSpeed * Time.fixedDeltaTime * Direction);
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    void Update()
    {
        stateMachine.Update();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ResetOnTouch();

        if (groundLayer.Contains(collision.gameObject))
            OnGrounded?.Invoke();
    }
}