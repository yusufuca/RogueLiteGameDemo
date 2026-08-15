using UnityEngine;

public class PlayerMovement : Movement
{
    
    protected override void Start()
    {
        base.Start();
    }

   
    protected override void Update()
    {
        MovementKeys();
        //lastPos = transform.position;
        base.Update();
        
    }
    protected override void MoveToPoisiton()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
            targetPos = hit.point;
            targetPos.y = 0;
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
        }

        charController.Move(movement * Time.deltaTime);

    }
}
