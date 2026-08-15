using Unity.Hierarchy;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class EnemyMovement : Movement
{
    GameObject player;
    public NPC_Types myData;
    public float visionDistance = 10;
    public float visionAngle = 45f;
    protected override void Start()
    {
        base.Start();
      
        Speed = myData.moveSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    protected override void Update()
    {
        base.Update();
       
    }
    protected override void MoveToPoisiton()
    {
        if (player != null)
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
        }

            base.MoveToPoisiton();
    }

    public void PlayerAggro()
    {

    }

    private void OnDrawGizmosSelected()
    {
        // 3. We wrap the drawing code so it doesn't break the final game build
#if UNITY_EDITOR

        // Set the color to Red, with 20% opacity (0.2f) so we can see through it
        Handles.color = new Color(1f, 0f, 0f, 0.2f);

        // Calculate where the left edge of our vision cone starts
        Vector3 leftVisionEdge = Quaternion.Euler(0, -visionAngle, 0) * transform.forward;

        // Draw a solid pie slice! 
        // (Center point, up direction, starting edge, total angle, radius)
        Handles.DrawSolidArc(transform.position, Vector3.up, leftVisionEdge, visionAngle * 2, visionDistance);

#endif
    }



}
