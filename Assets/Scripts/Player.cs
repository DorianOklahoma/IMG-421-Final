using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : Character
{
    [Header("Player Settings")]
    public float interactRadius = 5f;

    [Header("Dynamic Settings")]
    public Vector3 direction = Vector3.zero;

    private GameObject hitObject = null;
    private GameObject lastHitObject = null;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        // Do base function
        base.Update();

        // Update the direction the player is looking according to the mouse
        direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position + new Vector3(0, 0, 10)).normalized;
        
        /* Movement Code */
        // Move the player using AD keys
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float targetSpeed = moveHorizontal * maxSpeed;
        if (IsGrounded())
        {
            // If the player is on the ground, apply full acceleration
            float newSpeed = Mathf.MoveTowards(rigid.velocity.x, targetSpeed, acceleration * Time.deltaTime);
            rigid.velocity = new Vector3(newSpeed, rigid.velocity.y, 0);
        }
        else
        {
            // If the player is in the air, apply a slower acceleration to allow for better air control
            float newSpeed = Mathf.MoveTowards(rigid.velocity.x, targetSpeed, airAcceleration * Time.deltaTime);
            rigid.velocity = new Vector3(newSpeed, rigid.velocity.y, 0);
        }
        // Jump only when the player is on the ground
        if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
        {
            rigid.AddForce(Vector3.up * speed, ForceMode.Impulse);
        }

        // Animate player
        Animate();

        /* Interaction Code */
        hitObject = GetInteractedObject();
        SetInteractedHighlight();

        if (Input.GetKeyDown(KeyCode.E) && hitObject != null)
        {
            Interact();
        }

        // Interaction debug ray (Remove for final game)
        Debug.DrawRay(transform.position, direction * interactRadius, Color.green);
    }

    private GameObject GetInteractedObject()
    {
        // Get the closest interactable object in the direction the player is facing
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, interactRadius);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        foreach (var hit in hits)
        {
            GameObject obj = hit.collider.gameObject;
            Object mainObject = obj.GetComponentInParent<Object>();

            if (mainObject != null)
            {
                obj = mainObject.GameObject();
            }

            // Skip equipped weapon (including children)
            if (wc.weapon != null && obj == wc.weapon.gameObject)
                continue;

            // Skip player
            if (obj == gameObject)
                continue;

            if (obj.GetComponent<Object>() != null)
                return obj;
        }

        return null;
    }

    private void SetInteractedHighlight()
    {
        // If hit a different object (or nothing), remove previous highlight
        if (lastHitObject != hitObject)
        {
            if (lastHitObject != null)
            {
                lastHitObject.GetComponent<Object>().ResetHighlight();
            }
            if (hitObject != null){
                hitObject.GetComponent<Object>().Highlight();
            }
            lastHitObject = hitObject;
        }
    }

    private void Interact()
    {
        if (hitObject == null) return;
        // If the Object is a weapon, equip it
        if (hitObject.GetComponent<Weapon>() != null)
        {
            wc.EquipWeapon(hitObject.GetComponent<Weapon>());
        }

    }

    public void TakeDamage(int damage = 0)
    {
        float blockedDamage = damage * blockPercentage;
        health -= damage - (int)blockedDamage;
    }

    protected override void SetFacing()
    {
        if (direction.x < 0)
            currentFacing = Facing.left;
        else if (direction.x > 0)
            currentFacing = Facing.right;
    }

    public override float GetHorizontalDirection()
    {
        return Input.GetAxisRaw("Horizontal");
    }
}
