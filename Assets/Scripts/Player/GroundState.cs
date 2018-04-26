using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : BaseState
{
    public float frictionCoefficient = 4;

    Vector3 friction;

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
        if (Input.GetButtonDown("Jump"))
            rgdBody.AddForce(transform.up * (controller.jumpHeight * 0.925f), ForceMode.VelocityChange);
        Vector3 direction = transform.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"))).normalized;

        if (controller.Crouch)
        {
            if (rgdBody.velocity.magnitude > controller.crouchSpeed)
                friction = -rgdBody.velocity * 0.25f;
            else
            {
                friction = -rgdBody.velocity * frictionCoefficient;
                rgdBody.AddForce(direction * controller.crouchSpeed * 2, ForceMode.Acceleration);
            }
            rgdBody.AddForce(friction, ForceMode.Acceleration);
        }
        else
        {
            if (direction == Vector3.zero)
                friction = -rgdBody.velocity * frictionCoefficient * 3;
            else
                friction = -rgdBody.velocity * frictionCoefficient;

            rgdBody.AddForce(direction * controller.moveSpeed, ForceMode.Acceleration);
            rgdBody.AddForce(friction, ForceMode.Acceleration);
        }

        //if (controller.BottomRayHit().normal != Vector3.up)
        //{
        //    direction = Vector3.ProjectOnPlane(direction, controller.BottomRayHit().normal);
        //    friction = Vector3.ProjectOnPlane(friction, controller.BottomRayHit().normal);
        //}
    }

    public override bool Exit()
    {
        return true;
    }

}
