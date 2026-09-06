using UnityEngine;

public class PlayerAttack : Attack
{
    [SerializeField] private InputReader inputReader;
    
    private void OnEnable()
    {
        if (inputReader == null) inputReader = GetComponent<InputReader>();
        inputReader.OnAttackPerformed += base.AttackRequested;
        inputReader.OnSkill1Performed += base.Cast1Requested;
        inputReader.OnSkill2Performed += base.Cast2Requested;

    }
    private void OnDisable()
    {
        inputReader.OnAttackPerformed -= base.AttackRequested;
        inputReader.OnSkill1Performed -= base.Cast1Requested;
        inputReader.OnSkill2Performed -= base.Cast2Requested;
    }
}
