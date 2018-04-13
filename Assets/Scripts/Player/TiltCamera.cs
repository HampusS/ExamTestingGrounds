using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltCamera : MonoBehaviour
{

    float speed = 2;

    public bool Left { get; set; }
    public bool Right { get; set; }


    // Use this for initialization
    void Start()
    {

    }

    private void Update()
    {
        if (Left)
            TiltCameraLeft();
        else if (Right)
            TiltCameraRight();
        else
            AlignCamera();
    }

    void TiltCameraLeft()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.AngleAxis(20, Vector3.forward), Time.deltaTime * speed);
        if (transform.localRotation == Quaternion.AngleAxis(20, Vector3.forward))
            Left = false;
    }

    void TiltCameraRight()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.AngleAxis(-20, Vector3.forward), Time.deltaTime * speed);
        if (transform.localRotation == Quaternion.AngleAxis(-20, Vector3.forward))
            Right = false;
    }

    void AlignCamera()
    {
        if (transform.localRotation != Quaternion.AngleAxis(0, Vector3.forward))
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.AngleAxis(0, Vector3.forward), Time.deltaTime * speed);
    }

    public void ResetCamera()
    {
        Left = false;
        Right = false;
    }
}
