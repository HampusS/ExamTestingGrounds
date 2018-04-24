using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : BaseState
{
    public float airResistance = 3;
    public float airSpeed = 5;
    Vector3 drag;

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
            return true;
        }
        return false;
    }

    public override void Run()
    {
        Vector3 direction = transform.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"))).normalized;
        rgdBody.AddForce(direction * airSpeed, ForceMode.Acceleration);

        //if (controller.onForceLockMovement)
        //    direction = Vector3.zero;
        Vector3 horiz = new Vector3(rgdBody.velocity.x, 0, rgdBody.velocity.z).normalized;
        //Debug.Log(Vector3.Dot(direction, horiz));
        if (Vector3.Dot(direction, horiz) < -0.5)
            drag = -horiz * airResistance * 100;
        else
            drag = -horiz * airResistance;
        rgdBody.AddForce(drag, ForceMode.Acceleration);
    }

    public override bool Exit()
    {
        return true;
    }
}
