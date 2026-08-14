using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController charController;
    Animator animator;
    Vector3 targetPos;
    public float Speed = 5f;
    public Vector3 playerVelocity;
    public bool isGrounded;
    public float gravityValue = -9.8f;
    public float jumpHeight = 2f;

    Vector3 lastPos;
    void Start()
    {
        charController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        targetPos = transform.position;
    }

  
    void Update()
    {
        isGrounded = charController.isGrounded;
        lastPos = transform.position;
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
        MovementKeys();
        AnimateChar();
    }
    private void MoveToPoisiton()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
            targetPos = hit.point;
            targetPos.y = 0;
        }
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
    private void MovementKeys()
    {
        float mvRL = Input.GetAxis("Horizontal") * Speed;
        float mvFB = Input.GetAxis("Vertical") * Speed;
        Vector3 movement = new Vector3(mvRL, playerVelocity.y, mvFB);
        if(mvRL != 0 || mvFB != 0) 
        {
            targetPos = transform.position;
            Vector3 lookDirection = new Vector3(mvRL, 0, mvFB);
            transform.forward = lookDirection;
        }
      
        charController.Move(movement * Time.deltaTime);
        
    }
    private void DetectFacing()
    {
        
    }
    private void AnimateChar()
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
