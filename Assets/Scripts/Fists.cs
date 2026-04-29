using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an example weapon
public class Fists : Weapon
{
    public override void UsePrimary()
    {
        // Use base weapon behavior
        base.UsePrimary();
        // Throw a punch
        Debug.Log("WHAM!");
    }

    public override void UseSecondary()
    {
        // Use base weapon behavior
        base.UseSecondary();
        // Block
        Debug.Log("Blocking!");
    }

}
