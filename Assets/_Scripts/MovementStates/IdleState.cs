using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class IdleState : BaseMovementState
{

    public override void EnterState(Movement movement)
    {
        base.EnterState(movement);
        if(movement.debugText != null) movement.debugText.text = "Idling";
    }

    public override void UpdateState(Movement movement)
    {
        base.UpdateState(movement);
        if (movement.isGrounded)
        {
            if (movement.isMoving && !movement.jumpRequest)
            {
                movement.StateChanger(new MoveState());
            }
            if(movement.jumpRequest)
            {
                movement.StateChanger(new JumpState());
            }

        }
    }


    public override void ExitState(Movement movement)
    {
        base.ExitState(movement);
    }
}
