using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : BaseState
{
    [SerializeField]
    float moveFloatiness = .15f;
    [SerializeField]
    float sprint = 3;
    float speed;

    private void Start()
    {
        myStateType = MoveStates.GROUND;
        speed = controller.moveSpeed;
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
        speed = controller.moveSpeed;
        if (Input.GetButton("Action"))
        {
            speed *= sprint;
        }
        

        UpdateMoveInput(speed);
        controller.targetMove = TransformVector(controller.targetMove);
        controller.targetMove = ProjectVectorToPlane(controller.targetMove, controller.BottomRayHit().normal);
        UpdateMovement(moveFloatiness);

        if (Input.GetButtonDown("Jump"))
        {
            //transform.position += Vector3.up * 0.15f;
            Jump(controller.jumpHeight);
        }
    }

    public override bool Exit()
    {
        return true;
    }

}
