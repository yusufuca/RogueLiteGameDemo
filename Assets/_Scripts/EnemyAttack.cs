using UnityEngine;

public class EnemyAttack : Attack
{
    public NPC_Types myData;
    Detection detection;
    protected override void Start()
    {
        detection = GetComponent<Detection>();
        currentHP = myData.HP;
        base.maxHP = currentHP;
        attackSpeed = myData.attackSpeed;
        base.Start();

    }
    protected override void Update()
    {
        if (isStunned) return;
        AttackRequest();
        base.Update();

        if(currentHP <= 0)
        {
            Destroy(gameObject);
        }
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
