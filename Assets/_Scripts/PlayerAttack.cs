using UnityEngine;

public class PlayerAttack : Attack
{
    //PlayerMovement playerMovement;
    Collider onMouseEnemy;

    
    
    protected override void Start()
    {
        //playerMovement = GetComponent<PlayerMovement>();
        base.Start();
        
    }
    protected override void Update()
    {

        if (isStunned) return;
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
