using UnityEngine;

public class AttackIdleState : BaseCombatState
{
    public override void EnterState(Attack attackScript)
    {
        base.EnterState(attackScript);
        attackScript.weaponTrail.emitting = false;
        attackScript.attackRequest = false;
        attackScript.cast1Request = false;
        attackScript.cast2Request = false;
        attackScript.animator.speed = 1;

    }
    public override void UpdateState(Attack attackScript)
    {

        base.UpdateState(attackScript);
        if (attackScript.hitted) attackScript.StateChanger(new HitReactState()); 
        if (attackScript.attackRequest)
        {
            TurnToAttackPosition(attackScript);
            Attacking(attackScript);
            attackScript.StateChanger(new AttackWindupState());
        }
        if (attackScript.cast1Request) 
        {
            attackScript.StateChanger(new AttackWindupState());
        }
        if ( attackScript.cast2Request)
        {
            attackScript.StateChanger(new AttackWindupState());
        }

    }
    public override void ExitState(Attack attackScript)
    {
        base.ExitState(attackScript);
    }
}
