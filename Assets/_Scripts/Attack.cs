using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class Attack : MonoBehaviour
{
    public GameObject weapon; 
    
    protected bool isAttacking = false;
    protected TextMeshPro debugText;
    public bool isTargetLocked;
    Animator animator;
    public Movement movementScript;
    public LayerMask mask;
    public Vector3 targetPos;
    public float overlapSphereRadius = 10;
    public bool canAttackClosest;
    public bool isSelectedWithMouse;
    Collider closestEnemy = null;
    public bool isTimerRunning;
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        debugText = gameObject.GetComponentInChildren<TextMeshPro>();
        
    }

    protected virtual void Update()
    {
        if (weapon != null)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
            {
                weapon.GetComponent<BoxCollider>().enabled = true;
                Debug.Log("PlayerAttacking");
                isAttacking = true;
            }
            else
            {
                //Debug.Log("Buggy");
                weapon.GetComponent<BoxCollider>().enabled = false;
                isAttacking = false;
            }
        }
        else 
        {
            Debug.Log("weapon is null"); 
        }

        if (!isTimerRunning)
        {
            StartCoroutine(CanAttackTimer());
        }
        AttackClosest();   
    }
    

    protected virtual void Attacking()
    {
        
        
        animator.SetTrigger("Attack");

      
     
    }
    protected virtual void AttackClosest() 
    {
        Collider[] nearEnemies = Physics.OverlapSphere(transform.position, overlapSphereRadius, mask);
        
        
        float closestDistance = Mathf.Infinity;
        if (nearEnemies.Length > 0)
        {
            Debug.Log("there is " + nearEnemies.Length + (" enemy"));
            for(int i = 0; i < nearEnemies.Length; i++)
            {
                Collider c = nearEnemies[i];
                float distance = Vector3.Distance(c.transform.position,transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = c;
                    Debug.Log("enemy is " + c.name);
                }
             
            }

        }
        else
        {
            closestEnemy = null;
            Debug.Log("Cant Detect Anyone");
        }
        if (closestEnemy != null && !isTargetLocked && canAttackClosest && !isAttacking && !isSelectedWithMouse)
        {
            Debug.Log("Attack requested to " + closestEnemy.name);
            isTargetLocked = true;
        }
        if (isTargetLocked && !isSelectedWithMouse && closestEnemy != null)
        {
            AttackRequest(closestEnemy);
        }

    }
    protected virtual void AttackRequest(Collider enemy)
    {
        if (enemy != null)
        {
            
            if (isTargetLocked)
            {
                float distance = Vector3.Distance(transform.position, enemy.gameObject.transform.position);
                debugText.text = distance.ToString();

                if (distance < 2f)
                {
                    targetPos = transform.position;

                    Attacking();
                   
                        isTargetLocked = false;
                        isSelectedWithMouse = false;

                    
                }
                else
                {
                    
                    targetPos = enemy.gameObject.transform.position;
                    
                }
            }
        }
        else
        {
            Debug.Log("Enemy is null from AttackRequested");
        }
   
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, overlapSphereRadius);
       
    }
    public IEnumerator CanAttackTimer()
    {
        isTimerRunning = true;
        if (movementScript.isMoving) 
        {
            canAttackClosest = false;
            yield return null;
        }
        else
        {
            if (!canAttackClosest)
            {
                Debug.Log("waiting to attack the closest enemy for 2 sec");
            }
            yield return new WaitForSeconds(2);
            canAttackClosest=true;
        }
        isTimerRunning = false;
    }

}
