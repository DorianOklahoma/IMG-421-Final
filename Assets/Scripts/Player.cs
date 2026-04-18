using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    public int health = 100;
    public float blockPercentage = 0f;
    public float interactRadius = 5f;

    [Header("Movement Settings")]
    public float speed = 5.0f;
    public float maxSpeed = 5.0f;
    public float acceleration = 200.0f;
    public float airAcceleration = 20.0f;

    private Rigidbody rigid;
    private WeaponController wc;
    private GameObject lastHit = null;
    private GameObject hitObject = null;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        wc = GetComponent<WeaponController>();
    }

    void Update()
    {
        /* Movement Code */
        // Move the player using AD keys
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float targetSpeed = moveHorizontal * maxSpeed;
        if (rigid.velocity.y == 0)
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
        if (Input.GetKeyDown(KeyCode.W) && rigid.velocity.y == 0)
        {
            rigid.AddForce(Vector3.up * speed, ForceMode.Impulse);
        }

        /* Interaction Code */
        highlightInteractable();

        if (Input.GetKeyDown(KeyCode.E) && hitObject != null)
        {
            interact();
        }
    }

    private GameObject GetInteractedObject()
    {
        Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position + new Vector3(0, 0, 10)).normalized;

        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, interactRadius);

        // Sort by closest
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (var hit in hits)
        {
            GameObject obj = hit.collider.gameObject;

            // Skip equipped weapon (including children)
            if (wc.weapon != null && obj.transform.IsChildOf(wc.weapon.transform))
                continue;

            // Skip player
            if (obj == gameObject)
                continue;

            if (isInteractable(obj))
                return obj;
        }

        return null;
    }
    private void highlightInteractable()
    {
        hitObject = GetInteractedObject();

        if (hitObject != lastHit)
        {
            // Remove highlight from previous
            if (lastHit != null)
            {
                lastHit.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
            }

            // Apply highlight to new one
            if (hitObject != null && isInteractable(hitObject))
            {
                Renderer rend = hitObject.GetComponent<Renderer>();
                rend.material.EnableKeyword("_EMISSION");
                rend.material.SetColor("_EmissionColor", Color.yellow * 2f);
            }

            lastHit = hitObject;
        }

        // If nothing hit, clear highlight
        if (hitObject == null && lastHit != null)
        {
            lastHit.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
            lastHit = null;
        }

        // Debug ray
        Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) 
                            - transform.position + new Vector3(0, 0, 10)).normalized;

        Debug.DrawRay(transform.position, direction * interactRadius, Color.green);
    }

    private void interact()
    {
        if (hitObject.GetComponent<Weapon>() != null)
        {
            wc.EquipWeapon(hitObject.GetComponent<Weapon>());
        }
    }

    private bool isInteractable(GameObject obj)
    {
        if (obj.GetComponent<Weapon>() != null)
        {
            return true;
        }
        return false;
    }

    public void TakeDamage(int damage = 0)
    {
        float blockedDamage = damage * blockPercentage;
        health -= damage - (int)blockedDamage;
    }
}
