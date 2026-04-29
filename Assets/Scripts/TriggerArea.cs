using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    /* Should be able to use this as a simple trap trigger area prefab,
       Only requirement would be to hardcode the signal.
    */

    // random signal to test
    public bool isActive = false;


    // TESTING TRAPDOOR
    [SerializeField]
    private Trapdoor trapdoor;
    private float delay = 0.5f;

    // activates when player is inside area
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Area Activated!");
            isActive = true;
            Invoke(nameof(OpenTrapdoor), delay);
        }
    }

    // activated when player leaves area (could be used for pressure plates)
    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Area Deactivated!");
            isActive = false;
        }
    }

    // signal script
    private void OpenTrapdoor()
    {
        trapdoor.Open();
    }
    
}
