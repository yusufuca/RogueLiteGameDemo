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
    private void OnTriggerEnter(Collider other)
    {
        if (!damageGiven)
        {
            if (other.gameObject != parent && other.TryGetComponent<IDamagable>(out IDamagable target))
            {
                Debug.Log(parent.name + " hitted: " + other.tag + other.gameObject.name);
                target.TakeDamage(parent.GetComponent<StatManager>().currentDamage + damage);
                damageGiven = true;

            }
        }
    }
}
