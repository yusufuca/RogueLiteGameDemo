using NUnit.Framework;
using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;

public class EnemyMovement : Movement
{

    GameObject player;
    public Detection detection;
    public NPC_Types myData;
    public float maxPatrolLength = 10f;
    public GameObject marker;
    bool isMarkerSpawned = false;
    public List<GameObject> markers = new List<GameObject>();
    protected override void Start()
    {
        base.Start();
      
        Speed = myData.moveSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
        detection = GetComponent<Detection>();
        targetPos = transform.position;
    }
    protected override void Update()
    {
        base.Update();
        PatrolAreaPrep();
    }
    protected override void MoveToPoisiton()
    {

        detection.DetectPlayer();
        if (detection.isRayHitPlayer)
        {
            
            targetPos = detection.targetPos;
        }
        else
        {
            //EnemyPatrol();
        }
        
   
        base.MoveToPoisiton();
        Vector3 gravityMovement = new Vector3(0, playerVelocity.y, 0);
        charController.Move(gravityMovement * Time.deltaTime);
    }


    public void EnemyPatrol()
    {
        Vector3 currentPos = transform.position;
        float distance = Vector3.Distance(targetPos, currentPos);

        if (distance < 2f)
        {
            if (gm.patrolArea!= null)
            {
                float halfLength = gm.patrolAreaLength / 2f;
                float halfDepth = gm.patrolAreaDepth / 2f;

                targetPos.x = Random.Range(gm.patrolArea.position.x - halfLength, gm.patrolArea.position.x + halfLength);
                targetPos.z = Random.Range(gm.patrolArea.position.z - halfDepth, gm.patrolArea.position.z + halfDepth);
            }
            else
            {
                targetPos.x = Random.Range(currentPos.x - maxPatrolLength, currentPos.x + maxPatrolLength);
                targetPos.z = Random.Range(currentPos.z - maxPatrolLength, currentPos.z + maxPatrolLength);
            }

            GameObject newMarker = Instantiate(marker, targetPos, Quaternion.identity);
            markers.Add(newMarker);
        }

        if (markers.Count > 3)
        {
            Destroy(markers[0]);
            markers.RemoveAt(0);
        }
    }

    public void PatrolAreaPrep()
    {
        if (gm.patrolArea == null) return;

        float halfLength = gm.patrolAreaLength / 2f;
        float halfDepth = gm.patrolAreaDepth / 2f;
        Vector3 center = gm.patrolArea.position;

        Vector3 topLeft = new Vector3(center.x - halfLength, center.y, center.z + halfDepth);
        Vector3 topRight = new Vector3(center.x + halfLength, center.y, center.z + halfDepth);
        Vector3 bottomLeft = new Vector3(center.x - halfLength, center.y, center.z - halfDepth);
        Vector3 bottomRight = new Vector3(center.x + halfLength, center.y, center.z - halfDepth);

        Debug.DrawLine(topLeft, topRight, Color.blue, 0f, false);
        Debug.DrawLine(topRight, bottomRight, Color.blue, 0f, false);
        Debug.DrawLine(bottomRight, bottomLeft, Color.blue, 0f, false);
        Debug.DrawLine(bottomLeft, topLeft, Color.blue, 0f, false);
    }




}
