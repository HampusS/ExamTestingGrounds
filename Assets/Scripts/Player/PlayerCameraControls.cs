using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControls : MonoBehaviour {
    [SerializeField]
    float sensX = 5f;
    [SerializeField]
    float sensY = 5f;
    Transform cam;
    float camUpDown;
    float camLeftRight;

    // Use this for initialization
    void Start () {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update () {
        camUpDown += Input.GetAxis("Mouse Y") * sensY;
        camUpDown = Mathf.Clamp(camUpDown, -65, 65);
        cam.localRotation = Quaternion.AngleAxis(-camUpDown, Vector3.right);

        camLeftRight = Input.GetAxis("Mouse X") * sensX;
        transform.Rotate(Vector3.up, camLeftRight);
    }

    public void TiltCameraLeft()
    {
        cam.transform.rotation = Quaternion.Euler(20, 0, 0);
        Debug.Log("Left" + cam.transform.rotation);
    }

    public void TiltCameraRight()
    {
        cam.transform.rotation = Quaternion.Euler(-20, 0, 0);
        Debug.Log("Right" + cam.transform.rotation);
    }

    public void Align()
    {

    }
}
