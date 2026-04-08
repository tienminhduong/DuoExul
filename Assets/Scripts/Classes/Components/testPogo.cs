using UnityEngine;

public class testPogo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventBus<PogoEvent>.Register(OnPogoEvent);
    }

    private void OnPogoEvent(PogoEvent pogoEvent)
    {
        Debug.Log($"Pogo event triggered! Source: {pogoEvent.source.name}, Target: {pogoEvent.target.name}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
