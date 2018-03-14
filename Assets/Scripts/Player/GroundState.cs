using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : BaseState
{
    [SerializeField]
    float speed = 10;
    [SerializeField]
    float moveFloatiness = .15f;
    [SerializeField]
    float jumpForce = 300;
    
    
    private void Start()
    {
        myStateType = MoveStates.GROUND;
    }

    public override bool Enter()
    {
        if (controller.onBottom)
            return true;
        return false;
    }

    public override void Run()
    {
        Jump();
        SnapToGround();
        Vector3 strafe = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        controller.UpdateMoveAmount(speed, moveFloatiness, strafe);
    }

    public override bool Exit()
    {
        return true;
    }


    void SnapToGround()
    {
        float surfDist = Vector3.Distance(controller.BottomRayHit().point, transform.position);
        if (surfDist < controller.fullHeight * 0.6f)
        {
            transform.position += controller.BottomRayHit().normal * (controller.halfHeight - surfDist);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            transform.position += Vector3.up * (controller.fullHeight * 0.15f);
            controller.Jump(jumpForce);
        }
    }
}
