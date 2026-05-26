using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class CircleDetection : BaseDetectionTarget
{
    [SerializeField] private float radius = 1f;
    [SerializeField] private LayerMask detectLayerMask;

    private CircleCollider2D rangeDetection;
    private bool isDetecting = true;
    private GameObject objectDetected = null;

    private void Awake()
    {
        // set up the CircleCollider2D as a trigger with the specified radius
        rangeDetection = GetComponent<CircleCollider2D>();
        rangeDetection.isTrigger = true;
        rangeDetection.radius = radius;
    }
    public override bool IsDetected()
    {
        if(!isDetecting || rangeDetection == null)
        {
            return false;
        }
        return rangeDetection.IsTouchingLayers(detectLayerMask);
    }

    public override void SetActive(bool value)     
    {
        isDetecting = value;
    }

    public override GameObject ObjectDetected => objectDetected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & detectLayerMask) != 0)
        {
            objectDetected = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & detectLayerMask) != 0)
        {
            if (objectDetected == collision.gameObject)
            {
                objectDetected = null;
            }
        }
    }
}
