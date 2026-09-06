using UnityEngine;

public class EnemyAttack : Attack
{
    Detection detection;
    [SerializeField] private float DistanceToPlayer; 
    protected override void Start()
    {
        detection = GetComponent<Detection>();
        base.Start();
    }
    protected override void Update()
    {
        DistanceToPlayer = Vector3.Distance(transform.position, detection.player.transform.position);
        if (isStunned) return;
        AttackRequest();
        base.Update();
    }
    /*public void AttackRequest()
    {
        if (detection.isRayHitPlayer)
        {
            if (detection.detectedCollider != null)
            {
                isTargetLocked = true;
                base.AttackRequested(detection.detectedCollider);
            }
        }  
    }*/
    private void AttackRequest()
    {
        if (detection.isPlayerDetected && DistanceToPlayer < statManager.myData.attackStats.attackRange)
        {
            base.AttackRequested();
        }
        if (attackRequest && DistanceToPlayer > statManager.myData.attackStats.attackRange)
        {
            StateChanger(new AttackIdleState());
        }
    }
}
