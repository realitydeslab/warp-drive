using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.VFX;

public class FingerControl : MonoBehaviour
{
    public GameObject finger;
    public GameObject Originfinger;
    public GameObject portalVfx;
    private Vector3 lastFingerPosition;

    // Start is called before the first frame update
    void Start()
    {
        finger.SetActive(false);
        finger.GetComponent<ParentConstraint>().constraintActive = true;
        lastFingerPosition = Originfinger.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Originfinger.transform.position != lastFingerPosition && portalVfx.activeSelf)
        {
            finger.SetActive(true);
            portalVfx.GetComponent<VisualEffect>().SetBool("Wall-Hand", true); 
        }
        lastFingerPosition = Originfinger.transform.position;
    }
}
