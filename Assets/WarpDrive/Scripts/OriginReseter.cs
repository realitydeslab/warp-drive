using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class OriginReseter : MonoBehaviour
{
    public GameObject imageMaker;
    public GameObject cameraPose;
    public Unity.XR.CoreUtils.XROrigin XROrigin;

    public Transform lastTrackedImageTransform;
    double lastTrackedTime;

    // Start is called before the first frame    update
    void Start()
    {
        
    }

    public void OnImageTracked(Transform trackedImage)
    {
        lastTrackedImageTransform = trackedImage;
        lastTrackedTime = Time.timeAsDouble;
    }

    public void Calibrate()
    {
        if (lastTrackedImageTransform != null)
        {
            XROriginExtensions.MakeContentAppearAt(XROrigin, imageMaker.transform, lastTrackedImageTransform.position, lastTrackedImageTransform.rotation);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
