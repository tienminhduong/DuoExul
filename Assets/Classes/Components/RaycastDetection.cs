using UnityEngine;

public class RaycastDetection : MonoBehaviour, IDetectionTarget
{
    [SerializeField] private LayerMask detectLayerMask;
    [SerializeField] private Vector2 direction = Vector2.down;
    [SerializeField] private float distance = Mathf.Infinity;
    [SerializeField] private bool isActive = true;
    public bool IsDetected()
    {
        if (!isActive) return false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, detectLayerMask);
        if(hit.collider != null)
        {
            Debug.DrawRay(transform.position, direction * distance, Color.red);
            return true;
        }    
        Debug.DrawRay(transform.position, direction * distance, Color.green);
        return false;
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }
}
