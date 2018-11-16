﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public static CameraControls Instance;
    [SerializeField]
    float sensX = 5f;
    [SerializeField]
    float sensY = 5f;

    Transform player;

    float camUpDown;
    float camLeftRight;

    public float turnAssistSpeed = 3;
    float assistDelay = 0.25f;
    float assistTimer;

    public bool LockTurning { get; set; }
    public bool Disable { get; set; }

    // Use this for initialization
    void Start()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        LockTurning = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Disable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Disable)
        {
            camUpDown += Input.GetAxis("Mouse Y") * sensY;
            camUpDown = Mathf.Clamp(camUpDown, -65, 70);
            transform.localRotation = Quaternion.AngleAxis(-camUpDown, Vector3.right);

            if (!LockTurning)
            {
                camLeftRight = Input.GetAxis("Mouse X") * sensX;
                player.Rotate(Vector3.up, camLeftRight);
            }
        }
    }

    public void TurnToVector(Vector3 target)
    {
        if (target.magnitude > 0)
        {
            assistTimer += Time.deltaTime;
            bool assist = Input.GetAxis("Mouse X") == 0;
            if (assist)
            {
                if (assistTimer > assistDelay)
                {
                    player.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(target), Time.deltaTime * turnAssistSpeed);
                }
            }
            else
                assistTimer = 0;
        }
    }

    public void TiltCameraUpDown(Vector3 target)
    {
        Vector3 plane = Vector3.Cross(transform.forward, transform.up);
        float speed = 8 * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, target, speed, 0);
        newDir = Vector3.ProjectOnPlane(newDir, plane);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newDir), Time.deltaTime * 5 * turnAssistSpeed);
        camUpDown = 0;
        //Debug.Log(camUpDown);
    }

    public void CrouchCam()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
    }

    public void StandCam()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, 0.5f, transform.localPosition.z);
    }
}
