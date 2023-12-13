using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WaypointBinder : MonoBehaviour
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
        //var vfxComponent = portalVfx.GetComponent<VisualEffect>();
        //vfxComponent.SetBool("Hand-Stele", true);
    }

    // Update is called once per frame
        // Update is called once per frame
    void Update()
    {
        if (!posFixed)
        {
            transform.position = finger.transform.position;
            if (timeEnteredTrigger > 0f && Time.time - timeEnteredTrigger >= 10f)
            {
                posFixed = true;
                var vfxComponent = portalVfx.GetComponent<VisualEffect>();
                if (vfxComponent != null)
                {
                    vfxComponent.enabled = true;
                    vfxComponent.SetBool("Hand-Stele", true); // Enable Hand-Stele
                    StartCoroutine(DisableVFXAfterSeconds(1f, vfxComponent));
                }
            }
        }
    }

    private IEnumerator DisableVFXAfterSeconds(float seconds, VisualEffect vfxComponent)
    {
        yield return new WaitForSeconds(seconds);
        vfxComponent.SetBool("Wall-Hand", false); // Disable Wall-Hand after 1 second
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "trigger1" || other.gameObject.tag == "trigger2" || other.gameObject.tag == "trigger3")
        {
            timeEnteredTrigger = Time.time;
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
