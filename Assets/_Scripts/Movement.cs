using System;
using System.Reflection.Metadata;
using TMPro;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public event Action<float> OnMoveSpeedChanged;


    [Header("-----References-----")]
    public Entity_Types myData;
    [SerializeField] protected StatManager statManager;
    [SerializeField] protected GameManager gm;
    [SerializeField] protected Attack attackScript;
    [SerializeField] protected CharacterController charController;
    [SerializeField] protected Animator animator;
    [SerializeField] protected TextMeshPro debugText;




    [Header("-----Movement AI-----")]
    [SerializeField, Range(300f,1000f)] protected float acceleration = 500f;
    [SerializeField] protected Vector3 playerVelocity;
    [SerializeField] protected float movementVelocity;
    [SerializeField] protected float currentSpeed = 0;
    [SerializeField] protected Vector3 targetPos;
    [SerializeField] protected float gravityValue = -9.8f;
    [SerializeField] protected bool isGrounded;
    [SerializeField] public bool isMoving;
    [SerializeField] protected bool isSprinting;
    [SerializeField] protected Vector3 lastPos;
    [SerializeField] protected Vector3 moveDirection;
    [SerializeField] protected bool isDashing;

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
        isSprinting = Input.GetKey(KeyCode.LeftShift);
        statManager.Sprint(isSprinting);
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

        playerVelocity.y += gravityValue * Time.deltaTime;
        CalculateMoveDirection();
        MoveToPoisiton();
       
        AnimateChar();
        lastPos = transform.position;
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("VerticalVelocity", playerVelocity.y);
    }
    protected virtual void CalculateMoveDirection()
    {
        Vector3 horizantalTargetPos = new Vector3(targetPos.x,0,targetPos.z);
        Vector3 horizantalPos = new Vector3(transform.position.x,0,transform.position.z);
        float distance = Vector3.Distance(horizantalPos, horizantalTargetPos);
        if (distance > 0.5f) 
        {
            Vector3 direction = (targetPos - transform.position).normalized;
            direction.y = 0;
            moveDirection = direction;
        }
        else 
        {
            moveDirection = Vector3.zero;
        }
    }
    protected virtual void MoveToPoisiton()
    {
        float targetSpeed = statManager.Speed;
        if (currentSpeed != statManager.Speed && moveDirection != Vector3.zero)
        {
            if (isSprinting && !statManager.isExhausted)
            {
                currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed * statManager.myData.movementStats.runMultiplier
                    , ref movementVelocity, statManager.myData.movementStats.weight / acceleration);
            }
            else if (isDashing)
            {
                currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed * statManager.myData.movementStats.dashMultiplier
                    , ref movementVelocity, statManager.myData.movementStats.weight / acceleration);
                OnMoveSpeedChanged?.Invoke(currentSpeed);

            }
            else
            {
                currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed ,ref movementVelocity, statManager.myData.movementStats.weight / acceleration);
            }
                animator.SetFloat("MoveSpeed", currentSpeed);
                OnMoveSpeedChanged?.Invoke(currentSpeed);
        }
        else
        {
            movementVelocity = 0;
            currentSpeed = 0;
            OnMoveSpeedChanged?.Invoke(currentSpeed);
        }


            Vector3 finalDestination = Vector3.zero;
        if (moveDirection != Vector3.zero)
        {
            Vector3 lookDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
            if (lookDirection != Vector3.zero)
            {
                transform.forward = lookDirection;
            }
        }

        finalDestination = moveDirection * currentSpeed;
        finalDestination.y = playerVelocity.y;  

        charController.Move(finalDestination * Time.deltaTime);
        
    }
    protected virtual void AnimateChar()
    {
        if (isGrounded && moveDirection != Vector3.zero) 
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
