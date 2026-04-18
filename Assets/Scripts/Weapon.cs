using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Weapon : MonoBehaviour
{
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
    public virtual void usePrimary()
    {
        Debug.Log($"Used {weaponName} with {primaryDamage} damage at a rate of {primaryFireRate}");
    }
    public virtual void PrimaryAttack()
    {
        if (primaryCooldown <= 0f)
        {
            usePrimary();
            primaryCooldown = primaryFireRate;
        }
    }

    // Secondary attack methods
    public virtual void useSecondary()
    {
        Debug.Log($"Used {weaponName} with {secondaryDamage} damage at a rate of {secondaryFireRate}");
    }
    public virtual void SecondaryAttack()
    {
        if (secondaryCooldown <= 0f)
        {
            useSecondary();
            secondaryCooldown = secondaryFireRate;
        }
    }

    // Drop weapon method
    public virtual void onDropWeapon()
    {
        // Detach weapon from player and enable physics
        transform.SetParent(null);
        Rigidbody rb = GetComponent<Rigidbody>();
        Collider col = GetComponent<Collider>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        if (col != null)
        {
            col.enabled = true;
        }
        Debug.Log($"Dropped {weaponName}");
    }

    // Equip weapon method
    public virtual void onEquip()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Collider col = GetComponent<Collider>();
        
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        if (col != null)
        {
            col.enabled = false;
        }
        Debug.Log($"Equipped {weaponName}");
    }

}
