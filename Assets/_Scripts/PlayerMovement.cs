using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController charController;
    Vector3 targetPos;
    public float Speed = 5f;
    public Vector3 playerVelocity;
    public bool isGrounded;
    public float gravityValue = -9.8f;
    public float jumpHeight = 2f;
    void Start()
    {
        charController = GetComponent<CharacterController>();
        targetPos = transform.position;
    }

  
    void Update()
    {
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
        MovementKeys();
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
        }
        

        charController.Move(finalDestination *  Time.deltaTime);
    }
    private void MovementKeys()
    {
        float mvRL = Input.GetAxis("Horizontal") * Speed;
        float mvFB = Input.GetAxis("Vertical") * Speed;
        Vector3 movement = new Vector3(mvRL, playerVelocity.y, mvFB);
        if(mvRL != 0 || mvFB != 0) 
        { targetPos = transform.position; }
        charController.Move(movement * Time.deltaTime);
    }
}
