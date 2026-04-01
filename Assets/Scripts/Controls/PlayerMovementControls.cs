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
        controls.Player.Move.performed += ctx => player.SetDirection(ctx.ReadValue<Vector2>());
        controls.Player.Move.canceled += ctx => player.SetDirection(Vector2.zero);
        controls.Player.Jump.performed += ctx => player.SetJump();
    }

    void OnDisable()
    {
        controls.Disable();
    }
}
