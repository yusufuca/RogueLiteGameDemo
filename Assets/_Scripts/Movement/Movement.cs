using System;
using System.Collections;
using System.Reflection.Metadata;
using TMPro;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public event Action OnDashing;
    public event Action<float> OnMoveSpeedChanged;
    [Header("-----References-----")]
    [SerializeField] public InputReader inputReader;
    public StatManager statManager;
    [SerializeField] public GameManager gm;
    [SerializeField] public Attack attackScript;
    [SerializeField] public CharacterController charController;
    public Animator animator;
    [SerializeField] public TextMeshPro debugText;
    public BaseMovementState currentMovementState;



    [Header("-----Movement AI-----")]
    [SerializeField, Range(300f,1000f)] public float acceleration = 500f;
    public Vector3 playerVelocity;
    [SerializeField] public float movementVelocity;
    [SerializeField] public float currentSpeed = 0;
    [SerializeField] public float dashDuration;
    [SerializeField] public Vector3 targetPos;
    public float gravityValue = -9.8f;
    public bool isGrounded;
    public bool isMoving;
    public bool jumpRequest;
    public bool dashRequest;
    public bool sprintRequest;
    [SerializeField] public bool isSprinting;
    [SerializeField] public Vector3 lastPos;
    [SerializeField] public Vector3 moveDirection;
    public bool isDashing;

    private void Awake()
    {
        if(debugText == null) debugText = gameObject.GetComponentInChildren<TextMeshPro>();
    }

    protected virtual void Start()
    {

        currentMovementState = new IdleState();
        currentMovementState.EnterState(this);

        targetPos = transform.position;
        gm = GameManager.gm;
        statManager = GetComponent<StatManager>();
        charController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        attackScript = GetComponent<Attack>();
    }


    protected virtual void Update()
    {
        currentMovementState.UpdateState(this);
        isGrounded = charController.isGrounded;
        ApplyGravity();
        lastPos = transform.position;
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("VerticalVelocity", playerVelocity.y);
    }
   
    public void StateChanger(BaseMovementState newState)
    {
        currentMovementState.ExitState(this);
        currentMovementState = newState;
        currentMovementState.EnterState(this);
    }
    protected virtual void ApplyGravity()
    {
        if (isGrounded)
        {
            if (playerVelocity.y < -2)
            {
                playerVelocity.y = -2f;
            }
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
    }
    public virtual void CalculateMoveDirection()
    {
        if (attackScript.canTurnMousePos && attackScript.isAttacking) return;
        Vector3 horizantalTargetPos = new Vector3(targetPos.x, 0, targetPos.z);
        Vector3 horizantalPos = new Vector3(transform.position.x, 0, transform.position.z);
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
    public IEnumerator DashRoutine()
    {
        isDashing = true;
        OnDashing?.Invoke();
        animator.SetTrigger("Dash");
        animator.SetBool("isDashing", isDashing);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        animator.SetBool("isDashing", isDashing);
    }
    public void NotifySpeedChange(float speed)
    {
        OnMoveSpeedChanged?.Invoke(speed);
    }
}
