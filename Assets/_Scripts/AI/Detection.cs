using UnityEngine;

public class Detection : MonoBehaviour
{
    public GameObject player;
    public float visionDistance = 10;
    public float visionAngle = 45f;
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private float angleToPlayer;
    [SerializeField] private Attack attackScript;
    Color rayColor;
    public bool isPlayerDetected;
    public Vector3 targetPos;
    private float lastDetectedTime;
    [SerializeField] private float detectionCoolDown = 5;
    private void Awake()
    {
        attackScript = GetComponent<Attack>();
    }
    void Start()
    {
        player = GameManager.gm.Player;
    }
    /*public void DetectPlayer()
    {
  
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
                    //Debug.Log("Ray Hit The Player");
                    targetPos = player.transform.position;
                    detectedCollider = hit.collider;
                }
                else
                {
                    detectedCollider=null;
                    isRayHitPlayer = false;
                }
            }
        }
       
    }*/

    public void DetectPlayer()
    {
        distanceToPlayer = Vector3.Distance(player.transform.position,transform.position);
        //angleToPlayer = Vector3.Dot(transform.TransformDirection(Vector3.forward),Vector3.Normalize(player.transform.position - transform.position));
        angleToPlayer = Vector3.Angle(player.transform.position - transform.position, transform.forward);

        Vector3 leftRayDirection = Quaternion.Euler(0, -visionAngle, 0) * transform.forward;
        Vector3 rightRayDirection = Quaternion.Euler(0, visionAngle, 0) * transform.forward;
        Vector3 rayOrigin = transform.position + (Vector3.up * 1.5f);

        Debug.DrawRay(rayOrigin, leftRayDirection * visionDistance, rayColor, 0, false);
        Debug.DrawRay(rayOrigin, rightRayDirection * visionDistance, rayColor, 0, false);

        if (distanceToPlayer < visionDistance) 
        {
            if (angleToPlayer < visionAngle || attackScript.hitted)
            {
                isPlayerDetected = true;
                targetPos = player.transform.position;
                rayColor = Color.green;
                lastDetectedTime = Time.time;
            }
            else
            {
                if (Time.time - lastDetectedTime < detectionCoolDown && !isPlayerDetected)
                {
                    isPlayerDetected = true;
                    targetPos = player.transform.position;
                    rayColor = Color.green;
                }
                else
                {
                    isPlayerDetected = false;
                    rayColor = Color.blue;
                }
            }
        }
        else
        {
            isPlayerDetected = false;
            rayColor = Color.blue;
        }
    }
}
