using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponController : MonoBehaviour
{
    [Header("Weapon Settings")]
    public Weapon weapon;
    public Weapon defaultWeapon;
    public Transform weaponPoint;
    
    private Character character;
    void Start()
    {
        character = GetComponent<Character>();
        if (defaultWeapon == null)
        {
            Debug.LogWarning("No default weapon assigned to character!");
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

            // Flip weapon if facing left
        if (character.IsFlipped())
        {
            // Switch weaponPoint to other side
            weaponPoint.localPosition = new Vector3(Mathf.Abs(weaponPoint.localPosition.x) * -1, weaponPoint.localPosition.y, 0);

            // Rotate the point 180 degrees
            weaponPoint.localRotation = new Quaternion(weapon.transform.localRotation.x, 180, weapon.transform.localRotation.z, weapon.transform.localRotation.w);
        }
        else
        {
            // Switch position back
            weaponPoint.localPosition = new Vector3(Mathf.Abs(weaponPoint.localPosition.x), weaponPoint.localPosition.y, 0);
            // switch rotation back
            weaponPoint.localRotation = new Quaternion(weapon.transform.localRotation.x, 0, weapon.transform.localRotation.z, weapon.transform.localRotation.w);
        }

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
        weapon.transform.localPosition = Vector3.zero;
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            weapon.PrimaryAttack();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            weapon.SecondaryAttack();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropWeapon();
        }
    }

    private void EnemyControls()
    {
        return;
    }
}
