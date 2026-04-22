using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    private Renderer[] renderers;
    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void highlight()
    {
        foreach (Renderer rend in renderers)
        {
            rend.material.EnableKeyword("_EMISSION");
            rend.material.SetColor("_EmissionColor", Color.yellow * 2f);
        }
    }

    public void resetHighlight()
    {
        foreach (Renderer rend in renderers)
        {
            rend.material.SetColor("_EmissionColor", Color.black);
        }
    }
}
