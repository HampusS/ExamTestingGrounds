using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMoveState : EnemyBase
{
    public float acceleration = 15;
    float deceleration;
    float animspeed;

    void Start()
    {
        taskType = EnemyTasks.MOVE;
        animspeed = navMesh.speed;
        deceleration = acceleration * 10;
    }

    public override bool Enter()
    {
        if (controller.InAggroRange() && controller.InAggroSight() && !navMesh.pathPending)
        {
            controller.Destination = controller.player.transform.position;
            navMesh.SetDestination(controller.Destination);
            return true;
        }
        return false;
    }

    public override void Run()
    {
        if (controller.anim != null)
        {
            //controller.anim.speed = controller.anim.speed*3;// * animspeed;
            controller.anim.SetBool("Walking", true);
        }

        if (Vector3.Dot((controller.player.transform.position - transform.position).normalized, controller.rgdBody.velocity.normalized) < 0.4f)
            navMesh.acceleration = acceleration;
        else
            navMesh.acceleration = deceleration;
    }

    public override bool Exit()
    {
        return true;
    }


}
