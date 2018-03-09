using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : BaseState
{
    float timer;
    float timeSpan = 1.0f;
    float runHeight = 45;

    Vector3 prevNormal, currNormal;

    private void Start()
    {
        myStateType = MoveStates.WALLRUN;
        currNormal = Vector3.left;
        timer = 0;
    }

    void InitializeRun()
    {
        controller.SetMoveAmount(Vector3.ProjectOnPlane(controller.FinalMove, currNormal));
        GetComponent<Renderer>().material.color = Color.green;
        controller.Jump(transform.up * runHeight);
        controller.EnableGravity(false);
        timeSpan = 1.2f;
        timer = 0;
    }

    public override bool Enter()
    {
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
    }

    public override bool Exit()
    {
        if (timer >= timeSpan)
        {
            GetComponent<Renderer>().material.color = Color.red;
            controller.EnableGravity(true);
            prevNormal = currNormal;
            timer = 0;
            return true;
        }
        return false;
    }

}
