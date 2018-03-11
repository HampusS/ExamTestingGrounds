using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : BaseState
{
    [SerializeField]
    float speed = 3;
    [SerializeField]
    float moveFloatiness = 0.4f;

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
        if (!controller.onBottom && controller.moveStates != MoveStates.AIR)
        {
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
                controller.EnableGravity(false);
                controller.Jump(0);
                turning = true;
            }

            if (controller.onLeftWall || controller.onRightWall)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    controller.UpdateMoveAmount(controller.HorizontalHit().normal, jumpStrength);
                    controller.Jump(jumpHeight);
                }
            }
        }
        else if (controller.TurnAroundForJump(jumpHeight, jumpStrength, rotateSpeed))
        {
            turning = false;
            controller.EnableGravity(true);
        }
        //controller.MoveInAir(speed, moveFloatiness);
    }

    public override bool Exit()
    {
        if (turning)
            return false;
        return true;
    }
}
