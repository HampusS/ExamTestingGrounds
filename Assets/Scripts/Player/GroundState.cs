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

    bool jump;
    float halfHeight, fullHeight;

    private void Start()
    {
        myStateType = MoveStates.GROUND;
        fullHeight = GetComponent<CapsuleCollider>().height;
        halfHeight = fullHeight * 0.5f;
    }

    void SnapToGround()
    {
        float surfDist = Vector3.Distance(controller.BottomRayHit().point, transform.position);
        if (surfDist < fullHeight * 0.6f)
        {
            transform.position += controller.BottomRayHit().normal * (halfHeight - surfDist);
        }
    }

    public override bool Enter()
    {
        if (controller.moveStates == myStateType)
        {
            return true;
        }
        return false;
    }

    public override void Run()
    {
        jump = Input.GetButtonDown("Jump");
        if (jump && controller.moveStates == MoveStates.GROUND)
        {
            transform.position += Vector3.up * (fullHeight * 0.1f);
            rgdBody.AddForce(transform.up * jumpForce);
            //controller.moveStates = MoveStates.AIR;
        }
        SnapToGround();
        controller.UpdateMoveAmount(speed, moveFloatiness);
    }

    public override bool Exit()
    {
        return true;
    }

}
