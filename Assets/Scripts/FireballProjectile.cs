using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    public float speed = 12f;
    public float damage = 0f;
    public float lifetime = 4f;
    public float impactDuration = 0.4f;

    private Rigidbody rigid;
    private Animator anim;
    private bool hasHit = false;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        
        anim = GetComponentInChildren<Animator>();
        
        rigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        
        Destroy(gameObject, lifetime);
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
        
        if (other.GetComponent<FireballProjectile>() != null) 
        {
            return;
        }
        
        if (other.GetComponent<ArrowProjectile>() != null) 
        {
            return;
        }

        Enemy enemy = other.GetComponentInParent<Enemy>();
        
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Impact();
    }

    private void Impact()
    {
        hasHit = true;
        
        rigid.velocity = Vector3.zero;
        
        rigid.isKinematic = true;

        Collider col = GetComponent<Collider>();
        
        if (col != null) 
        {
            col.enabled = false;
        }

        if (anim != null)
        {
            anim.SetTrigger("impact");
        }

        Destroy(gameObject, impactDuration);
    }
}
