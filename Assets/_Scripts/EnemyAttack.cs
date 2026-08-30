using UnityEngine;

public class EnemyAttack : Attack
{
   
    Detection detection;

    private void OnEnable()
    {
    

    }
    protected override void Start()
    {
        detection = GetComponent<Detection>();
        base.Start();

    }
    protected override void Update()
    {
        if (isStunned) return;
        AttackRequest();
        base.Update();


    }

    protected override void AttackClosest()
    {
        
    }
    public void AttackRequest()
    {
        if (detection.isRayHitPlayer)
        {
            if (detection.detectedCollider != null)
            {
                isTargetLocked = true;
                base.AttackRequest(detection.detectedCollider);
            }
        }  
    }

}
