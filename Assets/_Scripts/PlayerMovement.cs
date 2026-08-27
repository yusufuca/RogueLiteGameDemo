using UnityEngine;

public class PlayerMovement : Movement
{
    Attack playerAttack;
    bool tapped;
    int tapCount;
    float tapInterval = 0.2f;
    float lastTapTime = -1;
    float dashSpeedMultiplier = 3;
    float runMultiplier = 2;
    float originaspeed;
    public float mySpeed;
    public bool isDashing;
    public Vector3 newTargetPos;
    public float stamina;
    protected override void Start()
    {
        Speed = mySpeed;
        originaspeed = Speed;
        currentStamina = stamina;
        playerAttack = GetComponent<Attack>();
        base.Start();
    }

   
    protected override void Update()
    {
       
        
        MovementKeys();
        base.Update();
        Dashing();
        StaminaCheck();
        animator.SetFloat("MoveSpeed", Speed);
    }
    protected override void MoveToPoisiton()
    {
       
        if (Input.GetMouseButtonDown(1))
        {
            playerAttack.isTargetLocked = false;
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
            targetPos = hit.point;
            targetPos.y = 0;
        }
        
        if(playerAttack.isTargetLocked || playerAttack.requestTargetPos) 
        {
            targetPos = playerAttack.targetPos;
            Debug.Log("Player Moving To Enemy");
        }
 
        base.MoveToPoisiton();

    }
    private void MovementKeys()
    {
        // if (isDashing) return;
        //float mvRL = Input.GetAxis("Horizontal") * Speed;
        // float mvFB = Input.GetAxis("Vertical") * Speed;
        Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 movement = new Vector3(inputDir.x * Speed, playerVelocity.y, inputDir.z * Speed);
        if (inputDir != Vector3.zero)
        {
            targetPos = transform.position;
            Vector3 lookDirection = new Vector3(inputDir.x, 0, inputDir.z);
            transform.forward = lookDirection;
            playerAttack.isTargetLocked = false;
        }

        charController.Move(movement * Time.deltaTime);
    

    }
    private void Dashing()
    {
        float dashSpeedMultiplier = 3;
        if (CheckDoubleTapped(KeyCode.W))
        {
           
            // Roll Anim
            //Debug.LogWarning("DoubleTappedW");
            animator.SetTrigger("Dash");
            isDashing = true;
            animator.SetBool("isDashing", isDashing);
            


        }
        if (isDashing)
        {


            if (Vector3.Distance(newTargetPos, transform.position) > 0.6f)
            {
                targetPos = newTargetPos;
                targetPos.y = transform.position.y;
                Speed = originaspeed * dashSpeedMultiplier;
            }
            else
            {
                targetPos = transform.position;
            
                Speed = originaspeed;

                isDashing = false;
                animator.SetBool("isDashing", isDashing);
            }
        }
    }
    private bool CheckDoubleTapped(KeyCode x)
    {
      
        if (Input.GetKeyDown(x))
        {
            if (Time.time - lastTapTime < tapInterval)
            {
                lastTapTime = -1f;
                newTargetPos = transform.position + transform.forward * 20;
                return true;
            }
            else
            {
                lastTapTime = Time.time;
            }
        }
        return false;
    }

    private void StaminaCheck()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            
            Speed = originaspeed * runMultiplier;
            currentStamina -= Time.deltaTime * 5;
          

        }
        else
        {
            Speed = originaspeed;
            if (currentStamina < maxStamina)
            {
                currentStamina += Time.deltaTime * 5;
              
            }
        }
    }
}
