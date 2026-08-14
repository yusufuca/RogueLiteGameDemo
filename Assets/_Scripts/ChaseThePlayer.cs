using UnityEngine;

public class ChaseThePlayer : MonoBehaviour
{
    public Transform playerTransform;
    void Update()
    {
        transform.position = playerTransform.position + new Vector3(0,15,-14);
    }
}
