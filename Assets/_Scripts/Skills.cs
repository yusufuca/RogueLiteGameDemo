using UnityEngine;


public enum CharClasses
{
    Warrior,
    Mage,
    Archer
}
public enum SkillTypes
{
    Buff,
    Attack,
    Defence,
    Stun,

}

[CreateAssetMenu(fileName = "Skills", menuName = "Scriptable Objects/Skills")]

public class Skills : ScriptableObject
{
    public SkillTypes skillTpye;

    public float value;
    public float coolDown;

    public bool isContinous;
    public float duration;

    public CharClasses suggestedClass;
    public float classMultiplier;

    public string animTriggerString;
    

}
