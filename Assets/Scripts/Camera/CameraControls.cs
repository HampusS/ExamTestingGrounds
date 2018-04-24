using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
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

    // Use this for initialization
    void Start()
    {
        LockTurning = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        camUpDown += Input.GetAxis("Mouse Y") * sensY;
        camUpDown = Mathf.Clamp(camUpDown, -65, 65);
        transform.localRotation = Quaternion.AngleAxis(-camUpDown, Vector3.right);

        if (!LockTurning)
        {
            camLeftRight = Input.GetAxis("Mouse X") * sensX;
            player.Rotate(Vector3.up, camLeftRight);
        }
    }

    public void TurnToVector(Vector3 target)
    {
        assistTimer += Time.deltaTime;
        bool assist = Input.GetAxis("Mouse X") == 0;
        if (assist)
        {
            if (assistTimer > assistDelay)
                player.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(target), Time.deltaTime * turnAssistSpeed);
        }
        else
            assistTimer = 0;
    }


}
