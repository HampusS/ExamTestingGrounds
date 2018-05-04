using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMoveState : EnemyBase
{
    float animspeed;
    void Start()
    {
        taskType = EnemyTasks.MOVE;
        animspeed = navMesh.speed;
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
    }

    public override bool Exit()
    {
        return true;
    }


}
