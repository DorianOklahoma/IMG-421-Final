using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Weapon : MonoBehaviour
{
    [Header("Static Weapon Settings")]
    public string weaponName = "Weapon Name";
    public float primaryDamage = 1.0f;
    public float secondaryDamage = 1.0f;
    public float primaryFireRate = 1.0f;
    public float secondaryFireRate = 1.0f;
    public bool isDefaultWeapon = false;

    [Header("Dynamic Weapon Settings")]
    public float primaryCooldown = 0f;
    public float secondaryCooldown = 0f;


    void Start()
    {
    }

    void Update()
    {
        // Cooldown management
        if (primaryCooldown > 0f)
        {
            primaryCooldown -= Time.deltaTime;
        }
        if (secondaryCooldown > 0f)
        {
            secondaryCooldown -= Time.deltaTime;
        }
    }

    // Primary attack methods
    public virtual void UsePrimary()
    {
        Debug.Log($"Used {weaponName} with {primaryDamage} damage at a rate of {primaryFireRate}");
    }
    public virtual void PrimaryAttack()
    {
        if (primaryCooldown <= 0f)
        {
            UsePrimary();
            primaryCooldown = primaryFireRate;
        }
    }

    // Secondary attack methods
    public virtual void UseSecondary()
    {
        Debug.Log($"Used {weaponName} with {secondaryDamage} damage at a rate of {secondaryFireRate}");
    }
    public virtual void SecondaryAttack()
    {
        if (secondaryCooldown <= 0f)
        {
            UseSecondary();
            secondaryCooldown = secondaryFireRate;
        }
    }

    public virtual void OnDropWeapon()
    {
        // Detach weapon from player
        transform.SetParent(null);

        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>(true);
        Collider[] colliders = GetComponentsInChildren<Collider>(true);

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
        }

        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }

        Debug.Log($"Dropped {weaponName}");
    }

    public virtual void OnEquip()
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>(true);
        Collider[] colliders = GetComponentsInChildren<Collider>(true);

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;
        }

        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        Debug.Log($"Equipped {weaponName}");
    }

}
