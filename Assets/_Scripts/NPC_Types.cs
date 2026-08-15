using UnityEngine;



public enum npcTypes
{
    Ranger,
    Melee,
    Mage
}
[CreateAssetMenu(fileName = "NPC_Types", menuName = "Scriptable Objects/NPC_Types")]
public class NPC_Types : ScriptableObject
{
    public npcTypes npcTypes;
    public float HP;
    public float moveSpeed;
    public float damage;
    public float maxHealth;
    public GameObject prefab;
}
