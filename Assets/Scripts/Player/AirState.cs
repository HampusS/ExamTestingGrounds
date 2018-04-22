﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : BaseState
{
    public float airResistance = 3;
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

        //if (controller.onForceLockMovement)
        //    direction = Vector3.zero;

        rgdBody.AddForce(direction * controller.moveSpeed * 0.25f, ForceMode.Acceleration);
        drag = -direction * airResistance;
        rgdBody.AddForce(drag, ForceMode.Acceleration);
    }

    public override bool Exit()
    {
        return true;
    }
}
