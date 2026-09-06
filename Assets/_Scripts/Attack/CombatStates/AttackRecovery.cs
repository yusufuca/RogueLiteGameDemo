using UnityEngine;

public class AttackRecovery : BaseCombatState
{
    public override void EnterState(Attack attackScript)
    {
        base.EnterState(attackScript);
        if(attackScript.weaponTrail != null) attackScript.weaponTrail.emitting = false;
    }
    public override void UpdateState(Attack attackScript)
    {
        base.UpdateState(attackScript);
        if (attackScript.hitted) attackScript.StateChanger(new HitReactState());
        if(attackScript.currentStateCurrentTime >= 0.72f || attackScript.attackRequest)
        {
            attackScript.animator.speed = 1;
            attackScript.StateChanger(new AttackIdleState());
        }
        else
        {
            attackScript.animator.speed = 0.25f;
        }
    }
    public override void ExitState(Attack attackScript)
    {
        base.ExitState(attackScript);

    }
}
