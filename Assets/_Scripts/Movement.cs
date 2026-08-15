using TMPro;
using UnityEngine;

public class Movement : MonoBehaviour
{
    GameManager gm;
    protected CharacterController charController;
    protected Animator animator;
    protected Vector3 targetPos;
    protected float Speed = 5f;
    protected Vector3 playerVelocity;
    protected bool isGrounded;
    protected float gravityValue = -9.8f;
    protected float jumpHeight = 2f;
    protected TextMeshPro debugText;
    protected Vector3 lastPos;

    private void Awake()
    {
      
        debugText = gameObject.GetComponentInChildren<TextMeshPro>();
    }

    protected virtual void Start()
    {
        gm = GameManager.gm;
        charController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        targetPos = transform.position;
    }


    protected virtual void Update()
    {
        if (gm.isDebugTextOpen && debugText != null)
        {
            
            debugText.text = ("Speed: " + Speed);
        }
        isGrounded = charController.isGrounded;
        
        if (isGrounded)
        {
            if(playerVelocity.y < -2)
            {
                playerVelocity.y = -2f;
            } 
        }
       
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        MoveToPoisiton();
       
        AnimateChar();
        lastPos = transform.position;
    }
    protected virtual void MoveToPoisiton()
    {
  
        float distance = Vector3.Distance(targetPos, transform.position);
        Vector3 finalDestination = Vector3.zero;
        if (distance > 0.5f)
        {
            Vector3 direction = (targetPos - transform.position).normalized;
            finalDestination = direction * Speed;
            Vector3 lookDirection = new Vector3(direction.x, 0, direction.z);
            if (lookDirection != Vector3.zero)
            {
                transform.forward = lookDirection;
            }
        }
        

        charController.Move(finalDestination *  Time.deltaTime);
    }
    
    private void DetectFacing()
    {
        
    }
    protected virtual void AnimateChar()
    {
        if (lastPos != transform.position) 
        {
            animator.SetBool("isMoving" , true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
}
