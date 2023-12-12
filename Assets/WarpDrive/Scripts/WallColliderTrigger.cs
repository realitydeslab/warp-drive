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
        if (other.gameObject.name == "Head" && portalVfx.activeSelf == false)
        {
            portalVfx.SetActive(true);
            portalVfx.transform.SetPositionAndRotation(head.transform.position, head.transform.rotation);
        }
    }
}
