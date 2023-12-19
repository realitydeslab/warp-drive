using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Waypoint2Binder : MonoBehaviour
{
    public GameObject finger;
    public GameObject portalVfx;
    bool posFixed; 
    float timeEnteredTrigger;

    // Start is called before the first frame update
    void Start()
    {
        posFixed = false;
        timeEnteredTrigger = 0f;
        var vfxComponent = portalVfx.GetComponent<VisualEffect>();
        vfxComponent.SetBool("Wall-Hand", false);
        vfxComponent.SetBool("Hand-Stele", false);
        vfxComponent.SetBool("Information module disappears", true);
    }

    void Update()
    {
        var vfxComponent = portalVfx.GetComponent<VisualEffect>();
        if (!posFixed)
        {
            transform.position = finger.transform.position;
            if (timeEnteredTrigger > 0f && Time.time - timeEnteredTrigger >= 15f)
        {
            posFixed = true;
            if (vfxComponent != null)
            {
                vfxComponent.enabled = true;
                vfxComponent.SetBool("Hand-Stele", true); // Enable Hand-Stele
                    
            }
        }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        timeEnteredTrigger = Time.time;
        if (!posFixed)
        {
            var vfxComponent = portalVfx.GetComponent<VisualEffect>();
            if (vfxComponent != null)
            {
                vfxComponent.SetBool("Wall-Hand", true); // 设置Wall-Hand为true
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "trigger1" || other.gameObject.tag == "trigger2" || other.gameObject.tag == "trigger3")
        {
            timeEnteredTrigger = 0f;
        }
    }
}


