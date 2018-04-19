using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : BaseState
{
    [SerializeField]
    float moveFloatiness = .15f;

    private void Start()
    {
        myStateType = MoveStates.GROUND;
    }

    public override bool Enter()
    {
        if (controller.onBottom)
        {
            //if (controller.prevMoveState != myStateType && controller.onBottom)
            //    UpdateMoveAmount(0.05f, Vector3.zero);
            return true;
        }
        return false;
    }

    public override void Run()
    {

        if (Input.GetButtonDown("Jump"))
        {
            //transform.position += Vector3.up * 0.15f;
            controller.Jump(controller.jumpHeight);
        }
    }

    public override bool Exit()
    {
        return true;
    }

}
