using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : BaseState
{
    public float frictionCoefficient = 4;
    public float staticFrictionCoefficient = 4;
    float coyoteTime, coyoteLimit = 0.45f;

    Vector3 friction;
    CamStates camTilt;

    private void Start()
    {
        myStateType = MoveStates.GROUND;
        camTilt = Camera.main.GetComponent<CamStates>();
    }

    public override bool Enter()
    {
        if (controller.onBottom)
        {
            // New Entry on ground
            if (controller.prevMoveState != myStateType)
            {
                controller.CanJump = true;
                coyoteTime = 0;
                if (controller.prevMoveState != MoveStates.LEDGEGRAB)
                {
                    camTilt.onBump = true;
                    controller.ForceGravity = false;
                    rgdBody.velocity += new Vector3(-rgdBody.velocity.x * 0.5f, 0, -rgdBody.velocity.z * 0.5f);
                }
            }
            return true;
        }
        else if (controller.CanJump)
        {
            coyoteTime += Time.deltaTime;
            if (coyoteTime >= coyoteLimit)
            {
                controller.CanJump = false;
            }
        }
        return false;
    }

    public override void Run()
    {
        controller.isRunning = false;
        
        Vector3 direction = transform.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"))).normalized;
        if (controller.LockMovement)
            direction = Vector3.zero;
        if (direction != Vector3.zero)
            controller.isRunning = true;

        if (controller.Crouch)
        {
            if (rgdBody.velocity.magnitude > controller.crouchSpeed)
            {
                friction = new Vector3(-rgdBody.velocity.x, 0, -rgdBody.velocity.z) * 0.25f;

            }
            else
            {
                friction = new Vector3(-rgdBody.velocity.x, 0, -rgdBody.velocity.z) * frictionCoefficient;
                rgdBody.AddForce(direction * controller.crouchSpeed * 2, ForceMode.Acceleration);
            }
            rgdBody.AddForce(friction, ForceMode.Acceleration);
        }
        else
        {
            if (direction == Vector3.zero)
                friction = new Vector3(-rgdBody.velocity.x, 0, -rgdBody.velocity.z) * frictionCoefficient * staticFrictionCoefficient;
            else
            {
                friction = new Vector3(-rgdBody.velocity.x, 0, -rgdBody.velocity.z) * frictionCoefficient;
            }


            //if (controller.BottomRayHit().normal != Vector3.up)
            //{
            //    direction = Vector3.ProjectOnPlane(direction, controller.BottomRayHit().normal);
            //    friction = Vector3.ProjectOnPlane(friction, controller.BottomRayHit().normal);
            //}
            rgdBody.AddForce(direction * controller.moveSpeed, ForceMode.Acceleration);
            rgdBody.AddForce(friction, ForceMode.Acceleration);
        }

    }

    public override bool Exit()
    {
        return true;
    }

}
