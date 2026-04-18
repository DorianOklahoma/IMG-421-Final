using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fists : Weapon
{
    public override void usePrimary()
    {
        base.usePrimary();
        // Throw a punch
        Debug.Log("WHAM!");
    }

    public override void useSecondary()
    {
        base.useSecondary();
        // Block
        Debug.Log("Blocking!");
    }

}
