using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    BoxCollider weaponCollider;
    public Attack attackScript;
    public float damage = 5f;
    public GameObject parent;
    public string parentTag;
    public TextMeshPro damagePopUp;
    public bool damageGiven;
    private void Start()
    {
        weaponCollider = GetComponent<BoxCollider>();
        parentTag = parent.tag;    
    }
    void Update()
    {
                
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!damageGiven)
        {
            if (other.gameObject != parent && !other.CompareTag("Plane") && !other.CompareTag("Weapon") && !other.CompareTag(parentTag))
            {
                Debug.Log(parent.name + " hitted: " + other.tag + other.gameObject.name);
                Attack otherAttackScript = other.gameObject.GetComponent<Attack>();
                float otherHp = otherAttackScript.currentHP;
                otherAttackScript.TakeDamage(damage + attackScript.currentDamage);
                CharUIUpdater uiScript = other.gameObject.GetComponentInChildren<CharUIUpdater>();
                StartCoroutine(uiScript.DamagePopUp(damage + attackScript.currentDamage));
                damageGiven = true;

            }
        }
    }
}
