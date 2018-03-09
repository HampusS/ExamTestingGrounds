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
        if (controller.onBottom)
            return true;
        return false;
    }

    public override void Run()
    {
        if (Input.GetButtonDown("Jump"))
        {
            transform.position += Vector3.up * (GetComponent<CapsuleCollider>().height * 0.15f);
            rgdBody.AddForce(transform.up * jumpForce);
        }
        SnapToGround();
        controller.UpdateMoveAmount(speed, moveFloatiness);
    }

    public override bool Exit()
    {
        return true;
    }

}
