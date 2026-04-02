using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EffectHealthPlayer : MonoBehaviour
{
    [SerializeField]
    private float _healthChange = 10f;
    [SerializeField]
    private float _timeToEffect = 3.0f;

    private float _timeInEffect = 0f;

    private void Start()
    {
        var collider = GetComponent<Collider2D>();
        if (!collider.isTrigger)
        {
            Debug.LogWarning("Collider on EffectHealthPlayer should be set as Trigger. Setting it automatically.");
            collider.isTrigger = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log($"Player entered effect area. Starting to apply health change of {_healthChange} after {_timeToEffect} seconds.");
            _timeInEffect = 0f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player"))
        {
            _timeToEffect = 0f;
            return;
        }
        Debug.Log($"Player is in effect area. Time in effect: {_timeInEffect:F2} seconds.");
        _timeInEffect += Time.deltaTime;
        if(_timeInEffect >= _timeToEffect)
        {
            var healthChangeEvent = new ChangeHealthPlayerEvent(_healthChange);
            Debug.Log($"Raising event to change player health by {_healthChange}");
            EventBus<ChangeHealthPlayerEvent>.RaiseEvent(healthChangeEvent);
            _timeInEffect = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Player exited effect area. Resetting timer.");
            _timeInEffect = 0f;
        }
    }
}
