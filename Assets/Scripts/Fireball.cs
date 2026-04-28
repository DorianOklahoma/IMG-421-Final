using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Weapon
{
    public FireballProjectile fireballPrefab;
    public Transform launchPoint;

    protected override void Start()
    {
        base.Start();
    }

    public override void UsePrimary()
    {
        base.UsePrimary();
        
        if (anim != null)
        {
            anim.SetTrigger("throw");
        }

        FireballProjectile projectile = Instantiate(fireballPrefab, launchPoint.position, Quaternion.identity);
        
        projectile.Launch(launchPoint.right, primaryDamage);
    }
}
