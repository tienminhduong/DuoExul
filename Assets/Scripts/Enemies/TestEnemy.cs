using UnityEngine;

[RequireComponent(typeof(HealthComponent), typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class TestEnemy : MonoBehaviour, IEntity, IDamageable, IObject
{
    public HealthComponent HealthComponent { get; private set; }

    public Rigidbody2D Rigidbody { get; private set; }

    public int Direction { get; private set; }

    [SerializeField] private ObjectType objectType = ObjectType.HardObject;

    public ObjectType ObjectType => objectType;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        HealthComponent = GetComponent<HealthComponent>();
    }

    void OnEnable()
    {
        HealthComponent.OnDeath += OnDead;
    }

    void OnDisable()
    {
        HealthComponent.OnDeath -= OnDead;
    }

    private void OnDead()
    {
        Destroy(gameObject);
    }
}