using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    public Collider hitCollider;
    private List<Enemy> enemies = new List<Enemy>();
    void Start()
    {
        hitCollider = GetComponent<BoxCollider>();
    }

    public override void UsePrimary()
    {
        base.UsePrimary();

        foreach(Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.TakeDamage(primaryDamage);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null && !enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemies.Remove(enemy);
        }
    }
}
