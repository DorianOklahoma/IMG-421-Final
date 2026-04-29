using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{
    [Header("Enemy Settings")]
    [Header("Inscribed")]
    public float minDistance = .5f;
    public float sightDistance = 5f;
    public float attackDistance = 4f;

    [Header("Dynamic")]
    public GameObject target;
    public bool active = true;

    // Private variables
    // Enemy state variables
    private enum State{idle, chase};
    private State currentState;
    public float distance;
    public Vector3 direction = new Vector3(1, 0, 0);
    protected override void Start()
    {
        base.Start();
        target = FindFirstObjectByType<Player>().gameObject;
    }

    protected override void Update()
    {
        base.Update();
        if (active)
        {
            if (target != null)
            {
                distance = Vector3.Distance(target.transform.position, transform.position);
                direction = target.transform.position - transform.position;

                switch(currentState)
                {
                    case State.idle:
                        IdleState();
                        if (distance < sightDistance)
                        {
                            currentState = State.chase;
                        }
                        break;
                    case State.chase:
                        ChaseTarget();
                        if (distance < attackDistance)
                        {
                            Attack();
                        }
                        if (distance > sightDistance * 1.2f)
                        {
                            currentState = State.idle;
                        }
                        break;
                }
            }
        }
        
        Animate();
    }

    public virtual void Attack()
    {
        if (wc != null)
        {
            wc.PrimaryAttack();
        }
        if (anim != null)
        {
            anim.SetTrigger("attack");
        }
    }

    public abstract void ChaseTarget();
    public virtual void IdleState()
    {
        rigid.velocity = Vector3.zero;
    }


}
