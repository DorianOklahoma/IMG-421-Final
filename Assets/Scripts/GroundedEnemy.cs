using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundedEnemy : Enemy
{
    
    public override void ChaseTarget()
    {
        float moveHorizontal = (transform.position.x - target.transform.position.x < 0) ? 1 : -1;
        float targetSpeed = moveHorizontal * maxSpeed;

        float distanceToTarget = Vector3.Distance(rigid.transform.position, target.transform.position);
        if (distanceToTarget > minDistance)
        {
            float newSpeed = Mathf.MoveTowards(rigid.velocity.x, targetSpeed, acceleration * Time.deltaTime);
            rigid.velocity = new Vector3(newSpeed, rigid.velocity.y, 0);
        }
        else
        {
            float newSpeed = Mathf.MoveTowards(rigid.velocity.x, 0, airAcceleration * Time.deltaTime);
            rigid.velocity = new Vector3(newSpeed, rigid.velocity.y, 0);
        }
    }
}
