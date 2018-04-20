using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : BaseState
{

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
        rgdBody.AddForce(direction * controller.moveSpeed * 0.25f, ForceMode.Acceleration);
    }

    public override bool Exit()
    {
        return true;
    }
}
