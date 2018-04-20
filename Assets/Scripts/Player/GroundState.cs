using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : BaseState
{
    public float kineticFrictionCoefficient = 2;
    public float staticFrictionCoefficient = 4;
    Vector3 friction;

    private void Start()
    {
        myStateType = MoveStates.GROUND;
    }

    public override bool Enter()
    {
        if (controller.onBottom)
        {
            //rgdBody.useGravity = false;
            //controller.onGravityMultiplier = false;
            return true;
        }
        return false;
    }

    public override void Run()
    {
        if (Input.GetButtonDown("Jump"))
            rgdBody.AddForce(transform.up * controller.jumpHeight, ForceMode.Impulse);
        Vector3 direction = transform.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"))).normalized;

        if (rgdBody.velocity == Vector3.zero)
            friction = -rgdBody.velocity * staticFrictionCoefficient;
        else
            friction = -rgdBody.velocity * kineticFrictionCoefficient;
        
        if (controller.BottomRayHit().normal != Vector3.up)
        {
            direction = Vector3.ProjectOnPlane(direction, controller.BottomRayHit().normal);
            friction = Vector3.ProjectOnPlane(friction, controller.BottomRayHit().normal);
        }

        rgdBody.AddForce(direction * controller.moveSpeed, ForceMode.Acceleration);
        rgdBody.AddForce(friction, ForceMode.Acceleration);
        rgdBody.velocity = Vector3.ClampMagnitude(rgdBody.velocity, controller.maxSpeed);
    }

    public override bool Exit()
    {
        return true;
    }

}
