using UnityEngine;

public class JumpState : BaseMovementState
{
    public override void EnterState(Movement movement)
    {
        base.EnterState(movement);
        if (movement.debugText != null) movement.debugText.text = "Jumping";
        if (movement.isGrounded)
        {
            movement.playerVelocity.y = Mathf.Sqrt(movement.statManager.jumpHeight * -2f * movement.gravityValue);
            movement.animator.SetTrigger("Jump");
            movement.jumpRequest = false;
        }

       
    }

    public override void UpdateState(Movement movement)
    {
        base.UpdateState(movement);
        if (movement.isGrounded && movement.playerVelocity.y <= 0)
        {
            if (movement.isMoving) 
            {
                movement.StateChanger(new MoveState());
            }
            else
            {
                movement.StateChanger(new IdleState());
            }
        }

    }

    public override void ExitState(Movement movement)
    {
        base.ExitState(movement);
    }
}
