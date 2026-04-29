using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    private Renderer[] renderers;
    void Start()
    {
        // Get every visable thing in the tree from the parent to highlight
        renderers = GetComponentsInChildren<Renderer>();
        Rigidbody rigid = GetComponentInChildren<Rigidbody>();
        if (rigid != null)
        {
            rigid.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        }
    }

    public void Highlight()
    {
        // Highlight every renderer
        foreach (Renderer rend in renderers)
        {
            rend.material.EnableKeyword("_EMISSION");
            rend.material.SetColor("_EmissionColor", Color.yellow * 2f);
        }
    }

    public void ResetHighlight()
    {
        // Disable highlight on every renderer
        foreach (Renderer rend in renderers)
        {
            rend.material.SetColor("_EmissionColor", Color.black);
        }
    }
}
