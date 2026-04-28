using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapdoor : MonoBehaviour
{
    /*
        Gravity based trapdoor, could be used for unstable ground?

    */

    private Collider col;
    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void Open() 
    {
        if (rigid != null)
        {
            Debug.Log("Trapdoor opened!");
            rigid.isKinematic = false;
        }
        
        Invoke(nameof(DisableCollider), 10f);
    }

    // more testing involving "jumping" to save player from death
    private void DisableCollider()
    {
        col.enabled = false;
    }
}
