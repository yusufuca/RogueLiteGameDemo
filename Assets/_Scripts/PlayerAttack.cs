using UnityEngine;

public class PlayerAttack : Attack
{
    //PlayerMovement playerMovement;
    GameObject onMouseEnemy;
    public Vector3 targetPos;
    public bool isTargetLocked;
    protected override void Start()
    {
        //playerMovement = GetComponent<PlayerMovement>();
        base.Start();
    }
    protected override void Update()
    {
        AttackRequest();
       
        base.Update();
    }
    private void AttackRequest()
    {
       
            
        
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
            
        if (Input.GetMouseButtonDown(0))
        {
            if (hit.collider.CompareTag("Enemy")) 
            {
                Debug.Log("Enemey Selected");
                onMouseEnemy = hit.collider.gameObject;
                float distance = Vector3.Distance(transform.position, hit.collider.gameObject.transform.position);
                if (distance < 1f)
                {
                    isTargetLocked = false;
                    isAttacking = true;
                    base.Attacking();
                    attackReuqest = true;
                }
                else
                {
                    isTargetLocked = true;
                    
                    attackReuqest = false;
                }
            }
        }
        if (isTargetLocked)
        {
            targetPos = onMouseEnemy.transform.position;
        }
        
    }
}
