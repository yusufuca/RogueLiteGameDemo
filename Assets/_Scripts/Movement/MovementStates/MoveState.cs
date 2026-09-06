using UnityEngine;

public class MoveState : BaseMovementState
{
    public override void EnterState(Movement movement)
    {
        base.EnterState(movement);
        if (movement.debugText != null) movement.debugText.text = "Moving";
        
    }

    public override void UpdateState(Movement movement)
    {
        base.UpdateState(movement);
        if (!movement.jumpRequest && !movement.dashRequest && !movement.isMoving)
        {
            movement.StateChanger(new IdleState());
        }
        if (movement.jumpRequest)
        {
            movement.StateChanger(new JumpState());
        }
        if (movement.dashRequest)
        {
            movement.StateChanger(new DashState());
        }
        if (movement.sprintRequest && !movement.isDashing && !movement.statManager.isExhausted) 
        {
            movement.StateChanger(new SprintState());
        }

    }

    public override void ExitState(Movement movement)
    {
        base.ExitState(movement);
    }



}
