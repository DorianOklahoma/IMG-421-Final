using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    [Header("Flying specific settings")]
    public float hoverDistance = 1.5f;
    protected override void Start()
    {
        base.Start();
        rigid.useGravity = false;
    }
    public override void ChaseTarget()
    {
        Vector3 hoverTargetPosition = target.transform.position + new Vector3(0, hoverDistance, 0);
        float distanceToHoverTarget = Vector3.Distance(rigid.transform.position, hoverTargetPosition);
        Vector3 newVelocity;

        if (distanceToHoverTarget > minDistance)
        {
            Vector3 direction = (hoverTargetPosition - rigid.position).normalized;
            // Desired velocity toward the target
            Vector3 targetVelocity = direction * maxSpeed;

            // Move current velocity toward target velocity
            newVelocity = Vector3.MoveTowards(rigid.velocity, targetVelocity, airAcceleration * Time.deltaTime);

        }
        else
        {
            newVelocity = Vector3.MoveTowards(rigid.velocity, Vector3.zero, airAcceleration * Time.deltaTime);
        }
        rigid.velocity = newVelocity;
    }
}
