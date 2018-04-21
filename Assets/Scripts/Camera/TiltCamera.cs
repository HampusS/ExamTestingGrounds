using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltCamera : MonoBehaviour
{
    [SerializeField]
    float lerpSpeed = 2;
    [SerializeField]
    float degrees = 20;

    public bool Left { get; set; }
    public bool Right { get; set; }

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
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.AngleAxis(degrees, Vector3.forward), Time.deltaTime * lerpSpeed);
    }

    void TiltCameraRight()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.AngleAxis(-degrees, Vector3.forward), Time.deltaTime * lerpSpeed);
    }

    void AlignCamera()
    {
        if (transform.localRotation != Quaternion.AngleAxis(0, Vector3.forward))
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.AngleAxis(0, Vector3.forward), Time.deltaTime * lerpSpeed);
    }

    public void ResetCamera()
    {
        Left = false;
        Right = false;
    }
}
