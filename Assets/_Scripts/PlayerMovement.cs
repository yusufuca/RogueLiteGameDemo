using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : Movement
{
    public event Action OnDashing;

    Attack playerAttack;
    float tapInterval = 0.2f;
    float lastTapTime = -1;
    [SerializeField] private float dashDuration;
    public Vector3 newTargetPos;
    private KeyCode lastPressedKey = KeyCode.None;
    protected override void Start()
    {
        
        
        playerAttack = GetComponent<Attack>();
        base.Start();
    }

   
    protected override void Update()
    {
        base.Update();
        Jump();
        Dashing();
    }
    protected override void CalculateMoveDirection()
    {
        Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

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
    private void Jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            playerVelocity.y = Mathf.Sqrt(statManager.jumpHeight * -2f * gravityValue);
            animator.SetTrigger("Jump");

        }

    }

    private void Dashing()
    {
        KeyCode[] keyCodes = {KeyCode.W,KeyCode.A,KeyCode.S,KeyCode.D};
        if (!isDashing && statManager.currentStamina > statManager.dashStaminaCost)
        {
            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (CheckDoubleTapped(keyCodes[i]))
                {
                    OnDashing?.Invoke();
                    StartCoroutine(DashRoutine());
                }
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                OnDashing?.Invoke();
                StartCoroutine(DashRoutine());
            }
        }
    }
    private IEnumerator DashRoutine()
    {
        isDashing = true;
        animator.SetTrigger("Dash");
        animator.SetBool("isDashing", isDashing);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        animator.SetBool("isDashing", isDashing );
    }
    private bool CheckDoubleTapped(KeyCode x)
    {
      
        if (Input.GetKeyDown(x))
        {
            if (lastPressedKey == x && Time.time - lastTapTime < tapInterval)
            {
                lastTapTime = -1f;
                lastPressedKey = KeyCode.None;
                return true;
            }
            else
            {
                lastPressedKey = x;
                lastTapTime = Time.time;
            }
        }
        return false;
    }



}
