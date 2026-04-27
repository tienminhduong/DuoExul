using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthComponent))]
public abstract class BaseEnemyController : MonoBehaviour, IAttacker, IDamageable
{
    // This variable is used to prevent the enemy from flipping too frequently,
    // which can cause visual glitches and unnatural behavior
    private float lastTimeFlip = -1;
    protected bool isDead = false;
    [SerializeField] private float minTimeBetweenFlips = 1f; // Minimum time in seconds between flips

    // Components
    protected Rigidbody2D _rigidbody;
    protected HealthComponent _healthComponent;

    // Helper classes
    protected AnimationController _animationController;
    protected StateMachine _stateMachine;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _healthComponent = GetComponent<HealthComponent>();
        _animationController = new AnimationController(GetComponentInChildren<Animator>());
    }

    public int BaseAttack => 0; // this property may not use, but we need it to implement IAttacker

    public AnimationController AnimationController => _animationController;

    public Rigidbody2D Rigidbody => _rigidbody;

    public HealthComponent HealthComponent => _healthComponent;

    virtual public void Attack(AttackData attackData) { }

    // This method can be overridden by child classes to implement different vision logic,
    // for example, some enemies can see the player from far away,
    // some can only see when the player is close,
    // some can see through walls, some can't see through walls, etc.
    virtual public bool CanSeePlayer() { return false; }
    virtual public GameObject GetPlayer() { return null; }

    public void Move(Vector2 direction, float speed=5)
    {
        // 1. Process the direction to determine the facing direction of the enemy
        if(Time.time - lastTimeFlip < minTimeBetweenFlips)
        {
            // If the time since the last flip is less than the minimum time between flips, do not flip
        }
        else if(direction.x != transform.localScale.x)
        {
            Flip();
        }    
        // 2. Calculating the target position based on the direction and speed
        Vector2 moveAmount = direction.Abs() * speed * Time.fixedDeltaTime * transform.localScale.x;
        Vector2 targetPosition = _rigidbody.position + moveAmount;

        // 3. Move the enemy to the target position using Rigidbody2D.MovePosition for smooth
        // movement and proper collision handling
        _rigidbody.MovePosition(targetPosition);
    }

    private void Flip()
    {
        // Flip the enemy by changing the local scale's x value
        Vector3 newScale = transform.localScale;
        newScale.x *= -1; // Flip the x scale to change facing direction
        transform.localScale = newScale;
        // Update the last time the enemy flipped
        lastTimeFlip = Time.time;
    }    

    public void ChangeState<T>() where T : IState
    {
        if(_stateMachine == null)
        {
            Debug.LogWarning("State machine is not initialized.");
            return;
        }
        _stateMachine.ChangeState<T>();
    }

    protected virtual void Die()
    {
        if (isDead)
            return;
        isDead = true;
    }

    public void DestroyAfter(float second = 0)
    {
        Debug.Log("Destroying enemy: " + gameObject.name);
        Destroy(gameObject, second);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
