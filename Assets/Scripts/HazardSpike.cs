using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardSpike : MonoBehaviour
{
    /*
        This is a simple spike script, inflicts damage as well as a bit of knockback.
    */
    
    [SerializeField] private int damage = 15;

    private void OnTriggerEnter(Collider other)
    {
        // assigns player variable
        Player player_ = other.GetComponentInParent<Player>();

        if (player_ != null)
        {
            // this reads the direction the player is coming from
            Vector3 direction = player_.transform.position - transform.position;

            // this line passes the damage amount and direction from the previous line into the TakeDamage script from the player
            player_.TakeDamage(damage, direction);
        }
    }
}
