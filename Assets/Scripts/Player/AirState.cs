using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : BaseState
{
    [SerializeField]
    float speed = 3;

    float jumpStrength = 8f;
    float jumpHeight = 300;
    float rotateSpeed = 6;
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
                    AppendFinalMove(controller.HorizontalHit().normal * jumpStrength);
                    Jump(jumpHeight);
                }
            }

            Vector3 strafe = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            if (strafe != Vector3.zero)
            {
                strafe = transform.TransformDirection(strafe);
                AppendFinalMove(strafe * (speed * Time.deltaTime));
            }

        }
        else if (TurnTowardsVector(rotateSpeed, controller.HorizontalHit().normal))
        {
            JumpFromWall(jumpHeight, jumpStrength);
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
