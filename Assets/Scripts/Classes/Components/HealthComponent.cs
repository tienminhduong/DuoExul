using TriInspector;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [ReadOnly] [SerializeField] private int currentHealth;

    public UnityAction OnDeath;

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }
}