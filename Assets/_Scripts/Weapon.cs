using UnityEngine;

public class Weapon : MonoBehaviour
{
    BoxCollider weaponCollider;
    private void Start()
    {
        weaponCollider = GetComponent<BoxCollider>();
    }
    void Update()
    {
                
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Debug.Log("Player hitted: " + other.tag);
        }
    }
}
