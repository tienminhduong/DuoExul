using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour, IHazard, IObject
{
    [SerializeField] private int damage = 10;
    [SerializeField] private bool isActive = true;
    public int Damage => damage;

    [SerializeField] private ObjectType objectType;


    public bool IsActive => isActive;

    public ObjectType ObjectType => objectType;

    public Rigidbody2D Rigidbody => throw new System.NotImplementedException();

    public int Direction => throw new System.NotImplementedException();

    private void Start()
    {
        var collider = GetComponent<Collider2D>();
        collider.isTrigger = false; // Ensure it's not a trigger so it can interact with the player
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsActive && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Player collided with hazard. Applying damage: {Damage}");
            var changeHealthEvent = new ChangeHealthPlayerEvent(-Damage);
            EventBus<ChangeHealthPlayerEvent>.RaiseEvent(changeHealthEvent);
        }      
    }
}
