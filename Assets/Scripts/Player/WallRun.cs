using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : BaseState
{
    public float timeBeforeFall = 1.6f;
    float runTimeMultiplier = 2f;
    float timer = 0;
    bool initOnce;
    bool exit;

    Vector3 prevNormal, currNormal;
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
            controller.onGravityMultiplier = false;
            rgdBody.useGravity = false;
            initOnce = false;
            exit = false;
            timer = 0;
            rgdBody.velocity = Vector3.zero;
            Vector3 result = (transform.forward + transform.up * 0.15f).normalized * controller.jumpStrength;
            rgdBody.velocity = Vector3.ProjectOnPlane(result, controller.HorizontalHit().normal);
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
            Vector3 result = (transform.forward + transform.up * 0.75f + controller.HorizontalHit().normal).normalized * controller.jumpStrength;
            rgdBody.velocity = result;
            exit = true;
        }

        if (controller.HorizontalHit().normal != currNormal)
            exit = true;

        if (Input.GetButton("Jump"))
            timer += Time.deltaTime;
        else
            timer += Time.deltaTime * runTimeMultiplier;

        if (timer >= timeBeforeFall * 0.5f)
            rgdBody.useGravity = true;

        if (!controller.onLeftWall && !controller.onRightWall || controller.onBottom && rgdBody.velocity.y < 0)
            exit = true;
    }

    public override bool Exit()
    {
        if (exit)
        {
            prevNormal = currNormal;
            cam.ResetCamera();
            return true;
        }
        return false;
    }

}
