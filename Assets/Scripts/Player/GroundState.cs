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
    
    float halfHeight, fullHeight;
    
    private void Start()
    {
        myStateType = MoveStates.GROUND;
        fullHeight = GetComponent<CapsuleCollider>().height;
        halfHeight = fullHeight * 0.5f;
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
        controller.UpdateMoveAmount(speed, moveFloatiness);
    }

    public override bool Exit()
    {
        return true;
    }


    void SnapToGround()
    {
        float surfDist = Vector3.Distance(controller.BottomRayHit().point, transform.position);
        if (surfDist < fullHeight * 0.6f)
        {
            transform.position += controller.BottomRayHit().normal * (halfHeight - surfDist);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            transform.position += Vector3.up * (fullHeight * 0.15f);
            controller.Jump(transform.up * jumpForce);
        }
    }
}
