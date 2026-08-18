using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    BoxCollider weaponCollider;
    public Attack attackScript;
    public float damage = 5f;
    public GameObject parent;
    private void Start()
    {
        weaponCollider = GetComponent<BoxCollider>();
        
    }
    void Update()
    {
                
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != parent && !other.CompareTag("Plane"))
        {
            Debug.Log(parent.name + " hitted: " + other.tag + other.gameObject.name);
            other.gameObject.GetComponent<Attack>().TakeDamage(damage + attackScript.currentDamage);
        }
    }
}
