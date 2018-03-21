using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : BaseEnemyState
{
    public float maxSpeed;
    public float rotateSpeed;
    public float speed = 50;

    Vector3 dir;
    Vector3 desired;
    void Start()
    {
        myStateType = RocketState.CHASE;
    }
    public override bool Enter()
    {
        if (!controller.Rayhit())
        {
            return true;
        }
        else
            return false;
    }
    public override void Run()
    {
        controller.targetVect = (controller.target.position - transform.position).normalized;
    }
    public override bool Exit()
    {
        if (controller.Rayhit())
        {
            return true;
        }
        else
            return false;
    }

}
