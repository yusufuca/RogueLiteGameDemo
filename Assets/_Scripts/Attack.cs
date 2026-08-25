using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class Attack : MonoBehaviour
{
    public GameObject weapon; 
    
    protected bool isAttacking = false;
    protected TextMeshPro debugText;
    protected TextMeshPro damagePopUpText;
    public bool isTargetLocked;
    Animator animator;
    public Movement movementScript;
    public LayerMask mask;
    public Vector3 targetPos;
    public float overlapSphereRadius = 10;
    public bool canAttackClosest;
    public bool isSelectedWithMouse;
    Collider closestEnemy = null;
    public bool requestTargetPos;
    public float currentHP;
    public float maxHP;
    public float currentDamage;
    public float waitTime;
    private float initialWaitTime = 5;

    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        movementScript = GetComponent<Movement>();
        Transform foundDebugText = transform.Find("DebugText");
        if (foundDebugText != null)
        {
            debugText = foundDebugText.GetComponent<TextMeshPro>();
        }
        else Debug.Log("Cant Find Debug Text" + gameObject.name);

        Transform foundDamagePopUpText = transform.Find("DamagePopUp");
        if (foundDamagePopUpText != null)
        {
            damagePopUpText = foundDamagePopUpText.GetComponent<TextMeshPro>();
        }
        else Debug.Log("Cant Find Damage PopUp Text" + gameObject.name);
        waitTime = initialWaitTime;
        
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

   
        AttackClosest();   
    }
    

    protected virtual void Attacking()
    {
        
        
        animator.SetTrigger("Attack");

      
     
    }
    public void TakeDamage(float damage)
    {

        currentHP -= damage;
        if (damagePopUpText != null)
        {
            damagePopUpText.text = ("Current Hp: " + currentHP + ("Damage Taken: ") + damage);
        }
    }
    protected virtual void AttackClosest() 
    {
        Collider[] nearEnemies = Physics.OverlapSphere(transform.position, overlapSphereRadius, mask);
        
        
        float closestDistance = Mathf.Infinity;
        if (nearEnemies.Length > 0)
        {
            //Debug.Log("there is " + nearEnemies.Length + (" enemy"));
            for(int i = 0; i < nearEnemies.Length; i++)
            {
                Collider c = nearEnemies[i];
                float distance = Vector3.Distance(c.transform.position,transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = c;
                    //Debug.Log("enemy is " + c.name);
                }
             
            }

        }
        else
        {
            closestEnemy = null;
            //Debug.Log("Cant Detect Anyone");
        }
        float timer = Time.deltaTime;
        waitTime -= timer;
        if (closestEnemy != null && !isAttacking && !isSelectedWithMouse && !movementScript.isMoving)
        {
          
            
            
            if (waitTime < 0)
            {
                isTargetLocked = true;
                AttackRequest(closestEnemy);
             
            }



        }
       
        if (movementScript.isMoving && !isTargetLocked)
        {
            waitTime = initialWaitTime;
        }


    }
    protected virtual void AttackRequest(Collider enemy)
    {
       
        if (enemy != null)
        {
            Debug.Log(gameObject.name + " Attack requested to " + enemy.gameObject.name);
            if (isTargetLocked)
            {
                float distance = Vector3.Distance(transform.position, enemy.gameObject.transform.position);
                debugText.text = distance.ToString();

                if (distance < 2f)
                {
                    requestTargetPos = false;
                    targetPos = transform.position;

                    Attacking();
                   
                        isTargetLocked = false;
                        isSelectedWithMouse = false;

                    
                }
                else
                {
                    requestTargetPos = true;
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


}
