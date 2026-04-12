using UnityEngine;

public class DecorateObject : MonoBehaviour
{
    [SerializeField] private VfxType type;
    [SerializeField] LayerMask effectedLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(((1 << collision.gameObject.layer) & effectedLayer) != 0)
        {
            Vector3 contactPoint = collision.ClosestPoint(transform.position);
            Quaternion rotation = Quaternion.identity; // You can customize this based on the collision normal if needed
            VFXSystem.Instance?.Play(type, transform.position, rotation);
            Destroy(gameObject);
        }
    }
}
