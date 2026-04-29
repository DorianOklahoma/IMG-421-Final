using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    public float speed = 18f;
    public float damage = 0f;
    public float lifetime = 5f;

    private Rigidbody rigid;
    private bool hasHit = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        Vector3 dir = rigid.velocity.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rigid.rotation = Quaternion.Euler(0, 0f, angle);
    }

    public void Launch(Vector3 direction, float dmg)
    {
        damage = dmg;

        rigid.velocity = direction.normalized * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasHit) 
        {
            return;
        }

        if (other.GetComponent<ArrowProjectile>() != null) 
        {
            return;
        }

        if (other.GetComponent<FireballProjectile>() != null) 
        {
            return;
        }

        Enemy enemy = other.GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Stick();
    }

    private void Stick()
    {
        hasHit = true;

        rigid.velocity = Vector3.zero;

        rigid.isKinematic = true;

        Collider col = GetComponent<Collider>();

        if (col != null) 
        {
            col.enabled = false;
        }

        Destroy(gameObject, 3f);
    }
}