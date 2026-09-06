using Unity.VisualScripting;
using UnityEngine;

public class DashState : BaseMovementState
{
    private InputReader inputReaderScript;
    private Movement movementScript;
    public override void EnterState(Movement movement)
    {
        base.EnterState(movement);
        if (movement.debugText != null) movement.debugText.text = "Dashing";

        if (!movement.isDashing && movement.statManager.currentStamina > movement.statManager.dashStaminaCost)
        {
            movement.StartCoroutine(movement.DashRoutine());
            movement.dashRequest = false;
        }
    }
    public override void UpdateState(Movement movement)
    {
        base.UpdateState(movement);
        if (!movement.isDashing)
        {
            movement.StateChanger(new MoveState());
        }
    }
    public override void ExitState(Movement movement)
    {
        base.ExitState(movement);
    }


}
