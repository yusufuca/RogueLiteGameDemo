using System;
using UnityEngine;

public class BaseMovementState
{

    public virtual void EnterState(Movement movement)
    {
        
    }
    public virtual void UpdateState(Movement movement)
    {
        movement.CalculateMoveDirection();
        MoveToPoisiton(movement);
        AnimateChar(movement);
        Sprint(movement.isSprinting,movement);
    }
    public virtual void ExitState(Movement movement)
    {

    }

    protected virtual void MoveToPoisiton(Movement movement)
    {
        float targetSpeed = movement.statManager.Speed;
        if (movement.attackScript.hitted) { movement.currentSpeed = targetSpeed / 5; }
        else
        {
            if (movement.currentSpeed != movement.statManager.Speed && movement.moveDirection != Vector3.zero)
            {
                if (movement.isSprinting && !movement.statManager.isExhausted)
                {
                    movement.currentSpeed = Mathf.SmoothDamp(movement.currentSpeed, targetSpeed * movement.statManager.myData.movementStats.runMultiplier
                        , ref movement.movementVelocity, movement.statManager.myData.movementStats.weight / movement.acceleration);
                }
                else if (movement.isDashing)
                {
                    movement.currentSpeed = Mathf.SmoothDamp(movement.currentSpeed, targetSpeed * movement.statManager.myData.movementStats.dashMultiplier
                        , ref movement.movementVelocity, movement.statManager.myData.movementStats.weight / movement.acceleration);
                    movement.NotifySpeedChange(movement.currentSpeed);

                }
                else
                {
                    movement.currentSpeed = Mathf.SmoothDamp(movement.currentSpeed, targetSpeed, ref movement.movementVelocity,
                        movement.statManager.myData.movementStats.weight / movement.acceleration);
                }
                movement.animator.SetFloat("MoveSpeed", movement.currentSpeed);
                movement.NotifySpeedChange(movement.currentSpeed);
            }
            else
            {
                movement.movementVelocity = 0;
                movement.currentSpeed = 0;
                movement.NotifySpeedChange(movement.currentSpeed);
            }
        }


        Vector3 finalDestination = Vector3.zero;
        if (movement.moveDirection != Vector3.zero)
        {

            Vector3 lookDirection;
            if (movement.attackScript.canTurnMousePos && movement.attackScript.attackRequest)
            {
                lookDirection = movement.attackScript.Mousepos();
            }
            else
            {
                lookDirection = new Vector3(movement.moveDirection.x, 0, movement.moveDirection.z);
            }
            if (lookDirection != Vector3.zero)
            {
                movement.transform.forward = lookDirection;
            }
        }

        finalDestination = movement.moveDirection * movement.currentSpeed;
        finalDestination.y = movement.playerVelocity.y;

        movement.charController.Move(finalDestination * Time.deltaTime);

    }
    protected virtual void AnimateChar(Movement movement)
    {
        if (movement.isGrounded && movement.moveDirection != Vector3.zero)
        {
            movement.isMoving = true;
            movement.animator.SetBool("isMoving", true);
        }
        else
        {
            movement.isMoving = false;
            movement.animator.SetBool("isMoving", false);
        }
    }
    public void Sprint(bool isSprinting,Movement movement)
    {
        if (isSprinting == true)
        {
            if (!movement.statManager.isExhausted)
            {
                movement.statManager.currentStamina -= Time.deltaTime * 5;
                if (movement.statManager.currentStamina <= 0)
                {
                    movement.statManager.currentStamina = 0;
                    movement.statManager.isExhausted = true;
                }
            }
            else
            {
                movement.statManager.currentStamina += Time.deltaTime * 5;

                if (movement.statManager.currentStamina >= movement.statManager.maxStamina * 0.20f)
                {
                    movement.statManager.isExhausted = false;
                }
            }
        }
        else
        {
            if (movement.statManager.currentStamina < movement.statManager.maxStamina)
            {
                movement.statManager.currentStamina += Time.deltaTime * 5;

            }
            if (movement.statManager.isExhausted && movement.statManager.currentStamina >= movement.statManager.maxStamina * 0.20f)
            {
                movement.statManager.isExhausted = false;
            }
        }
        movement.statManager.Sprint(isSprinting);
    }

}
