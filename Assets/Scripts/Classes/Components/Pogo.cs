using System.Runtime.Versioning;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pogo : MonoBehaviour, IPogoable
{
    public bool CanPogo(GameObject source)
    {
        if (source == null)
            return false;

        if (source.CompareTag("Player"))
            return true;
        return false;
    }

    public void OnPogo()
    {
        // Add effects here
        Debug.Log($"{gameObject.name} was pogoed!");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CanPogo(collision.gameObject))
        {
            var pogoEvent = new PogoEvent(collision.gameObject, this.gameObject);
            EventBus<PogoEvent>.RaiseEvent(pogoEvent);
            OnPogo();
        }
    }
}

public class PogoEvent: IEvent
{
    public GameObject source;
    public GameObject target;
    public PogoEvent(GameObject source, GameObject target)
    {
        this.source = source;
        this.target = target;
    }
}