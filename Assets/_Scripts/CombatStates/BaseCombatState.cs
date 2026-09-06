using Unity.VisualScripting;
using UnityEngine;

public class BaseCombatState 
{
    public Vector3 attackPosition;
    
    public virtual void EnterState(Attack attackScript)
    {
        if (attackScript.attackStateDebugText != null) attackScript.attackStateDebugText.text = attackScript.currentCombatState.ToString();
    }
    public virtual void UpdateState(Attack attackScript)
    {
      
    }
    public virtual void ExitState(Attack attackScript)
    {
        
    }
    public virtual void AttackRequest(Collider enemy,Attack attackScript)
    {

        if (enemy != null)
        {
            Debug.Log(attackScript.gameObject.name + " Attack requested to " + enemy.gameObject.name);
            if (attackScript.isTargetLocked)
            {
                float distance = Vector3.Distance(attackScript.transform.position, enemy.gameObject.transform.position);
                if (distance < 2f)
                {
                    attackScript.requestTargetPos = false;
                    attackScript.targetPos = attackScript.transform.position;

                    Attacking(attackScript);

                    attackScript.isTargetLocked = false;
                }
                else
                {
                    if (enemy.gameObject == null || attackScript.movementScript.isMoving)
                    {
                        attackScript.targetPos = attackScript.transform.position;
                    }
                    attackScript.requestTargetPos = true;
                    attackScript.targetPos = enemy.gameObject.transform.position;
                }
            }
        }
        else
        {
            Debug.Log("Enemy is null from AttackRequested");
        }
    }
    public virtual void Attacking(Attack attackScript)
    {
        if (attackScript.isAttacking) return;
        attackScript.lastAttackTime = Time.time;
        attackScript.comboTime = attackScript.initialComboTime;


    }
    public virtual void WeaponColliderControl(Attack attackScript)
    {
        if (attackScript.weapon != null)
        {
            if (attackScript.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attacking"))
            {
                attackScript.weapon.GetComponent<BoxCollider>().enabled = true;
                Debug.Log("PlayerAttacking");
                attackScript.isAttacking = true;
            }
            else
            {
                attackScript.weapon.GetComponent<BoxCollider>().enabled = false;
                attackScript.isAttacking = false;
                attackScript.weapon.GetComponent<Weapon>().damageGiven = false;
            }
        }
        else
        {
            Debug.Log("weapon is null");
        }
    }
    public virtual void TurnToAttackPosition(Attack attackScript)
    {
        if (attackScript.canTurnMousePos)
        {
            attackScript.movementScript.moveDirection = attackScript.Mousepos();
            attackScript.transform.forward = attackScript.Mousepos();
        }

    }
    public virtual void CreateAndCheckTheOverlapBox(Attack attackScript)
    {
        float forwardOffset = 1.2f;
        Vector3 center = attackScript.transform.position + (attackScript.transform.forward * forwardOffset);
        Vector3 halfExtents = new Vector3(0.75f, 0.5f, 0.75f);
        Quaternion rotation = attackScript.transform.rotation;
        Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, rotation, attackScript.enemyLayerMask);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if(hitColliders[i] != null) 
            { 
                if (hitColliders[i].TryGetComponent<IDamagable>(out IDamagable target))
                {
                    if (!attackScript.hittedEnemies.Contains(target))
                    {
                        GameObject enemyObject = hitColliders[i].gameObject;
                        Animator enemyAnimator = hitColliders[i].GetComponentInChildren<Animator>();
                        target.TakeDamage(attackScript.statManager.currentDamage + attackScript.weaponDamage);
                        attackScript.hittedEnemies.Add(target);
                    }
                }
            }
        }
    }
   
}
