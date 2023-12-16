using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class WallColliderTrigger : MonoBehaviour
{
    public GameObject head;
    public GameObject portalVfx;

    // Start is called before the first frame update
     void Start()
    {
        portalVfx.SetActive(false);        
    }

    private void OnTriggerEnter(Collider other)
{
    if (other.gameObject.CompareTag("Player") && portalVfx.activeSelf == false)
    {
        
        float newY = head.transform.position.y - 0.2f; // Get the y position of the camera and subtract 0.2

        if (gameObject.tag == "trigger1")
        {
            Vector3 newPosition = new Vector3(7.71f, newY, other.transform.position.z);
            portalVfx.transform.position = newPosition;
            portalVfx.SetActive(true);
        }
        else if (gameObject.tag == "trigger2")
        {
            Vector3 newPosition = new Vector3(other.transform.position.x, newY, -6.63f);
            portalVfx.transform.position = newPosition;
            portalVfx.SetActive(true);
        }
        else if (gameObject.tag == "trigger3")
        {
            Vector3 newPosition = new Vector3(-7.67f, newY, other.transform.position.z);
            portalVfx.transform.position = newPosition;
            portalVfx.SetActive(true);
        }
    }
}
}
