using System;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class TouchHandler : MonoBehaviour
{
    private PlayerInputSystem inputActions;
    public bool IsTouching { get; private set; }
    public static event Action<Vector2> OnPointerPressed;
    public static event Action<bool> OnTouchingChanged;
    private void Awake()
    {
        inputActions = new PlayerInputSystem();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.TouchPress.started += OnTouchStarted;
        inputActions.Player.TouchPress.canceled += OnTouchEnded;
    }

    private void OnDisable()
    {
        inputActions.Player.TouchPress.started -= OnTouchStarted;
        inputActions.Player.TouchPress.canceled -= OnTouchEnded;
        inputActions.Player.Disable();
    }

    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        IsTouching = true;
        OnTouchingChanged?.Invoke(true);
        Vector2 pos = inputActions.Player.TouchPosition.ReadValue<Vector2>();
        OnPointerPressed?.Invoke(pos);
    }

    private void OnTouchEnded(InputAction.CallbackContext context)
    {
        IsTouching = false;
        OnTouchingChanged?.Invoke(false);
    }
    public Vector2 GetPointerPosition() =>
        inputActions.Player.TouchPosition.ReadValue<Vector2>();
}