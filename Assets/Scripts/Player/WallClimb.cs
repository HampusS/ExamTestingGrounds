using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimb : BaseState
{
    float timer = 0;
    float timeSpan = 0.8f;
    bool initOnce;
    bool turning;
    bool exit;
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
            rgdBody.useGravity = false;
            initOnce = false;
            turning = false;
            exit = false;
            timer = 0;
            rgdBody.velocity = Vector3.zero;
            rgdBody.AddForce(Vector3.up * controller.jumpHeight * 0.75f, ForceMode.VelocityChange);
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
            if (Input.GetButton("Jump"))
                timer += Time.deltaTime;
            else
                timer += Time.deltaTime * 2;

            if (timer >= timeSpan)
            {
                rgdBody.useGravity = true;
            }

            if (controller.onBottom && rgdBody.velocity.y < 0)
                exit = true;


            if (timer > 0 && Input.GetButtonDown("Jump"))
            {
                controller.onGravityMultiplier = false;
                rgdBody.useGravity = false;
                rgdBody.velocity = Vector3.zero;
                turning = true;
            }

            if (inReachOfLedge())
                exit = true;
        }
        else if (TurnTowardsVector(controller.turnAroundSpeed, controller.HorizontalHit().normal))
        {
            Vector3 result = (controller.HorizontalHit().normal + transform.up).normalized;
            rgdBody.AddForce(result * controller.jumpStrength * 0.75f, ForceMode.VelocityChange);
            turning = false;
            exit = true;
        }

    }

    public override bool Exit()
    {
        if (exit)
        {
            prevNormal = currNormal;
            return true;
        }
        return false;
    }



}
