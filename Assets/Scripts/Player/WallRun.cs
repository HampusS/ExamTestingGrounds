using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : BaseState
{
    float timer = 0;
    float timeSpan = 1.0f;
    float runHeight = 45;
    float runSpeed = 10;
    float runTimeMultiplier = 2f;
    float jumpStrength = 8f;
    float jumpHeight = 200;
    Vector3 prevNormal, currNormal;



    private void Start()
    {
        myStateType = MoveStates.WALLRUN;
        currNormal = Vector3.left;
    }

    void InitializeRun()
    {
        SetMoveAmount(Vector3.ProjectOnPlane(transform.forward * runSpeed, currNormal));
        GetComponent<Renderer>().material.color = Color.green;
        controller.onGravityMultiplier = false;
        EnableGravity(false);
        Jump(runHeight);
        timer = 0;
        PlayerCameraControls cam = GetComponent<PlayerCameraControls>();

        if (controller.onLeftWall)
            cam.TiltCameraRight();
        else
            cam.TiltCameraLeft();
    }

    public override bool Enter()
    {
        if (controller.onBottom)
            prevNormal = Vector3.zero;

        if (controller.onLeftWall || controller.onRightWall)
        {
            if (Input.GetButton("Jump") && Input.GetAxisRaw("Vertical") > 0)
            {

                currNormal = controller.HorizontalHit().normal;

                if (currNormal != prevNormal)
                {
                    InitializeRun();
                    return true;
                }
            }
        }

        return false;
    }

    public override void Run()
    {
        if (timer > 0 && Input.GetButtonDown("Jump"))
        {
            JumpFromWall((transform.forward + (controller.HorizontalHit().normal * 0.5f)).normalized, jumpHeight, jumpStrength);

            timer += timeSpan;
        }

        if (!controller.onLeftWall && !controller.onRightWall)
        {
            timer += timeSpan;
        }

        if (Input.GetButton("Jump"))
            timer += Time.deltaTime;
        else
            timer += Time.deltaTime * runTimeMultiplier;
    }

    public override bool Exit()
    {
        if (timer >= timeSpan)
        {
            GetComponent<Renderer>().material.color = Color.red;
            prevNormal = currNormal;
            EnableGravity(true);
            timer = 0;
            return true;
        }
        return false;
    }

}
