using UnityEngine;

public class AttackWindupState : BaseCombatState
{
    public override void EnterState(Attack attackScript)
    {
        base.EnterState(attackScript);
        if (attackScript.weaponTrail != null) attackScript.weaponTrail.emitting = true;
    }
    public override void UpdateState(Attack attackScript)
    {
        base.UpdateState(attackScript);
        if (attackScript.hitted) attackScript.StateChanger(new HitReactState());
        if (attackScript.currentStateCurrentTime >= 0.1f)
        {
            attackScript.animator.speed = 1;
            attackScript.StateChanger(new AttackActivateState());
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
