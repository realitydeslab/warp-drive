using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.VFX;

public class FingerControl : MonoBehaviour
{
    public GameObject finger;
    public GameObject originFinger;
    public GameObject portalVfx1;
    public GameObject portalVfx2;
    public GameObject portalVfx3;
    private Vector3 lastFingerPosition;

    // Start is called before the first frame update
    void Start()
    {
        finger.SetActive(false);
        finger.GetComponent<ParentConstraint>().constraintActive = true;
        lastFingerPosition = originFinger.transform.position;
    }

    // Update is called once per frame
    void Update()
{
    if (originFinger.transform.position != lastFingerPosition && portalVfx1.activeSelf)
    {
        finger.SetActive(true);
        portalVfx1.GetComponent<VisualEffect>().SetBool("Wall-Hand", true); 
        lastFingerPosition = originFinger.transform.position;
    }
    if (originFinger.transform.position != lastFingerPosition && portalVfx2.activeSelf)
    {
        finger.SetActive(true);
        portalVfx2.GetComponent<VisualEffect>().SetBool("Wall-Hand", true); 
        lastFingerPosition = originFinger.transform.position;
    }
    if (originFinger.transform.position != lastFingerPosition && portalVfx3.activeSelf)
    {
        finger.SetActive(true);
        portalVfx3.GetComponent<VisualEffect>().SetBool("Wall-Hand", true); 
        lastFingerPosition = originFinger.transform.position;
    }
}
}
