using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : BaseState
{
    public float runTimeLength = 1.6f;
    public float jumpStrengthMultiplier = 1.75f;
    float timer = 0;
    bool exit;

    public bool WallRunAssisting = true;

    bool init = false;

    Vector3 prevNormal, currNormal, snapPos;
    CamStates camTilt;
    CameraControls camControl;

    private void Start()
    {
        myStateType = MoveStates.WALLRUN;
        camTilt = Camera.main.GetComponent<CamStates>();
        camControl = Camera.main.transform.parent.gameObject.GetComponent<CameraControls>();
    }

    void InitializeRun()
    {
        Vector3 result = (transform.forward + (transform.up * 0.1f)).normalized * controller.jumpStrength * jumpStrengthMultiplier;
        rgdBody.velocity = Vector3.ProjectOnPlane(result, controller.HorizontalHit().normal);
        controller.MultiplyGravity = false;
        rgdBody.useGravity = false;
        exit = false;
        timer = 0;
        camTilt.ResetCamera();
        if (controller.onLeftWall)
            camTilt.onRight = true;
        else if (controller.onRightWall)
            camTilt.onLeft = true;
        init = false;
    }

    public override bool Enter()
    {
        if (controller.onBottom)
            prevNormal = Vector3.zero;

        if (controller.onLeftWall || controller.onRightWall)
        {
            if (Vector3.Dot(transform.forward, rgdBody.velocity.normalized) > 0)
            {
                if (Input.GetButton("Jump") || controller.prevMoveState == myStateType && !controller.onBottom)
                {
                    if (controller.onLeftWall)
                        currNormal = controller.leftHit.normal;
                    else
                        currNormal = controller.rightHit.normal;
                    if (currNormal != prevNormal)
                    {
                        init = true;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public override void Run()
    {
        if(init)
            InitializeRun();

        if (WallRunAssisting)
        {
            //SnapToWall();
            camControl.TurnToVector(new Vector3(rgdBody.velocity.x, 0, rgdBody.velocity.z));
        }

        if (timer > 0 && Input.GetButtonDown("Jump"))
        {
            Vector3 result = (transform.forward + transform.up * 0.5f + controller.HorizontalHit().normal * 1.35f);
            rgdBody.velocity = result.normalized * controller.jumpStrength * jumpStrengthMultiplier;
            exit = true;
        }

        if (controller.HorizontalHit().normal != currNormal)
            exit = true;

        if (timer >= runTimeLength * 0.5f)
            rgdBody.useGravity = true;

        if (!controller.onLeftWall && !controller.onRightWall ||
            controller.onBottom && rgdBody.velocity.y < 0 ||
            Input.GetAxisRaw("Vertical") < 0)
            exit = true;
        timer += Time.deltaTime;
    }

    public override bool Exit()
    {
        if (exit)
        {
            prevNormal = currNormal;
            camTilt.ResetCamera();
            camTilt.onAlign = true;
            init = false;
            return true;
        }
        return false;
    }

    void SnapToWall()
    {
        snapPos = controller.HorizontalHit().point + (currNormal * controller.capsule.radius);
        transform.position = Vector3.Lerp(transform.position, snapPos, Time.deltaTime * 2);
    }
}
