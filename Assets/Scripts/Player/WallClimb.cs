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
            rgdBody.velocity = Vector3.zero;
            rgdBody.useGravity = false;
            rgdBody.AddForce(transform.up * controller.jumpHeight, ForceMode.Impulse);

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
                rgdBody.velocity = Vector3.zero;
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
            Vector3 result = controller.HorizontalHit().normal + transform.up;
            rgdBody.AddForce(result.normalized * controller.jumpStrength, ForceMode.Impulse);
            timer += timeSpan;
            turning = false;
        }

    }

    public override bool Exit()
    {
        if (timer >= timeSpan)
        {
            prevNormal = currNormal;
            timer = 0;
            return true;
        }
        return false;
    }



}
