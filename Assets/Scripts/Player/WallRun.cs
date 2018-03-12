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
        controller.SetMoveAmount(Vector3.ProjectOnPlane(transform.forward * runSpeed, currNormal));
        GetComponent<Renderer>().material.color = Color.green;
        controller.Jump(runHeight);
        controller.EnableGravity(false);
        timer = 0;
    }

    public override bool Enter()
    {
        if (controller.onLeftWall || controller.onRightWall)
        {
            if (Input.GetButton("Jump") && Input.GetAxisRaw("Vertical") > 0)
            {
                if (controller.onBottom)
                    prevNormal = Vector3.zero;

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
        WallJump();

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
            controller.EnableGravity(true);
            prevNormal = currNormal;
            timer = 0;
            return true;
        }
        return false;
    }

    private void WallJump()
    {
        if (timer > 0 && Input.GetButtonDown("Jump"))
        {
            controller.UpdateMoveAmount(currNormal, jumpStrength);
            controller.Jump(jumpHeight);

            timer += timeSpan;
        }
    }

}
