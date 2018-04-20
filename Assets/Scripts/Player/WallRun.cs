using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : BaseState
{
    float timer = 0;
    float timeSpan = 1.0f;
    float runHeight = 2;
    float runTimeMultiplier = 2f;

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
            rgdBody.velocity = Vector3.zero;
            rgdBody.AddForce(Vector3.ProjectOnPlane(transform.forward * controller.moveSpeed, currNormal), ForceMode.VelocityChange);
            rgdBody.AddForce(transform.up * controller.jumpHeight, ForceMode.Impulse);

            controller.onGravityMultiplier = false;
            rgdBody.useGravity = false;
            initOnce = false;
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
            Vector3 result = (transform.forward + controller.HorizontalHit().normal + (transform.up * 0.5f)).normalized;
            rgdBody.velocity = Vector3.Project(rgdBody.velocity, result);
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
            prevNormal = currNormal;
            rgdBody.useGravity = true;
            cam.ResetCamera();
            timer = 0;
            return true;
        }
        return false;
    }

}
