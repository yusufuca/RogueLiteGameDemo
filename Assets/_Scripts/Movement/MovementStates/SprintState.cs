using UnityEngine;

public class SprintState : BaseMovementState
{
    public override void EnterState(Movement movement)
    {
        base.EnterState(movement);

        if (movement.debugText != null) movement.debugText.text = "Sprinting";
      
    }
    public override void UpdateState(Movement movement)
    {
        base.UpdateState(movement);
        if (movement.dashRequest)
        {
            movement.StateChanger(new DashState());
        }
        if (!movement.sprintRequest || movement.statManager.isExhausted)
        {
            movement.StateChanger(new MoveState());
        }


    }
    public override void ExitState(Movement movement)
    {
        base.ExitState(movement);
    }
}
