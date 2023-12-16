using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Waypoint11Binder : MonoBehaviour
{
    public GameObject finger;
    public GameObject portalVfx;
    bool posFixed; 
    float timeEnteredTrigger;
    bool prevPosFixed; // 新增变量

    // Start is called before the first frame update
    void Start()
    {
        posFixed = false;
        timeEnteredTrigger = 0f;
        var vfxComponent = portalVfx.GetComponent<VisualEffect>();
        vfxComponent.SetBool("Wall-Hand", false);
    }

    // Update is called once per frame
        // Update is called once per frame
    void Update()
{
    if (!posFixed)
    {
        if (prevPosFixed) // 如果之前的状态是true
        {
            var vfxComponent = portalVfx.GetComponent<VisualEffect>();
            if (vfxComponent != null)
            {
                vfxComponent.SetBool("Wall-Hand", true); // 设置Wall-Hand为true
            }
        }

        transform.position = finger.transform.position;
        if (timeEnteredTrigger > 0f && Time.time - timeEnteredTrigger >= 10f)
        {
            posFixed = true;
            var vfxComponent = portalVfx.GetComponent<VisualEffect>();
            if (vfxComponent != null)
            {
                vfxComponent.enabled = true;
                vfxComponent.SetBool("Hand-Stele", true); // Enable Hand-Stele
                    
            }
        }
    }

    prevPosFixed = posFixed; // 更新prevPosFixed的值
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
