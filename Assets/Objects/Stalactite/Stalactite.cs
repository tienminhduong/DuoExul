using System.Collections;
using UnityEngine;

public class Stalactite : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private IDetectionTarget detectionTarget;
    private Rigidbody2D rb;
    private bool isFalling = false;
    void Start()
    {
        detectionTarget = GetComponent<IDetectionTarget>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Initially disable gravity
    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling)
            return;

        if(detectionTarget.IsDetected())
        {
            // Add falling logic here
            Debug.Log("Stalactite is falling!");
            StartCoroutine(Falling());
        }
    }

    IEnumerator Falling()
    {
        isFalling = true;
        // Add falling animation or logic here
        yield return new WaitForSeconds(1f); // Simulate falling time
        rb.gravityScale = 2; // Enable gravity to make the stalactite fall
    }
}
