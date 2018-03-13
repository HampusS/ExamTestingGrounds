﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : BaseEnemyState
{
   
    public float maxSpeed;
    public float rotateSpeed;
    Rigidbody rbg;

    Vector3 dir;
    Vector3 desired;
    void Start()
    {
        myStateType = RocketState.CHASE;
    }
    public override bool Enter()
    {
        return true;
    }
    public override void Run()
    {
        Vector3 tar= controller.target.position;
        dir = controller.target.position - controller.rb.position;
        dir.Normalize();
        desired = dir * maxSpeed;
        controller.steering = -(desired - controller.rb.velocity);

        controller.steering = controller.steering * rotateSpeed;
        transform.rotation = Quaternion.Slerp(transform.rotation,
                                                     Quaternion.LookRotation(dir),
                                                     dir.magnitude * Time.deltaTime);
    }
    public override bool Exit()
    {
        return false;
    }

}
