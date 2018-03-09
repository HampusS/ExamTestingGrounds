using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : BaseState
{
    [SerializeField]
    float speed = 3;
    [SerializeField]
    float moveFloatiness = 0.4f;

    // Use this for initialization
    void Start()
    {
        myStateType = MoveStates.AIR;
    }

    public override bool Enter()
    {
        if (!controller.onBottom && controller.moveStates != MoveStates.AIR)
            return true;
        return false;
    }

    public override void Run()
    {
        if (Input.GetButtonDown("Jump"))
            if (controller.onLeftWall || controller.onRightWall || controller.onForwardWall)
            {
                controller.Jump(controller.HorizontalHit().normal * 400);
                controller.Jump(transform.up * 200);
            }
        controller.UpdateMoveAmount(speed, moveFloatiness);
    }

    public override bool Exit()
    {
        return true;
    }
}
