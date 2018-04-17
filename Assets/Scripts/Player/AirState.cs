using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : BaseState
{
    bool turning;

    // Use this for initialization
    void Start()
    {
        myStateType = MoveStates.AIR;
    }

    public override bool Enter()
    {
        if (!controller.onBottom)
        {
            controller.onGravityMultiplier = true;
            turning = false;
            return true;
        }
        return false;
    }

    public override void Run()
    {
        if (!turning)
        {
            if (controller.onForwardWall && Input.GetButtonDown("Jump"))
            {
                controller.onGravityMultiplier = false;
                EnableGravity(false);
                Jump(0);
                turning = true;
            }

            if (controller.onLeftWall || controller.onRightWall)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    AppendFinalMove(controller.HorizontalHit().normal * controller.jumpStrength);
                    Jump(controller.jumpHeight);
                }
            }

            Vector3 strafe = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            if (strafe != Vector3.zero)
            {
                strafe = transform.TransformDirection(strafe);
                AppendFinalMove(strafe * (controller.moveSpeed * Time.deltaTime));
            }

        }
        else if (TurnTowardsVector(controller.turnAroundSpeed, controller.HorizontalHit().normal))
        {
            JumpFromWall(controller.jumpHeight, controller.jumpStrength);
            turning = false;
            EnableGravity(true);
        }

    }

    public override bool Exit()
    {
        if (turning)
            return false;
        return true;
    }
}
