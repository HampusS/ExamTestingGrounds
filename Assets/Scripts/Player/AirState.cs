using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : BaseState
{
    bool turning;

    // Use this for initialization
    void Start()
    {
        myStateType = MoveStates.AIR;
    }

    public override bool Enter()
    {
        if (!controller.onBottom)
        {
            controller.onGravityMultiplier = true;
            rgdBody.useGravity = true;
            turning = false;
            return true;
        }
        return false;
    }

    public override void Run()
    {
        if (!turning)
        {
            Vector3 direction = transform.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"))).normalized;
            rgdBody.AddForce(direction * controller.moveSpeed * 0.25f, ForceMode.Acceleration);

            if (controller.onForwardWall && Input.GetButtonDown("Jump"))
            {
                controller.onGravityMultiplier = false;
                rgdBody.useGravity = false;
                rgdBody.velocity = Vector3.zero;
                turning = true;
            }

            if (controller.onLeftWall || controller.onRightWall)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    Vector3 result = transform.forward + controller.HorizontalHit().normal + transform.up;
                    rgdBody.velocity = Vector3.Project(rgdBody.velocity, result);
                }
            }

        }
        else if (TurnTowardsVector(controller.turnAroundSpeed, controller.HorizontalHit().normal))
        {
            Vector3 result = controller.HorizontalHit().normal + transform.up;
            rgdBody.AddForce(result.normalized * controller.jumpStrength, ForceMode.Impulse);
            turning = false;
        }

    }

    public override bool Exit()
    {
        if (turning)
            return false;
        return true;
    }
}
