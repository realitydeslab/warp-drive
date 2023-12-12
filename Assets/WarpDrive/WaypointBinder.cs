using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointBinder : MonoBehaviour
{
    public GameObject finger;
    bool posFixed; 

    // Start is called before the first frame update
    void Start()
    {
        posFixed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!posFixed)
        {
            transform.position = finger.transform.position;
        }
    }
}
