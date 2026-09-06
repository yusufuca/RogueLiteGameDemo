using NUnit.Framework;
using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;

public class EnemyMovement : Movement
{

   
    public Detection detection;
    public float maxPatrolLength = 10f;
    public GameObject marker;
    public List<GameObject> markers = new List<GameObject>();
    private Vector3 center;
    protected override void Start()
    {
        base.Start();      
        detection = GetComponent<Detection>();
        CalculateCenter();
        targetPos = center;
    }
    protected override void Update()
    {
        base.Update();
        CalculateCenter();
        PatrolAreaPrep();

    }
    public override void CalculateMoveDirection()
    {
        detection.DetectPlayer();
        if (detection.isPlayerDetected)
        {

            targetPos = detection.targetPos;
        }
        else
        {
            EnemyPatrol();
        }
        base.CalculateMoveDirection();
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

                targetPos.x = Random.Range(center.x - halfLength, center.x + halfLength);
                targetPos.z = Random.Range(center.z - halfDepth, center.z + halfDepth);
                targetPos.y = transform.position.y;
            }
            else
            {
                targetPos.x = Random.Range(currentPos.x - maxPatrolLength, currentPos.x + maxPatrolLength);
                targetPos.z = Random.Range(currentPos.z - maxPatrolLength, currentPos.z + maxPatrolLength);
                targetPos.y = transform.position.y;
            }

            //GameObject newMarker = Instantiate(marker, targetPos, Quaternion.identity);
            //markers.Add(newMarker);
        }

        /*if (markers.Count > 3)
        {
            Destroy(markers[0]);
            markers.RemoveAt(0);
        }*/
    }
    public void CalculateCenter()
    {
        Vector3 dirToZero = (Vector3.zero - transform.parent.position);
        dirToZero.y = 0;
        dirToZero = dirToZero.normalized;
        center = transform.parent.position + (dirToZero * 30);
        center.y = 0;
    }

    public void PatrolAreaPrep()
    {
        if (gm.patrolArea == null) return;

        float halfLength = gm.patrolAreaLength / 2f;
        float halfDepth = gm.patrolAreaDepth / 2f;
       

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
