using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    static private FollowCamera S;
    public static GameObject POI;
    public GameObject player;
    void Start()
    {
        S = this;
        POI = player;
    }

    void FixedUpdate()
    {
        if (POI != null)
        {
            transform.position = new Vector3(POI.transform.position.x, POI.transform.position.y, -10);
        }
    }
}
