using System.Reflection.Metadata;
using TMPro;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("-----References-----")]
    public Entity_Types myData;
    [SerializeField] protected StatManager statManager;
    [SerializeField] protected GameManager gm;
    [SerializeField] protected Attack attackScript;
    [SerializeField] protected CharacterController charController;
    [SerializeField] protected Animator animator;
    [SerializeField] protected TextMeshPro debugText;




    [Header("-----Movement AI-----")]
    [SerializeField] protected Vector3 playerVelocity;
    [SerializeField] protected Vector3 targetPos;
    [SerializeField] protected float gravityValue = -9.8f;
    [SerializeField] protected bool isGrounded;
    [SerializeField] public bool isMoving;
    [SerializeField] protected Vector3 lastPos;

    private void Awake()
    {
      
        debugText = gameObject.GetComponentInChildren<TextMeshPro>();
    }

    protected virtual void Start()
    {

      
        targetPos = transform.position;
        gm = GameManager.gm;
        statManager = GetComponent<StatManager>();
        charController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        attackScript = GetComponent<Attack>();
    }


    protected virtual void Update()
    {
        if (attackScript.isStunned) return;
        if (gm.isDebugTextOpen && debugText != null)
        {
            
            //debugText.text = ("Speed: " + Speed);
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
            playerVelocity.y = Mathf.Sqrt(statManager.jumpHeight * -2f * gravityValue);
            animator.SetTrigger("Jump");
           
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        MoveToPoisiton();
       
        AnimateChar();
        lastPos = transform.position;
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("VerticalVelocity", playerVelocity.y);
    }
    protected virtual void MoveToPoisiton()
    {
  
        float distance = Vector3.Distance(targetPos, transform.position);
        Vector3 finalDestination = Vector3.zero;
        if (distance > 0.5f)
        {
            Vector3 direction = (targetPos - transform.position).normalized;
            finalDestination = direction * statManager.Speed;
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
        if (lastPos.x != transform.position.x || lastPos.z != transform.position.z) 
        {
            isMoving = true;
            animator.SetBool("isMoving" , true);
        }
        else
        {
            isMoving = false;
            animator.SetBool("isMoving", false);
        }
    }
}
