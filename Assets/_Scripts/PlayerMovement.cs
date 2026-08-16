using UnityEngine;

public class PlayerMovement : Movement
{
    PlayerAttack playerAttack;
    protected override void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        base.Start();
    }

   
    protected override void Update()
    {
        MovementKeys();
        
        base.Update();
        
    }
    protected override void MoveToPoisiton()
    {
        if (Input.GetMouseButtonDown(1))
        {
            playerAttack.isTargetLocked = false;
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
            targetPos = hit.point;
            targetPos.y = 0;
        }
        
        if(playerAttack.isTargetLocked) 
        {
            targetPos = playerAttack.targetPos;
            Debug.Log("Player Moving To Enemy");
        }
        base.MoveToPoisiton();
    }
    private void MovementKeys()
    {
        float mvRL = Input.GetAxis("Horizontal") * Speed;
        float mvFB = Input.GetAxis("Vertical") * Speed;
        Vector3 movement = new Vector3(mvRL, playerVelocity.y, mvFB);
        if (mvRL != 0 || mvFB != 0)
        {
            targetPos = transform.position;
            Vector3 lookDirection = new Vector3(mvRL, 0, mvFB);
            transform.forward = lookDirection;
            playerAttack.isTargetLocked = false;
        }

        charController.Move(movement * Time.deltaTime);

    }
}
