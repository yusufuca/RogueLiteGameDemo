using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, InputControl.IPlayerActions
{
    public event Action OnMovePerformed;
    public event Action <bool> OnSprintPerformed;
    public event Action OnJumpPerformed;
    public event Action OnDashPerformed;
    public event Action OnAttackPerformed;
    public event Action OnSkill1Performed;
    public event Action OnSkill2Performed;
    public event Action OnPerkMenuPerformed;
    public event Action OnStatMenuPerformed;

    public Vector2 MovementInput { get; private set; }

    private InputControl controls;

    private void Awake()
    {
        controls = new InputControl();
        controls.Player.SetCallbacks(this);
    }
    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnJumpPerformed?.Invoke();
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnDashPerformed?.Invoke();
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnAttackPerformed?.Invoke();
    }

    public void OnSkill_1_Input(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnSkill1Performed?.Invoke();
    }

    public void OnSkill_2_Input(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnSkill2Performed?.Invoke();
    }
    public void OnPerkMenuInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnPerkMenuPerformed?.Invoke();
    }

    public void OnStatMenuInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnStatMenuPerformed?.Invoke();
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnSprintPerformed?.Invoke(true);
        if(context.canceled)
            OnSprintPerformed?.Invoke(false);
    }

}
