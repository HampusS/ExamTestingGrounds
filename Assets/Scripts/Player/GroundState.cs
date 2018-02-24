using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : BaseState
{
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    float speed = 10;
    [SerializeField]
    float moveFloatiness = .15f;
    [SerializeField]
    float jumpForce = 300;

    bool jump;

    private void Start()
    {
        myStateType = MoveStates.GROUND;
    }

    void SnapToGround()
    {
        float surfDist = Vector3.Distance(controller.BottomRayHit().point, transform.position);
        if (surfDist < GetComponent<CapsuleCollider>().height * 0.6f)
        {
            transform.position += controller.BottomRayHit().normal * ((GetComponent<CapsuleCollider>().height * 0.5f) - surfDist);
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
            transform.position += Vector3.up * (GetComponent<CapsuleCollider>().height * 0.15f);
            rgdBody.AddForce(transform.up * jumpForce);
            controller.moveStates = MoveStates.ERROR;
        }
        SnapToGround();
        controller.UpdateMoveAmount(speed, moveFloatiness);
    }

    public override bool Exit()
    {
        return true;
    }

}
