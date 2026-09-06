using UnityEngine;

public class AttackActivateState : BaseCombatState
{
    public override void EnterState(Attack attackScript)
    {
        base.EnterState(attackScript);
    }
    public override void UpdateState(Attack attackScript)
    {
        base.UpdateState(attackScript);

        CreateAndCheckTheOverlapBox(attackScript);
        if (attackScript.hitted) attackScript.StateChanger(new HitReactState());
        if (attackScript.currentStateCurrentTime >= 0.6f)
        {
            attackScript.StateChanger(new AttackRecovery());
        }

    }
    public override void ExitState(Attack attackScript)
    {
        base.ExitState(attackScript);
        attackScript.attackRequest = false;
        attackScript.cast1Request = false;
        attackScript.hittedEnemies.Clear();
    }
}
