using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skywisp : FlyingEnemy
{
    public override void Animate()
    {
        if (anim == null) return;
        if(rigid.velocity == Vector3.zero)
        {
            anim.SetBool("isMoving", false);
        }
        else
        {
            anim.SetBool("isMoving", true);
        }
    }
}
