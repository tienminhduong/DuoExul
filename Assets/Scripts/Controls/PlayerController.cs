using TriInspector;
using UnityEngine;

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


    [Header("Debug readonly")]
    [ReadOnly][SerializeField] private StateMachine stateMachine;

    // For double jumping, reset if touch the ground, -1 when leave the ground
    [ReadOnly][SerializeField] private int viableJumps = 1;


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
        stateMachine.AddState(new PlayerWalkingState(this, moveSpeed));
        stateMachine.AddState(new PlayerJumpState(this, moveSpeed, jumpForce));

        stateMachine.SetState<PlayerIdleState>();
    }

    private void ResetOnTouch()
    {
        viableJumps = maxJumps;
    }

    public void SetDirection(Vector2 newDirection)
    {
        Direction = newDirection.normalized;
        if (Direction.magnitude > 0)
            stateMachine.ChangeState<PlayerWalkingState>();
        else
            stateMachine.ChangeState<PlayerIdleState>();
    }

    public void SetJump()
    {
        if (viableJumps <= 0)
            return;

        viableJumps--;
        stateMachine.ChangeState<PlayerJumpState>();
    }

    public void HandleJumping()
    {

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
    }
}