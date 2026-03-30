using UnityEngine;

public class PlayerMovementControls : MonoBehaviour
{
    PlayerControls controls;
    [SerializeField] PlayerController player;

    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += ctx => player.SetVelocity(ctx.ReadValue<Vector2>());
        controls.Player.Move.canceled += ctx => player.SetVelocity(Vector2.zero);
    }

    void OnDisable()
    {
        controls.Disable();
    }
}
