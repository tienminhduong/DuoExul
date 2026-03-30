using UnityEngine;

public class PlayerMovementControls : MonoBehaviour
{
    PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        
    }

    
}
