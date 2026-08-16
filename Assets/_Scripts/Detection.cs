using UnityEngine;

public class Detection : MonoBehaviour
{
    public GameObject player;

    public float visionDistance = 10;
    public float visionAngle = 45f;
    public int maxRayCount = 5;
    public bool isRayHitPlayer = false;
    public Vector3 targetPos;
    LayerMask layerMask;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DetectPlayer()
    {
        /*if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < visionDistance)
            {
                Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, directionToPlayer);
                if (angle < visionAngle)
                {
                    Debug.Log("I can see the player!");
                    targetPos = player.transform.position;
                }
                else
                {
                    targetPos = transform.position;
                }

            }
            else
            {
                targetPos = transform.position;
            }
            
        }
        else
        {
            Debug.Log("Player Data is Null");
            targetPos = transform.position;
        }*/
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + (Vector3.up * 1.5f);
        for(int i = 0; i < maxRayCount; i++)
        {
            float currentAngle = 0;

            float percentage = (float)i / (maxRayCount - 1);

            currentAngle = Mathf.Lerp(visionAngle, -visionAngle, percentage);

            Vector3 rayAngle = Quaternion.Euler(0, currentAngle, 0) * transform.forward;

            Debug.DrawRay(rayOrigin, rayAngle * visionDistance, Color.red,0f,false);
            if (Physics.Raycast(rayOrigin, rayAngle, out hit, visionDistance))
            {

                if (hit.collider.CompareTag("Player"))
                {
                    isRayHitPlayer=true;
                    Debug.Log("Ray Hit The Player");
                    targetPos = player.transform.position;
                }
                else
                {
                    isRayHitPlayer = false;
                }
            }
        }
       
    }


}
