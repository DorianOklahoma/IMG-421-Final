using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponController : MonoBehaviour
{
    [Header("Weapon Settings")]
    public Weapon weapon;
    public Weapon defaultWeapon;
    public Transform weaponPoint;
    public float weaponPointRadius = 1f;
    public Vector3 weaponPointDirection = new Vector3(1, 0, 0);

    private Character character;
    void Start()
    {
        character = GetComponent<Character>();
        if (defaultWeapon == null)
        {
            Debug.LogWarning("No default weapon assigned to character!");
        }
        if (weaponPoint == null)
        {
            weaponPoint = transform.Find("WeaponPoint").transform;
        }
    }

    void Update()
    {
        // If the character doesn't have a weapon equipped, equip the default weapon
        if (weapon == null)
        {
            // instantiate the default weapon and equip it
             if (defaultWeapon != null && weapon == null)
             {
                Weapon newWeapon = Instantiate(defaultWeapon, transform.position, Quaternion.identity);
                newWeapon.isDefaultWeapon = true;
                EquipWeapon(newWeapon);
             }
             return;
        } 
        LookTowards(weaponPointDirection);

        // if its a player use player scpecific controls
        if (character is Player)
        {
            PlayerControls();
        }
        else if (character is Enemy)
        {
            EnemyControls();
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        // remove current weapon if there is one
        if (weapon != null)
        {
            // delete the current weapon if its the default weapon, otherwise drop it
            if (weapon.isDefaultWeapon)
            {
                Destroy(weapon.gameObject);
            }
            else
            {
                DropWeapon();
            }
        }
        // equip new weapon
        weapon = newWeapon;
        // parent weapon to weapon point and reset local position and rotation
        weapon.transform.SetParent(weaponPoint);
        weapon.transform.localPosition = new Vector3(weaponPointRadius, 0, 0);
        weapon.transform.localRotation = Quaternion.identity;
        weapon.OnEquip();
    }

    public void DropWeapon()
    {
        if (weapon != null)
        {
            // If the weapon being dropped is the default weapon, destroy the instance instead of dropping it
            if (weapon.isDefaultWeapon)
            {
                Debug.LogWarning("Cannot drop default weapon!");
                return;
            }
            weapon.OnDropWeapon();
            weapon = null;
        }
    }
    private void PlayerControls()
    {
        weaponPointDirection = character.GetComponent<Player>().direction;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PrimaryAttack();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            SecondaryAttack();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropWeapon();
        }
    }

    private void EnemyControls()
    {
        Enemy enemy = character.GetComponent<Enemy>();
        weaponPointDirection = enemy.direction;
        return;
    }

    private void LookTowards(Vector3 dir)
    {
        dir = dir.normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Flip logic
        if (character.IsFlipped())
        {
            weaponPoint.localRotation = Quaternion.Euler(180f, 0f, 1-angle);
        }
        else
        {
            weaponPoint.localRotation = Quaternion.Euler(0, 0f, angle);
        }
    }

    public void PrimaryAttack()
    {
        if (weapon != null)
        {
            weapon.PrimaryAttack();
        }
    }

    public void SecondaryAttack()
    {
        if (weapon != null)
        {
            weapon.SecondaryAttack();

            if (weapon is Bow bow)
            {
                bow.ReleaseSecondary();
            }
        }
    }
}
