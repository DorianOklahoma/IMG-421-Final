using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{
    [Header("Enemy Settings")]
    [Header("Inscribed")]
    public float minDistance = .5f;
    public float sightDistance = 5f;

    [Header("Dynamic")]
    public GameObject target;
    public bool active = true;

    // Private variables
    // Enemy state variables
    private enum State{idle, chase};
    private State currentState;
    // Movement variables
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
            float distance = Vector3.Distance(target.transform.position, transform.position);

            switch(currentState)
            {
                case State.idle:
                    if (distance < sightDistance)
                    {
                        currentState = State.chase;
                    }
                    break;
                case State.chase:
                    ChaseTarget();
                    if (distance > sightDistance * 1.2f)
                    {
                        currentState = State.idle;
                    }
                    break;
            }
        }
        
        Animate();
    }

    public virtual void Attack()
    {
        if (wc != null)
        {
            wc.weapon.PrimaryAttack();
        }
    }

    public abstract void ChaseTarget();


}
