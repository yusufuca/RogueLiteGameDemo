using UnityEngine;



public enum entityTypes
{
    Ranger,
    Melee,
    Mage
}
[CreateAssetMenu(fileName = "Entity_Types", menuName = "Scriptable Objects/Entity")]
public class Entity_Types : ScriptableObject
{
    public entityTypes npcTypes;
    [System.Serializable]
    public struct HealthStats
    {
        public float HP;
        public float maxHealth;
    }
    [System.Serializable]
    public struct AttackStats
    {
        public float damage;
        public float attackSpeed;
        public float attackRange;
    }
    [System.Serializable]
    public struct MovementStats
    {
        public float Stamina;
        public float moveSpeed;
        public float jumpHeight;
        public float runMultiplier;
        public float dashMultiplier;
        public float weight;
    }
    [System.Serializable]
    public struct EconomyStats
    {
        public int startCoin;
        public int startSkillPoint;
    }
    public EconomyStats economyStats;
    public MovementStats movementStats;
    public HealthStats healthStats;
    public AttackStats attackStats;
    public int scoreValue;
    public GameObject prefab;
}
