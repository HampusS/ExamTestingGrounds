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
        if (controller.onForwardWall)
        {
            if (controller.onBottom)
                prevNormal = Vector3.zero;

            currNormal = controller.HorizontalHit().normal;

            if (currNormal != prevNormal && Input.GetButton("Jump") && Input.GetAxisRaw("Vertical") > 0)
            {
                GetComponent<Renderer>().material.color = Color.green;
                controller.Jump(runHeight);
                controller.UpdateMoveAmount(0, 0, Vector3.zero);
                controller.EnableGravity(false);
                controller.onGravityMultiplier = false;

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
                controller.Jump(0);
                turning = true;
            }

            if (Input.GetButton("Jump"))
                timer += Time.deltaTime;
            else
                timer += Time.deltaTime * 2;
        }
        else if (controller.TurnAroundForJump(jumpHeight, jumpStrength, rotateSpeed))
        {
            timer += timeSpan;
            turning = false;
        }

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
