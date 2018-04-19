using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimb : BaseState
{
    float timer = 0;
    float timeSpan = 0.8f;
    public float runHeight = 100;

    bool initOnce;
    bool turning;
    Vector3 prevNormal, currNormal;

    // Use this for initialization
    void Start()
    {
        myStateType = MoveStates.WALLCLIMB;
    }

    private void InitializeRun()
    {
        if (initOnce)
        {
            controller.onGravityMultiplier = false;
            //controller.UpdateMoveAmount(0, Vector3.zero);
            EnableGravity(false);
            controller.Jump(runHeight);
            turning = false;
            initOnce = false;
            timer = 0;
        }
    }

    public override bool Enter()
    {
        if (controller.onBottom)
            prevNormal = Vector3.zero;

        if (controller.onForwardWall && !inReachOfLedge())
        {
            currNormal = controller.HorizontalHit().normal;
            if (currNormal != prevNormal && Input.GetButton("Jump"))
            {
                initOnce = true;
                return true;
            }
        }

        return false;
    }

    public override void Run()
    {
        InitializeRun();
        if (!turning)
        {
            if (timer > 0 && Input.GetButtonDown("Jump"))
            {
                controller.Jump(0);
                turning = true;
            }

            if (Input.GetButton("Jump"))
                timer += Time.deltaTime;
            else
                timer += Time.deltaTime * 2;

            if (inReachOfLedge())
                timer += timeSpan;
        }
        else if (TurnTowardsVector(controller.turnAroundSpeed, controller.HorizontalHit().normal))
        {
            //controller.JumpFromWall(controller.jumpHeight, controller.jumpStrength);
            timer += timeSpan;
            turning = false;
        }

    }

    public override bool Exit()
    {
        if (timer >= timeSpan)
        {
            rgdBody.velocity = Vector3.zero;
            prevNormal = currNormal;
            EnableGravity(true);
            timer = 0;
            return true;
        }
        return false;
    }



}
