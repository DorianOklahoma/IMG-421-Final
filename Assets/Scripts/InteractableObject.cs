using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;
    public UnityEvent onInteract;

    [Header("Highlight")]
    public Color highlightColor = Color.yellow;
    public Color normalColor = Color.white;

    private Renderer[] renderers;
    private bool playerInRange = false;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        ResetHighlight();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            Interact();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Highlight();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            ResetHighlight();
        }
    }

    void Highlight()
    {
        foreach (Renderer rend in renderers)
        {
            rend.material.color = highlightColor;
        }
    }

    void ResetHighlight()
    {
        foreach (Renderer rend in renderers)
        {
            rend.material.color = normalColor;
        }
    }

    void Interact()
    {
        Debug.Log("Interacted with: " + gameObject.name);
        onInteract.Invoke();
    }
}
