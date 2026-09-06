using UnityEngine;

public class HitReactState : BaseCombatState
{
    public override void EnterState(Attack attackScript)
    {
        base.EnterState(attackScript);
        attackScript.TriggerHitStop(0.1f);
        attackScript.animator.speed = 1f;
    }
    public override void UpdateState(Attack attackScript)
    {
        base.UpdateState(attackScript);
        if (!attackScript.hitted) attackScript.StateChanger(new AttackIdleState());
    }
    public override void ExitState(Attack attackScript)
    {
        base.ExitState(attackScript);
    }
}
