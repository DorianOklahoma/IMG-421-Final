using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    
    [Header("Character Settings")]
    public string characterName;
    public float health = 100;
    public float blockPercentage = 0f;

    [Header("Movement Settings")]
    public float speed = 5.0f;
    public float maxSpeed = 5.0f;
    public float acceleration = 200.0f;
    public float airAcceleration = 20.0f;
    protected enum Facing{left, right};
    protected Facing currentFacing = Facing.right;

    
    protected Rigidbody rigid;
    protected WeaponController wc;
    protected Animator anim;
    protected SpriteRenderer sprite;

    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody>();
        wc = GetComponent<WeaponController>();
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        SetFacing();
    }

    protected virtual void SetFacing()
    {
        if (rigid.velocity.x < 0)
            {
                currentFacing = Facing.left;
            }
            else if (rigid.velocity.x > 0)
            {
                currentFacing = Facing.right;
            }
    }

    public virtual bool IsGrounded()
    {
        return rigid.velocity.y <= 0.01 && rigid.velocity.y >= -0.01;
    }

    public virtual bool IsFlipped()
    {
        return currentFacing == Facing.left;
    }

    public virtual float GetHorizontalDirection()
    {
        return rigid.velocity.x;
    }

    public virtual void Animate()
    {
        if (anim == null) return;
        float horizontalDirection = GetHorizontalDirection();

        switch(currentFacing)
        {
            case Facing.left:
                sprite.flipX = true;
                break;
            case Facing.right:
                sprite.flipX = false;
                break;
        }

        if (IsGrounded())
        {
            anim.SetBool("isFlying", false);
            anim.SetBool("isFalling", false);
            anim.SetBool("isRunning", horizontalDirection != 0);
        }
        else
        {
            if (rigid.velocity.y > 0)
            {
                anim.SetBool("isFalling", false);
                anim.SetBool("isFlying", true);
            }
            if (rigid.velocity.y < 0)
            {
                anim.SetBool("isFlying", false);
                anim.SetBool("isFalling", true);
            }
        }
    }

    public virtual void TakeDamage(float damage = 0)
    {
        damage *= 1 - blockPercentage;
        health -= damage;
    }
}
