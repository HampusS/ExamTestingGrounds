using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimb : BaseState
{
    float timer = 0;
    float timeSpan = 0.8f;
    float runHeight = 100;
    float jumpStrength = 8f;
    float jumpHeight = 300;
    float rotateSpeed = 6;
    bool turning;
    Vector3 prevNormal, currNormal;

    // Use this for initialization
    void Start()
    {
        myStateType = MoveStates.WALLCLIMB;
        turning = false;
    }

    public override bool Enter()
    {
        if (controller.onBottom)
            prevNormal = Vector3.zero;

        if (controller.onForwardWall)
        {

            currNormal = controller.HorizontalHit().normal;

            if (currNormal != prevNormal && Input.GetButton("Jump") && Input.GetAxisRaw("Vertical") > 0)
            {
                GetComponent<Renderer>().material.color = Color.green;
                controller.onGravityMultiplier = false;
                UpdateMoveAmount(0, Vector3.zero);
                EnableGravity(false);
                Jump(runHeight);
                turning = false;
                timer = 0;
                return true;
            }
        }

        return false;
    }

    public override void Run()
    {
        if (!turning)
        {
            if (timer > 0 && Input.GetButtonDown("Jump"))
            {
                Jump(0);
                turning = true;
            }

            if (Input.GetButton("Jump"))
                timer += Time.deltaTime;
            else
                timer += Time.deltaTime * 2;
        }
        else if (TurnTowardsVector(rotateSpeed, controller.HorizontalHit().normal))
        {
            JumpFromWall(jumpHeight, jumpStrength);
            timer += timeSpan;
            turning = false;
        }

    }

    public override bool Exit()
    {
        if (timer >= timeSpan || ReachForLedge())
        {
            GetComponent<Renderer>().material.color = Color.red;
            EnableGravity(true);
            prevNormal = currNormal;
            timer = 0;
            return true;
        }
        return false;
    }



}
