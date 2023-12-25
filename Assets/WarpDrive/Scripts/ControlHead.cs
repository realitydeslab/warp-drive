using UnityEngine;
using UnityEngine.Playables;
using OscJack;

public class ControlHead : MonoBehaviour
{
    public GameObject Head;
    public GameObject Finger;

    private Vector3 HeadPosition;
    private Vector3 FingerPosition;

    void Update()
    {
        Head.transform.position = HeadPosition;
        Finger.transform.position = FingerPosition;
    }

    public void SetHeadPositionX(float position1)
    {
        HeadPosition.x = position1;
    }

    public void SetHeadPositionY(float position2)
    {
        HeadPosition.y = position2;
    }

    public void SetHeadPositionZ(float position3)
    {
        HeadPosition.z = position3;
    } 

    public void SetFingerPositionX(float position4)
    {
        FingerPosition.x = position4;
    }
    public void SetFingerPositionY(float position5)
    {
        FingerPosition.y = position5;
    }
    public void SetFingerPositionZ(float position6)
    {
        FingerPosition.z = position6;
    }
}







