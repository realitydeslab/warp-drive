using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using HoloKit;

public class OriginReseter : MonoBehaviour
{
    public GameObject imageMaker;

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

            var r = Matrix4x4.Rotate(lastTrackedImageTransform.rotation).transpose *
                    Matrix4x4.Rotate(imageMaker.transform.rotation);
            var a =  (r.m00 + r.m22);
            var b =  (-r.m20 + r.m02);
             
            float thetaInDeg =  Mathf.Atan2(b, a) / Mathf.Deg2Rad;

            Matrix4x4 rotation = Matrix4x4.Rotate(Quaternion.AngleAxis(thetaInDeg, Vector3.up));

            Vector3 translate = -rotation.MultiplyPoint3x4(lastTrackedImageTransform.position - imageMaker.transform.position);
            
            Debug.Log(translate);
            Debug.Log(thetaInDeg);
            HoloKitARSessionControllerAPI.ResetOrigin(translate, Quaternion.AngleAxis(thetaInDeg, Vector3.up));
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
