using UnityEngine;

public class PlayerAttack : Attack
{
    //PlayerMovement playerMovement;
    Collider onMouseEnemy;
    public float HP = 1000;
    public float damage = 10;
    public float myAttackSpeed = 2;
    
    
    protected override void Start()
    {
        //playerMovement = GetComponent<PlayerMovement>();
        base.Start();
        base.maxHP = HP;
        base.currentHP = HP;
        base.currentDamage = damage;
        attackSpeed = myAttackSpeed;
    }
    protected override void Update()
    {
        AttackRequest();
  

        if (Input.GetKeyDown(KeyCode.R))
        {
            base.Attacking();
        }
        base.Update();
    }
    public void AttackRequest()
    {
       
            
        
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
            
        if (Input.GetMouseButtonDown(0))
        {
            if (hit.collider != null && hit.collider.CompareTag("Enemy")) 
            {
                Debug.Log("Enemey Selected");
                onMouseEnemy = hit.collider;
                float distance = Vector3.Distance(transform.position, hit.collider.gameObject.transform.position);
                isTargetLocked = true;

                isSelectedWithMouse = true;
            }
            if (isTargetLocked && isSelectedWithMouse)
            {
                base.AttackRequest(onMouseEnemy);
            }
        }
       
        
    }
}
