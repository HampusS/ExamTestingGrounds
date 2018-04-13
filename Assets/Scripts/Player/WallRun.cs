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
    bool initOnce;

    TiltCamera cam;


    private void Start()
    {
        myStateType = MoveStates.WALLRUN;
        cam = Camera.main.GetComponent<TiltCamera>();
    }

    void InitializeRun()
    {
        if (initOnce)
        {
            SetMoveAmount(Vector3.ProjectOnPlane(transform.forward * runSpeed, currNormal));
            GetComponent<Renderer>().material.color = Color.green;
            controller.onGravityMultiplier = false;
            EnableGravity(false);
            initOnce = false;
            Jump(runHeight);
            timer = 0;
            cam.ResetCamera();
            if (controller.onLeftWall)
                cam.Right = true;
            else if (controller.onRightWall)
                cam.Left = true;

        }
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
                    initOnce = true;
                    return true;
                }
            }
        }

        return false;
    }

    public override void Run()
    {
        InitializeRun();

        if (timer > 0 && Input.GetButtonDown("Jump"))
        {
            JumpFromWall((transform.forward + controller.HorizontalHit().normal).normalized, jumpHeight, jumpStrength);

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
            cam.ResetCamera();
            return true;
        }
        return false;
    }

}
