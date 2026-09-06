using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : Movement
{
    
    Attack playerAttack;
    public Vector3 newTargetPos;
    private void OnEnable()
    {
        inputReader.OnJumpPerformed += Jump;
        inputReader.OnDashPerformed += Dashing;
        inputReader.OnSprintPerformed += Sprint;
    }
    private void OnDisable()
    {
        inputReader.OnJumpPerformed -= Jump;
        inputReader.OnDashPerformed -= Dashing;
        inputReader.OnSprintPerformed -= Sprint;
    }
    protected override void Start()
    {
        playerAttack = GetComponent<Attack>();
        base.Start();
    }
    private void Jump()
    {
        jumpRequest = true;
    }
    private void Dashing()
    {
        if(currentMovementState is MoveState || currentMovementState is SprintState)
        {
            if (statManager.currentStamina > statManager.dashStaminaCost)
            {
                dashRequest = true;
            }
        }
    }
    private void Sprint(bool sprintRequested)
    {
        if (currentMovementState is not DashState)
        {
            sprintRequest = sprintRequested;
            isSprinting = sprintRequested;
        }
    }

    public override void CalculateMoveDirection()
    {
        Vector3 inputDir = new Vector3(inputReader.MovementInput.x, 0, inputReader.MovementInput.y).normalized;

        if (inputDir != Vector3.zero)
        {
            targetPos = transform.position;
            moveDirection = inputDir;
            playerAttack.isTargetLocked = false;
            return;
        }
        else
        {
            moveDirection = Vector3.zero;
        }
        if (Input.GetMouseButtonDown(1))
        {

            playerAttack.isTargetLocked = false;
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
            targetPos = hit.point;
            targetPos.y = 0;
        }
        if (playerAttack.isTargetLocked || playerAttack.requestTargetPos)
        {
            targetPos = playerAttack.targetPos;
            Debug.Log("Player Moving To Enemy");
        }
        base.CalculateMoveDirection();
    }
}
