using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimb : BaseState
{
    public float timeBeforeFall = 0.2f;
    float timer = 0;
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
            controller.onGravityMultiplier = true;
            rgdBody.useGravity = false;
            initOnce = false;
            turning = false;
            exit = false;
            timer = 0;
            rgdBody.velocity = Vector3.zero;
            rgdBody.AddForce(Vector3.up * controller.jumpHeight, ForceMode.VelocityChange);
        }
    }

    public override bool Enter()
    {
        if (controller.onBottom)
            prevNormal = Vector3.zero;

        if (controller.onForwardWall && !inReachOfLedge() && Vector3.Dot(controller.HorizontalHit().normal, transform.forward) < -0.9f)
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
            if (timer >= timeBeforeFall)
                rgdBody.useGravity = true;

            if (timer > 0 && Input.GetButtonDown("Jump"))
            {
                controller.onGravityMultiplier = false;
                rgdBody.useGravity = false;
                rgdBody.velocity = Vector3.zero;
                turning = true;
            }

            if (Input.GetButton("Jump"))
                timer += Time.deltaTime;
            else
                timer += Time.deltaTime * 2;

            if (controller.onBottom && rgdBody.velocity.y < 0)
                exit = true;

            if (inReachOfLedge())
                exit = true;
        }
        else if (TurnTowardsVector(controller.turnAroundSpeed, controller.HorizontalHit().normal))
        {
            Vector3 result = (controller.HorizontalHit().normal + transform.up).normalized * controller.jumpStrength;
            rgdBody.velocity = result;
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
