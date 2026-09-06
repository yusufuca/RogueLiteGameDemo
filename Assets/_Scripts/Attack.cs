using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.Metadata;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UIElements;

public class Attack : MonoBehaviour, IDamagable
{
    [Header("-----REFERENCES-----")]
    [SerializeField] protected TextMeshPro debugText;
    [SerializeField] protected TextMeshPro damagePopUpText;
    public StatManager statManager;
    public SkillManager skillManager;
    public Animator animator;
    public Movement movementScript;
    public SkinnedMeshRenderer skinnedMeshRenderer;


    public bool isAttacking = false;
    public float comboTime;
    public float initialComboTime = 2;
    public float attackCoolDown = 0.2f;
    public float lastAttackTime;
    public int attackCount;
    public bool isStunned;

    [Header("-----WEAPON-----")]
    public GameObject weapon;
    public TrailRenderer weaponTrail;
    public float weaponDamage;
    public HashSet<IDamagable> hittedEnemies = new HashSet<IDamagable>();
    public LayerMask enemyLayerMask;
    public TextMeshPro attackStateDebugText;
    [Header("-----TARGET & AI LOGICS-----")]
    public Material flashMaterial;
    public Material originalMat;
    public bool canTurnMousePos;
    public Vector3 targetPos;
    public bool requestTargetPos;
    public bool isTargetLocked;
    public float overlapSphereRadius = 10;
    public bool canAttackClosest;
    public bool isSelectedWithMouse;
    public float waitTime;
    public bool attackRequest = false;
    public bool cast1Request = false;
    public bool cast2Request = false;
    public bool hitted;
    public GameObject hitImpactPrefab;
    public float currentStateCurrentTime;
    bool upscaled;

    [SerializeField] private float initialWaitTime = 5;

    public BaseCombatState currentCombatState;
    private void Awake()
    {
        statManager = GetComponent<StatManager>();
        animator = GetComponentInChildren<Animator>();
        movementScript = GetComponent<Movement>();
        skillManager = GetComponentInChildren<SkillManager>();
        debugText = GetComponent<TextMeshPro>();
        weaponDamage = weapon.GetComponent<Weapon>().damage;
        weaponTrail = weapon.GetComponentInChildren<TrailRenderer>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }
    protected virtual void Start()
    {
        waitTime = initialWaitTime;
        comboTime = initialComboTime;
        currentCombatState = new AttackIdleState();
        currentCombatState.EnterState(this);
    }

    protected virtual void Update()
    {
        ComboTimer();
        CalculateComboDamage();
        CurrentAnimationTime();
        currentCombatState.UpdateState(this);
    }
    public void StateChanger(BaseCombatState newState)
    {
        currentCombatState.ExitState(this);
        currentCombatState = newState;
        currentCombatState.EnterState(this);
    }
    public virtual void ComboTimer()
    {
        float timer = Time.deltaTime;
        comboTime -= timer;
        if (attackCount > 0)
        {
            if (comboTime <= 0)
            {
                attackCount = 0;
                comboTime = initialComboTime;
            }
        }
        else
        {
            comboTime = initialComboTime;
        }
        animator.SetFloat("AttackCount", attackCount);
        animator.SetFloat("AttackSpeed", statManager.attackSpeed);
    }


    protected virtual void CalculateComboDamage()
    {
        switch (attackCount)
        {
            case 1:
                statManager.currentDamage = statManager.initialDamage;
                break;
            case 2:
                statManager.currentDamage = statManager.initialDamage * 1.2f;
                break;
            case 3:
                statManager.currentDamage = statManager.initialDamage * 2;
                break;

        }
    }
    public void TakeDamage(float damage)
    {
        if (statManager != null)
        {
            statManager.ApplyDamage(damage);
            hitted = true;
        }
    }

    public void AttackRequested()
    {
        if (!attackRequest)
        {
            attackRequest = true;
            attackCount++;
            if (attackCount > 3)
            {
                attackCount = 1;
            }
            animator.SetFloat("AttackCount", attackCount);
            if (skillManager.mySkills != null)
            {
                skillManager.CastSkill(skillManager.mySkills[2]);
            }
        }
    }
    public void Cast1Requested()
    {
        cast1Request = true;
        skillManager.CastSkill(skillManager.mySkills[0]);
    }
    public void Cast2Requested()
    {
        cast2Request = true;
        skillManager.CastSkill(skillManager.mySkills[1]);
    }
    public Vector3 Mousepos()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
        Vector3 mousepos = (hit.point - transform.position).normalized;
        mousepos.y = 0;
        return mousepos;
    }
    public virtual void CurrentAnimationTime()
    {
        AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(1);
        if (currentStateInfo.IsTag("Attacking"))
        {
            currentStateCurrentTime = currentStateInfo.normalizedTime % 1f;
        }
    }
    public void TriggerHitStop(float duration)
    {
        StartCoroutine(HitStopRoutine(duration));
        Vector3 hitDirection = (transform.position - transform.forward).normalized;
        if (hitImpactPrefab != null)
        {
            Quaternion impactRotation = Quaternion.LookRotation(hitDirection);
            Object.Instantiate(hitImpactPrefab, transform.position, impactRotation);
        }
    }

    private IEnumerator HitStopRoutine(float duration)
    {
       
        animator.speed = 0;
        transform.localScale *= 1.2f;
        upscaled = true;
        skinnedMeshRenderer.sharedMaterial = flashMaterial;
        float originalSpeed = GetComponent<Movement>().currentSpeed;
        yield return new WaitForSecondsRealtime(duration);
        if (animator != null)
        {
            InitializeHitStop();
        }

    }
    public void InitializeHitStop()
    {
        skinnedMeshRenderer.sharedMaterial = originalMat;
        animator.speed = 1f;
        hitted = false;
        if (upscaled) transform.localScale /= 1.2f;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, overlapSphereRadius);
       
    }


}
