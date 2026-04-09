using UnityEngine;

[RequireComponent(typeof(HealthComponent), typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class TestEnemy : MonoBehaviour, IEntity
{
    [SerializeField] private int baseAttack = 5;
    
    public AnimationController AnimationController { get; private set; }

    public HealthComponent HealthComponent { get; private set; }

    public Rigidbody2D Rigidbody { get; private set; }
    public Collider2D Collider { get; private set; }


    public int BaseAttack => baseAttack;

    public int Direction { get; private set; }

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<BoxCollider2D>();
        AnimationController = new AnimationController(GetComponentInChildren<Animator>());
        HealthComponent = GetComponent<HealthComponent>();
    }

    public void Attack(AttackData attackData)
    {
    }
}