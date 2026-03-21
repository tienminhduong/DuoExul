using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRotator : MonoBehaviour
{
    [SerializeField] private float degreePerSecond = 360f;
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        sprite.transform.Rotate(0, 0, degreePerSecond *  Time.deltaTime);
    }
}
