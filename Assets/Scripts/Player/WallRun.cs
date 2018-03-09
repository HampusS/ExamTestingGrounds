using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : BaseState
{
    float timer, timeSpan;
    float runHeight = 60;
    bool running;

    Vector3 prevNormal, currNormal;

    private void Start()
    {
        myStateType = MoveStates.WALLRUN;
        timer = 0;
        timeSpan = 1.2f;
        currNormal = Vector3.left;
    }

    void InitializeRun()
    {
        GetComponent<Renderer>().material.color = Color.green;
        prevNormal = currNormal;
        currNormal = controller.HorizontalHit().normal;
        running = true;
        timer = 0;
    }

    public override bool Enter()
    {
        if (Input.GetButton("Jump") && Input.GetAxisRaw("Vertical") > 0)
        {
            if (controller.onLeftWall || controller.onRightWall || controller.onForwardWall)
            {
                if (currNormal != prevNormal)
                {
                    InitializeRun();

                    if (controller.onForwardWall)
                    {
                        runHeight = 100;
                        timeSpan = 0.8f;
                        controller.UpdateMoveAmount(0, 0);
                    }
                    else
                    {
                        runHeight = 50;
                        timeSpan = 1.2f;
                        controller.SetMoveAmount(Vector3.ProjectOnPlane(controller.FinalMove, currNormal));
                    }

                    return true;
                }
                //else if (Input.GetButtonDown("Jump"))
                //{
                //    controller.Jump(currNormal * 400);
                //    controller.Jump(transform.up * 200);
                //}
            }
        }
        return false;
    }

    public override void Run()
    {
        if (timer > 0 && Input.GetButtonDown("Jump"))
        {
            controller.Jump(currNormal * 400);
            controller.Jump(transform.up * 200);
            timer += timeSpan;
        }

        if (!controller.onLeftWall && !controller.onRightWall)
        {
            timer += timeSpan;
        }

        if (Input.GetButton("Jump"))
            timer += Time.deltaTime;
        else
            timer += Time.deltaTime * 2;

        if (running)
        {
            controller.rgdBody.useGravity = false;
            controller.Jump(transform.up * runHeight);
            running = false;
        }
    }

    public override bool Exit()
    {
        if (timer >= timeSpan)
        {
            GetComponent<Renderer>().material.color = Color.red;
            controller.rgdBody.useGravity = true;
            currNormal = Vector3.zero;
            timer = 0;
            return true;
        }
        return false;
    }

}
